using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetBundle : Editor
{
    public static string[] m_ExtensionList = new string[] { "*.png", "*.prefab" };
    public static string AB_Name = "test";
    public static string AB_Variant = "tex";

    [MenuItem("我是AB打包按钮/自动标记打包！！")]
    static void Start()
    {
        var outPutPath = Application.streamingAssetsPath + "/AssetBundle";

        var p1 = "Assets/AssetsBundle/Texture";
        BuildAB(p1, "*.png", outPutPath);

        var p2 = "Assets/AssetsBundle/Prefabs";
        BuildAB(p2, "*.prefab", outPutPath);

        AssetDatabase.Refresh();
    }

    static void BuildAB(string targetPath, string extension, string outPutPath)
    {
        if (!Directory.Exists(targetPath))
        {
            Debug.LogError("没有该目录！！");
            EditorUtility.DisplayDialog("警告！", "没有找到目标目录！！", "OK");
            return;
        }

        DirectoryInfo directory = new DirectoryInfo(targetPath);
        FileInfo[] files = directory.GetFiles(extension, SearchOption.AllDirectories);
        Debug.LogError(files.Length);

        for (int i = 0; i < files.Length; i++)
        {
            var path = targetPath + "/" + files[i].Name;
            Debug.LogError(path);

            AssetImporter assetImporter = AssetImporter.GetAtPath(path);

            var index = files[i].Name.LastIndexOf(".");
            var name = files[i].Name.Remove(index);
            assetImporter.assetBundleName = name;

            //assetImporter.SetAssetBundleNameAndVariant(AB_Name, AB_Variant);
            //assetImporter.assetBundleName = AB_Name;
            //assetImporter.assetBundleVariant = AB_Variant;
        }

        if (!Directory.Exists(outPutPath))
            Directory.CreateDirectory(outPutPath);

        BuildPipeline.BuildAssetBundles(outPutPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
    }

    [MenuItem("我是AB打包按钮/手动标记打包")]
    static void Update()
    {
        var outPutPath = Application.streamingAssetsPath + "/AssetBundle";

        if (!Directory.Exists(outPutPath))
            Directory.CreateDirectory(outPutPath);

        BuildPipeline.BuildAssetBundles(outPutPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);

        AssetDatabase.Refresh();
    }

    [MenuItem("我是AB打包按钮/清除AB标记")]
    static void ClearABFlag()
    {
        var path = "Assets";

        for (int i = 0; i < m_ExtensionList.Length; i++)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles(m_ExtensionList[i], SearchOption.AllDirectories);

            for (int j = 0; j < files.Length; j++)
            {
                var index = files[j].Name.LastIndexOf(".");
                var name = files[j].Name.Remove(index).ToLower();
                Debug.LogError(name);
                AssetDatabase.RemoveAssetBundleName(name, true);
                EditorUtility.DisplayProgressBar("Clearing...", path + "/" + files[j].Name, (float)i / files.Length);
            }
            EditorUtility.ClearProgressBar();
        }
        AssetDatabase.RemoveUnusedAssetBundleNames();
        AssetDatabase.Refresh();
    }

}
