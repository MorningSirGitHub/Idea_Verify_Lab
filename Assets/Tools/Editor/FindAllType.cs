using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

/// <summary>
/// 按给定后缀从选中的文件夹查找该类型文件工具
/// </summary>
public class FindAllType : EditorWindow
{
    private GUIStyle m_GUIStyle = new GUIStyle();
    //private GUISkin m_GuiSkin = new GUISkin();
    //private Editor m_Editor = new Editor();
    //private string m_Path = string.Empty;
    private string m_Default = "*.";
    private string m_Extension = "dds";
    private List<Object> m_TargetList = new List<Object>();
    private Vector3 m_ScrollPos = Vector2.zero;

    [MenuItem("查找工具/查找所有指定后缀文件")]
    public static void Create()
    {
        FindAllType findAllTypeWin = EditorWindow.GetWindow<FindAllType>();
        findAllTypeWin.titleContent = new GUIContent("文件检索器");
        findAllTypeWin.minSize = new Vector2(500, 300);
    }

    void OnGUI()
    {
        GUILayout.Space(10);

        //if (GUILayout.Button("路径选择"))
        //{
        //    var path = EditorUtility.OpenFolderPanel("选择目标路径", "Assets", "Assets");
        //    string[] paths = Directory.GetFiles(path);
        //    for (int i = 0; i < paths.Length; i++)
        //    {
        //        Debug.LogError(paths[i]);
        //    }
        //}

        //GUILayout.Space(10);

        GUILayout.BeginHorizontal(m_GUIStyle);
        {
            m_Extension = EditorGUILayout.TextField("文件后缀名", m_Extension);

            GUILayout.Space(50);

            if (!GUILayout.Button("开始检索文件"))
            {
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.LabelField("文件列表:\t\t\t" + m_TargetList.Count);

                GUILayout.Space(10);

                m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, false, true);

                if (m_TargetList.Count == 0)
                    EditorGUILayout.LabelField("没有找到该类型文件！！");

                for (int i = 0; i < m_TargetList.Count; i++)
                    EditorGUILayout.ObjectField(m_TargetList[i], typeof(GameObject), true);

                EditorGUILayout.EndScrollView();

                return;
            }
        }

        m_TargetList.Clear();
        //string[] guids = AssetDatabase.FindAssets("t:prefab", Selection.assetGUIDs);
        string[] guids = Selection.assetGUIDs;
        foreach (var guid in guids)
        {
            var source = AssetDatabase.GUIDToAssetPath(guid);
            //Debug.LogError(source);
            var objList = Directory.GetFiles(source, m_Default + m_Extension, SearchOption.AllDirectories);
            //Debug.LogError(objList.Length);

            foreach (var o in objList)
            {
                //Debug.LogError(o);
                var obj = AssetDatabase.LoadAssetAtPath(o, typeof(Object));
                m_TargetList.Add(obj);
                EditorUtility.DisplayProgressBar("查找中 ...", o, ((float)m_TargetList.Count / (float)objList.Length));
                //string fileName = o.Substring(source.Length + 1);
            }

        }
        EditorUtility.ClearProgressBar();

        if (m_TargetList.Count == 0)
            EditorUtility.DisplayDialog("查找失败！", "选中的文件夹下没有 \"" + m_Extension + "\" 类型的文件！！\n请检查后缀名或者文件夹", "OK");
        else
            EditorUtility.DisplayDialog("查找成功！", "选中的文件夹下 \"" + m_Extension + "\" 类型的文件共有： " + m_TargetList.Count + " 个", "OK");

        //Object[] selects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        //if (selects == null)
        //{
        //    if (EditorUtility.DisplayDialog("警告！", "没有选中需要检索的文件夹！\n\n确定开始从根目录检索\n取消不执行本次操作", "确定", "取消"))
        //    {
        //        var path = Application.dataPath;
        //        Debug.LogError("你没有选中任何文件夹！！！");
        //    }
        //    return;
        //}

    }

}
