using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Object = UnityEngine.Object;

public class EditorTools
{

    #region ProgressBar

    private static float progress = 0, totalProgress = 0;
    public static void UpdateProgressBar(string title, string info)
    {
        EditorUtility.DisplayProgressBar(title, info, ++progress / totalProgress);
    }
    public static void UpdateProgressBar(string title, string format, params object[] args)
    {
        EditorUtility.DisplayProgressBar(title, string.Format(format, args), ++progress / totalProgress);
    }
    public static void ResetProgressBar(int total = 0)
    {
        progress = 0;
        totalProgress = total;
        EditorUtility.ClearProgressBar();
    }

    #endregion

    #region EditorWindow

    public static PathSelectWindow CreatePathSelectWindow(string title, Vector2 minsize, string searchPath = "", bool isSingleChoice = true)
    {
        var pathSelectWindow = EditorWindow.GetWindow<PathSelectWindow>();
        pathSelectWindow.titleContent = new GUIContent(title);
        pathSelectWindow.minSize = minsize;
        pathSelectWindow.Show();
        pathSelectWindow.SearchPath = searchPath;
        pathSelectWindow.IsSingleChoice = isSingleChoice;
        return pathSelectWindow;
    }

    #endregion

}

public class PathSelectWindow : EditorWindow
{
    private List<Object> m_PathList = new List<Object>();
    private List<string> m_OutputList = new List<string>();
    private List<bool> m_PathSelect = new List<bool>();
    private Vector2 m_ScrollPos = Vector2.zero;
    private Action<List<Object>, List<string>> m_CloseCallback;
    private bool m_IsCompiling = false;

    public bool IsSingleChoice { get; set; }
    private string m_SearchPath;
    public string SearchPath
    {
        get { return m_SearchPath; }
        set
        {
            m_SearchPath = value;
            UpdateWindow();
        }
    }
    private string m_OutputPath;
    public string OutputPath
    {
        get { return m_OutputPath; }
        set
        {
            m_OutputPath = value;
            UpdateWindow();
        }
    }
    public void SetCloseListener(Action<List<Object>, List<string>> callback, bool isClear = true)
    {
        if (isClear)
            m_CloseCallback = callback;
        else
            m_CloseCallback += callback;
    }
    public void UpdateWindow()
    {
        m_PathList.Clear();
        m_OutputList.Clear();
        m_PathSelect.Clear();
        if (string.IsNullOrEmpty(m_SearchPath))
            return;

        var pathList = AssetDatabase.GetSubFolders(m_SearchPath);
        EditorTools.ResetProgressBar(pathList.Length);
        foreach (var path in pathList)
        {
            m_PathSelect.Add(false);
            var obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            m_OutputList.Add(string.Format("{0}/{1}/{2}.png", m_OutputPath, obj.name, obj.name.ToLower()));
            m_PathList.Add(obj);
            EditorTools.UpdateProgressBar("Path Find ...", path);
        }
        EditorTools.ResetProgressBar();
    }

    void OnGUI()
    {
        GUILayout.Space(10);
        if (m_IsCompiling)
        {
            EditorGUILayout.HelpBox("编译完成后自动刷新，请等待 ...", MessageType.Error);
            return;
        }

        EditorGUILayout.HelpBox(titleContent.text + string.Format(" [{0}选]", IsSingleChoice ? "单" : "多"), MessageType.Warning);
        EditorGUILayout.HelpBox("路径数量:\t\t" + m_PathList.Count, MessageType.Info);

        GUILayout.BeginVertical("GroupBox");
        {
            m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, false, false);
            {
                if (m_PathList.Count == 0)
                {
                    EditorGUILayout.HelpBox("路径下没有子文件夹！！\n请重新选择路径！！", MessageType.Error);
                }

                for (int i = 0; i < m_PathList.Count; i++)
                {
                    var pathObj = m_PathList[i];
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.BeginVertical();
                        {
                            var select = EditorGUILayout.ToggleLeft(pathObj.name, m_PathSelect[i], GUILayout.Width(60));
                            if (select != m_PathSelect[i])
                            {
                                if (IsSingleChoice)
                                    ResetSelect(i);
                                else
                                    m_PathSelect[i] = select;
                            }
                            if (GUILayout.Button("更改", GUILayout.Width(50)))
                            {
                                var path = EditorUtility.SaveFilePanel("Select Save Path", m_OutputPath, "default", "png");
                                if (!string.IsNullOrEmpty(path))
                                {
                                    path = path.Replace("\\", "/");
                                    var startIndex = path.IndexOf("Assets");
                                    m_OutputList[i] = path.Substring(startIndex);
                                }
                            }
                        }
                        GUILayout.EndVertical();

                        GUILayout.BeginVertical();
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Input  :", GUILayout.Width(50));
                                EditorGUILayout.ObjectField(pathObj, typeof(GameObject), false);
                            }
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Output:", GUILayout.Width(50));
                                Object outputObj = null;
                                var ouputPath = m_OutputList[i];
                                if (!string.IsNullOrEmpty(ouputPath))
                                {
                                    ouputPath = ouputPath.Remove(ouputPath.LastIndexOf("/"));
                                    outputObj = AssetDatabase.LoadAssetAtPath(ouputPath, typeof(Object));
                                }
                                EditorGUILayout.ObjectField(outputObj, typeof(GameObject), false);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        GUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(10);
                }
            }
            EditorGUILayout.EndScrollView();
        }
        GUILayout.EndVertical();

        if (GUILayout.Button("我选好了"))
        {
            Close();
        }
        GUILayout.Space(10);
    }
    private void ResetSelect(int index = -1)
    {
        for (int i = 0; i < m_PathSelect.Count; i++)
        {
            m_PathSelect[i] = index == i;
        }
    }
    private void GetSelectPath(out List<Object> objList, out List<string> outputList)
    {
        objList = new List<Object>();
        outputList = new List<string>();
        for (int i = 0; i < m_PathList.Count; i++)
        {
            if (!m_PathSelect[i])
                continue;

            objList.Add(m_PathList[i]);
            outputList.Add(m_OutputList[i]);
        }
    }
    private void OnEnable()
    {
        CompilationPipeline.assemblyCompilationStarted += OnCompilationStarted;
        CompilationPipeline.assemblyCompilationFinished += OnCompilationFinished;
    }
    private void OnCompilationStarted(string assemblyName)
    {
        m_IsCompiling = true;
        ShowNotification(new GUIContent("编译中 ..."));
    }
    private void OnCompilationFinished(string assemblyName, CompilerMessage[] compilerMessages)
    {
        m_IsCompiling = false;
        RemoveNotification();
        // NOTE: 可以根据窗口的需要进行过滤
        //if (assemblyName != "Library/ScriptAssemblies/Assembly-CSharp-Editor.dll")
        //    return;

        UpdateWindow();
    }
    private void OnDestroy()
    {
        CompilationPipeline.assemblyCompilationStarted -= OnCompilationStarted;
        CompilationPipeline.assemblyCompilationFinished -= OnCompilationFinished;

        if (m_CloseCallback != null)
        {
            List<Object> objList;
            List<string> outputList;
            GetSelectPath(out objList, out outputList);
            m_CloseCallback(objList, outputList);
        }
    }
}
