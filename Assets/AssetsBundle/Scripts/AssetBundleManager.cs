using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AssetBundleManager : MonoBehaviour
{
    string Url = "/AssetBundle";

    void Start()
    {
        //StartCoroutine(ParseAB("1228iconc512"));
        //StartCoroutine(ParsePrefabs("cube"));
        StartCoroutine(ParsePrefabs("sphere"));
    }

    IEnumerator ParsePrefabs(string name)
    {
        //var path = GetBundleSourceFile(name);
        var path = @"http://localhost/AssetBundle/" + name;

        WWW www = WWW.LoadFromCacheOrDownload(path, 0);

        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError(www.error);
            yield break;
        }

        var ab = www.assetBundle;

        var manifestAB = AssetBundle.LoadFromFile("Assets/StreamingAssets/AssetBundle/AssetBundle");
        AssetBundleManifest manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        foreach (var n in manifest.GetAllDependencies(name))
        {
            AssetBundle.LoadFromFile("Assets/StreamingAssets/AssetBundle/" + n);
        }

        var go = ab.LoadAllAssets<GameObject>();

        for (int i = 0; i < go.Length; i++)
        {
            Debug.LogError(go[i].name);
            Instantiate(go[i]);
        }
    }

    IEnumerator ParseAB(string name)
    {
        //var path = GetBundleSourceFile(name);
        var path = @"http://localhost/AssetBundle/" + name;

        WWW www = WWW.LoadFromCacheOrDownload(path, 0);

        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError(www.error);
            yield break;
        }

        var ab = www.assetBundle;

        var sprites = ab.LoadAllAssets<Texture>();

        for (int i = 0; i < sprites.Length; i++)
        {
            Debug.LogError(sprites[i].name);
            this.GetComponent<UnityEngine.UI.RawImage>().texture = sprites[i];
        }

        //var manifest = ab.LoadAsset("AssetBundleManifest");

        //string[] fullnames = ab.GetDirectDependencies(fullname);

    }

    /// <summary>
    /// AB 保存的路径相对于 Assets/StreamingAssets 的名字
    /// </summary>
    public string BundleSaveDirName { get { return "AssetBundle"; } }

    /// <summary>
    /// 获取 AB 源文件路径（打包进安装包的）
    /// </summary>
    /// <param name="path"></param>
    /// <param name="forWWW"></param>
    /// <returns></returns>
    public string GetBundleSourceFile(string path, bool forWWW = true)
    {
        string filePath = null;
#if UNITY_EDITOR
        if (forWWW)
            filePath = string.Format("file://{0}/StreamingAssets/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
        else
            filePath = string.Format("{0}/StreamingAssets/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
#elif UNITY_ANDROID
            if (forWWW)
                filePath = string.Format("jar:file://{0}!/assets/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
            else
                filePath = string.Format("{0}!assets/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
#elif UNITY_IOS
            if (forWWW)
                filePath = string.Format("file://{0}/Raw/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
            else
                filePath = string.Format("{0}/Raw/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
#else
            throw new System.NotImplementedException();
#endif
        return filePath;
    }

}
