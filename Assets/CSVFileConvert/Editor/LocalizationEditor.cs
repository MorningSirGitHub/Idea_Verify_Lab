using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class LocalizationEditor : Editor
{
    /// <summary>
    /// 需要提取中文的文件后缀名
    /// </summary>
    static string[] _extension = new string[] { ".cs" };

    /// <summary>
    /// 查找中文的时候，需要忽略的行数
    /// 如果当前行中，存在下列列表中的任意一个，就会跳过，不提取
    /// </summary>
    static string[] _ignoreStringLine = new string[] { "Debug", "Exception", "//", "///", "[Tooltip", "[Header", "[MenuItem", "[TaskDescription", "Log", "EditorGUI", "Obsolete" };

    /// <summary>
    /// 目标文本。待替换的文本
    /// </summary>
    static List<string> AllTargetText = new List<string>();

    /// <summary>
    /// 除了代码的文本
    /// </summary>
    static List<string> ExceptCodeText = new List<string>();

    /// <summary>
    /// 代码中文本
    /// </summary>
    static List<string> CodeText = new List<string>();

    /// <summary>
    /// 每次的增量文件
    /// </summary>
    static List<string> DeltaText = new List<string>();

    /// <summary>
    /// 上一次保存的文件
    /// </summary>
    static List<string> OldTextList = new List<string>();

    /// <summary>
    /// 中文的正则表达式
    /// </summary>
    static Regex chiness = new Regex(@"[\u4e00-\u9fa5]");

    static Regex enter = new Regex(@"[\r\n]+");

    static Encoding ENCODING = Encoding.GetEncoding("GB2312");

    static string saveAllPath = Application.streamingAssetsPath + "/Data/Language/NewestTranslateText_All.xls";        //输出提出所有的中文文件路径
    static string saveCodePath = Application.streamingAssetsPath + "/Data/Language/NewestTranslateText_Code.xls";        //输出提出代码的中文文件路径
    static string saveSceneAndTablePath = Application.streamingAssetsPath + "/Data/Language/NewestTranslateText_SceneAndTable.xls";        //输出提出场景和表格的中文文件路径
    static string TextSource;

    [MenuItem("语言转换/抓取场景和代码中的中文")]
    static void TakeCodeAndSceneText()
    {
        TextSource = string.Empty;
        string dateTime = string.Empty;
        var time = System.DateTime.Now;
        dateTime = time.Year + "." + time.Month + "." + time.Day;
        string delataSavePath = Application.streamingAssetsPath + "/Data/Language/Delta." + dateTime + ".xls";        //输出提出的中文 差异化 文件路径
        OldTextList.Clear();
        AllTargetText.Clear();
        DeltaText.Clear();
        ExceptCodeText.Clear();
        CodeText.Clear();

        //先读取上次输出的文件，如果有的话
        bool _delateWrite;
        if (File.Exists(saveAllPath))
        {
            var oldLines = File.ReadAllLines(saveAllPath, ENCODING);
            foreach (var old in oldLines)
            {
                var temp = old.Split('\t');
                OldTextList.Add(temp[0]);
            }
            _delateWrite = true;
        }
        else
        {
            Debug.Log("没有找到之前生成的文件");
            _delateWrite = false;
        }

        //string path = Application.dataPath + "/Alive/Scripts";     //代码文件夹

        //var codes = GetFileInfo(new DirectoryInfo(path));
        //float process = 0;
        //foreach (var code in codes)
        //{
        //    var lines = File.ReadAllLines(code.FullName);
        //    foreach (var line in lines)
        //    {
        //        StringBuilder sb = new StringBuilder(line);
        //        GetCodeChinese(line);
        //    }
        //    EditorUtility.DisplayProgressBar("提取中···", "正在提取代码 "+code.Name, process / codes.Count);
        //}

        string codeFilePath = Application.dataPath + "/Alive/Scripts/_Common/Lang.cs";      //现在改为只要在一个代码文件中获取中文
        if (File.Exists(codeFilePath))
        {
            TextSource = "Lang.cs";
            var lines = File.ReadAllLines(codeFilePath, ENCODING);
            foreach (var line in lines)
            {
                //StringBuilder sb = new StringBuilder(line);
                GetCodeChinese(line);
            }
            // EditorUtility.DisplayProgressBar("提取中···", "正在提取代码 ", 0);
        }
        else
        {
            Debug.LogError("没有找到Lang.cs" + codeFilePath);
        }
        // EditorUtility.ClearProgressBar();


        string scenePath = Application.dataPath + "/Alive/Scenes";  //场景文件夹

        GetSceneChiniese(scenePath);

        //提取表格

        string tablePath = Application.streamingAssetsPath + "/Data/Csv";
        var fileList = new DirectoryInfo(tablePath).GetFiles();
        foreach (var file in fileList)
        {
            if (!file.FullName.EndsWith(".csv"))
            {
                continue;
            }
            TextSource = file.Name;
            CsvFile csv = new CsvFile(file.FullName, 1);
            var checkLine = csv.Lines[0]; //因为上一行跳过了一行，所以这里是第0行
            foreach (var line in csv.Lines)
            {
                for (int i = 0; i < line.rowValues.Count; i++)
                {
                    if (string.IsNullOrEmpty(checkLine.rowValues[i]))
                    {
                        continue;
                    }
                    var sb = line.rowValues[i];
                    string chinese = sb;
                    chinese = enter.Replace(chinese, "\\n");
                    //if(chinese.EndsWith("\t"))
                    //{
                    //    chinese = chinese.Remove(chinese.Length - 1); 
                    //}
                    if (chinese.Contains("\t"))
                    {
                        Debug.LogErrorFormat("当前字段中存在 TAB 空格，请检查 表明：{0}，本行第一个字段：{1},字段:{2}", file.Name, line.rowValues[0], chinese);
                        continue;
                    }
                    if (chiness.IsMatch(sb))
                    {
                        if (!OldTextList.Contains(chinese))
                        {
                            if (DeltaText.Contains(chinese))
                            {
                                DeltaText.Add(chinese);
                            }
                        }
                        if (!AllTargetText.Contains(chinese))
                        {
                            AllTargetText.Add(chinese + "\t" + TextSource);
                        }
                        if (!ExceptCodeText.Contains(chinese))
                        {
                            ExceptCodeText.Add(chinese);
                        }
                    }

                }
            }
        }

        //写入文件

        if (!_delateWrite)
        {
            DeltaText.Clear();
        }
        if (DeltaText.Count > 0)
        {
            //每次都把增量的放在文件的最后
            foreach (var text in DeltaText)
            {
                int index = AllTargetText.IndexOf(text);
                if (index != -1)
                {
                    AllTargetText.RemoveAt(index);
                }
            }
            foreach (var text in DeltaText)
            {
                if (!AllTargetText.Contains(text))
                {
                    AllTargetText.Add(text + "\t" + TextSource);
                }
            }
            File.WriteAllLines(delataSavePath, DeltaText.ToArray(), ENCODING);
        }
        File.WriteAllLines(saveAllPath, AllTargetText.ToArray(), ENCODING);
        File.WriteAllLines(saveSceneAndTablePath, ExceptCodeText.ToArray(), ENCODING);
        File.WriteAllLines(saveCodePath, CodeText.ToArray(), ENCODING);
        //File.WriteAllBytes(savePath,Encoding.Convert(Encoding.GetEncoding("GB2312"), Encoding.UTF8, File.ReadAllBytes(savePath)));






        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("提示", "提取完成了呢~~~  一个共提取:\r\n" + AllTargetText.Count + "个中文\r\n" + DeltaText.Count + "个新字符", "好的呢", "");

    }

    [MenuItem("语言转换/获取代码中中文的翻译")]
    static void GetCodeTranslate()
    {
        var all = File.ReadAllLines(saveAllPath, Encoding.GetEncoding("GB2312"));
        var codes = File.ReadAllLines(saveCodePath, Encoding.GetEncoding("GB2312"));
        Dictionary<string, string> allDic = new Dictionary<string, string>();
        List<string> codeDic = new List<string>();
        foreach (var str in all)
        {
            var temp = str.Split('\t');
            if (temp.Length == 1)
            {
                Debug.LogErrorFormat("这一行没有翻译：{0}", temp[0]);
                continue;
            }
            else if (temp.Length == 2)
            {
                if (string.IsNullOrEmpty(temp[1]))
                {
                    Debug.LogErrorFormat("这一行没有翻译：{0}", temp[0]);
                }

                if (allDic.ContainsKey(temp[0]))
                {
                    Debug.LogErrorFormat("这一行又重复：{0}", temp[0]);
                }
                else
                {
                    allDic.Add(temp[0], temp[1]);
                }
            }

        }
        foreach (var str in codes)
        {
            var temp = str.Split('\t');
            if (string.IsNullOrEmpty(str))
            {
                continue;
            }
            if (allDic.ContainsKey(temp[0]))
            {
                codeDic.Add(temp[0] + "\t" + allDic[temp[0]]);
            }
            else
            {
                Debug.LogErrorFormat("这一行没有翻译：{0}", temp[0]);
            }
        }
        File.WriteAllLines(saveCodePath, codeDic.ToArray(), Encoding.GetEncoding("GB2312"));
    }

    static Dictionary<string, string> ContentList = new Dictionary<string, string>();

    [MenuItem("语言转换/读取并且替换场景和表格文本")]
    static void ConvertText()
    {
        string savePath = Application.streamingAssetsPath + "/Data/Language/NewestTranslateText_All.xls";

        var lines = File.ReadAllLines(savePath, Encoding.GetEncoding("GB2312"));       //因为直接xcel保存过的文件会自动变成 GB2312 编码

        if (lines.Length <= 0)
        {
            Debug.LogErrorFormat("翻译文件没找到：{0}", savePath);
            return;
        }

        foreach (var line in lines)
        {
            var temp = line.Split('\t');
            if (temp.Length == 1)
            {
                Debug.LogErrorFormat("这一行没有翻译：{0}", temp[0]);
                continue;
            }
            if (ContentList.ContainsKey(temp[0]))
            {
                Debug.LogError("存在相同的KEY  " + temp[0]);
                continue;
            }
            ContentList.Add(temp[0], temp[1]);
            //Debug.Log(temp[1]);
        }
        Debug.Log(ContentList.Count);

        //ConvertCode(Application.dataPath);
        ConvertScenes(Application.dataPath + "/Alive/Scenes");
        ConverTable();

        EditorUtility.DisplayDialog("提示", "替换完成了呢~", "好的呢", "");
    }

    #region 提取

    static void GetCodeChineseInFile(string path)
    {

    }

    /// <summary>
    /// 获取代码中的中文
    /// </summary>
    /// <param name="sb"></param>
    static void GetCodeChinese(string sb)
    {
        if (sb.Contains("\""))
        {
            var chinese = ExtractQuotes(ref sb);
            if (chinese != string.Empty)
            {
                if (!AllTargetText.Contains(chinese))
                {
                    if (!OldTextList.Contains(chinese))
                    {
                        DeltaText.Add(chinese);
                    }
                    AllTargetText.Add(chinese + "\t" + TextSource);
                }
                if (!CodeText.Contains(chinese))
                {
                    CodeText.Add(chinese);
                }
                else
                {
                    Debug.LogErrorFormat("代码中有相同的字符： {0}  请合并 ", chinese);
                }
                if (HasChinese(sb))
                {
                    Debug.LogError("这行存在多个引号分割的中文字符    " + sb);
                }
            }

        }
    }

    /// <summary>
    /// 获取代码中的中文
    /// </summary>
    /// <param name="sb"></param>
    static void GetCodeChineseForCheck(string sb, string fileName)
    {
        for (int i = 0; i < 10; i++)
        {
            if (sb.Contains("\""))
            {
                var chinese = ExtractQuotes(ref sb);
                if (chinese != string.Empty)
                {
                    if (!AllTargetText.Contains(chinese))
                    {
                        AllTargetText.Add(chinese + "\t" + fileName);
                    }
                    if (HasChinese(sb))
                    {
                        Debug.LogError("这行存在多个引号分割的中文字符    " + sb);
                    }
                }
            }
            else
            {
                if (i >= 9)
                {
                    Debug.LogError("这一行存在10个以上的分号？？");
                }
                break;
            }
        }
    }

    /// <summary>
    /// 获取场景中的中文
    /// </summary>
    /// <param name="scenesPathRoot"></param>
    static void GetSceneChiniese(string scenesPathRoot)
    {
        List<string> scenesPath = new List<string>();
        GetScenesInPath(scenesPathRoot, scenesPath);
        foreach (var path in scenesPath)
        {
            TextSource = path;
            var scene = EditorSceneManager.OpenScene(path);
            foreach (var obj in scene.GetRootGameObjects())
            {
                GetUITextInScene(obj.transform);
            }
        }
    }

    /// <summary>
    /// 获取场景中UI的中午
    /// </summary>
    /// <param name="go"></param>
    static void GetUITextInScene(Transform go)
    {
        Text text = go.GetComponent<Text>();
        if (text != null)
        {
            if (!string.IsNullOrEmpty(text.text))
            {
                string str = text.text;
                //if (str.Contains("\r"))
                //{
                //    str = str.Replace("\r", "\\r"); ///去掉最末尾的回车符
                //}
                //if (str.Contains("\n"))
                //{
                //    str = text.text.Replace("\n", "\\n"); ///去掉最末尾的回车符
                //}
                str = enter.Replace(str, "\\n");

                if (HasChinese(str))
                {
                    //str = str.Replace("\r", "");
                    if (!OldTextList.Contains(str))
                    {
                        if (!OldTextList.Contains(str))
                        {
                            if (!DeltaText.Contains(str))
                            {
                                DeltaText.Add(str);
                            }
                        }
                    }
                    if (!AllTargetText.Contains(str))
                    {
                        AllTargetText.Add(str + "\t" + TextSource);
                    }
                    if (!ExceptCodeText.Contains(str))
                    {
                        ExceptCodeText.Add(str);
                    }
                    Debug.Log(str);
                }
            }
        }
        if (go.childCount > 0)
        {
            for (int i = 0; i < go.childCount; i++)
            {
                GetUITextInScene(go.GetChild(i));
            }
        }
    }

    /// <summary>
    /// 在地址中获取场景文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="scenes"></param>
    static void GetScenesInPath(string path, List<string> scenes)
    {
        DirectoryInfo direc = new DirectoryInfo(path);
        foreach (var file in direc.GetFiles())
        {
            if (file.FullName.EndsWith(".unity"))
            {
                string scenePath = file.FullName;
                int index = scenePath.IndexOf("Assets");
                scenePath = scenePath.Remove(0, index);
                var target = scenePath[6];
                scenePath = scenePath.Replace('\\', '/');
                Debug.Log(scenePath);
                //var scene = EditorSceneManager.OpenScene(scenePath);
                scenes.Add(scenePath);
            }
        }
    }

    /// <summary>
    /// 提取字符串中引号隔开的中文字符
    /// 并且在原字符串中移除该中文
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    static string ExtractQuotes(ref string content)
    {
        string chinese = string.Empty;
        //TODO: 这里可以尝试使用stringBuilder 和 正则表达式，来代替 stirng.Contains
        //Tips: 虽然使用StringBuilder可以减少GC 但是实际测试中发现 String.Contains速度比他快很多，所以在编辑器代码还是选用String.Contains更合适
        foreach (var ignore in _ignoreStringLine)
        {
            if (content.Contains(ignore))
            {
                return chinese;
            }
        }
        int start = content.IndexOf("\"");
        if (start != -1)
        {
            int end = content.IndexOf("\"", start + 1);
            if (end != -1)
            {
                var check = content.Substring(start + 1, end - start - 1);
                if (HasChinese(check))
                {
                    chinese = check;
                    Debug.Log(check);
                }
                content = content.Remove(start, end - start + 1);
            }

        }
        return chinese;
    }

    /// <summary>
    /// 获取文件夹中所有文件
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    static List<FileInfo> GetFileInfo(DirectoryInfo info)
    {
        List<FileInfo> files = new List<FileInfo>();
        foreach (var child in info.GetDirectories())
        {
            files.AddRange(FilterFileInfo(child.GetFiles()));
            if (child.GetDirectories().Length != 0)
            {
                files.AddRange(FilterFileInfo(GetFileInfo(child).ToArray()));
            }
        }
        return files;
    }

    /// <summary>
    /// 提取文件列表中所有符合条件的文件
    /// </summary>
    /// <param name="infos"></param>
    /// <returns></returns>
    static List<FileInfo> FilterFileInfo(FileInfo[] infos)
    {
        List<FileInfo> files = new List<FileInfo>();
        foreach (var file in infos)
        {
            foreach (var extension in _extension)
            {
                if (file.FullName.EndsWith(extension))
                {
                    files.Add(file);
                    break;
                }
            }
        }
        return files;
    }

    #endregion 

    /// <summary>
    /// 字符串中是否含有中文
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    static bool HasChinese(string str)
    {
        return chiness.IsMatch(str);
    }

    #region 转换

    /// <summary>
    /// 转换文件夹下所有代码中的中文
    /// </summary>
    /// <param name="sourcePath"></param>
    static void ConvertCode(string sourcePath)
    {
        string path = Application.dataPath;

        var codes = GetFileInfo(new DirectoryInfo(path));
        foreach (var code in codes)
        {
            var lines = File.ReadAllLines(code.FullName);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                ChangeCodeText(ref line);
            }
            File.WriteAllLines(code.FullName, lines);      //替换代码
        }
    }

    /// <summary>
    /// 转换当前行代码中的中文
    /// </summary>
    /// <param name="content"></param>
    static void ChangeCodeText(ref string content)
    {
        //TODO: 这里可以尝试使用stringBuilder 和 正则表达式，来代替 stirng.Contains
        //Tips: 虽然使用StringBuilder可以减少GC 但是实际测试中发现 String.Contains速度比他快很多，所以在编辑器代码还是选用String.Contains更合适
        foreach (var ignore in _ignoreStringLine)
        {
            if (content.Contains(ignore))
            {
                return;
            }
        }
        int start = content.IndexOf("\"");
        if (start != -1)
        {
            int end = content.IndexOf("\"", start + 1);
            var check = content.Substring(start + 1, end - start - 1);
            if (HasChinese(check))
            {
                if (ContentList.ContainsKey(check))
                {
                    content = content.Replace(check, ContentList[check]);
                    Debug.Log(content);
                }
            }
        }
    }

    /// <summary>
    /// 转换所有场景的中文
    /// </summary>
    /// <param name="scenesPathRoot"></param>
    static void ConvertScenes(string scenesPathRoot)
    {
        List<string> scenesPath = new List<string>();
        GetScenesInPath(scenesPathRoot, scenesPath);
        foreach (var path in scenesPath)
        {
            var scene = EditorSceneManager.OpenScene(path);
            foreach (var obj in scene.GetRootGameObjects())
            {
                ConvertScenesText(obj.transform);
            }
            EditorSceneManager.SaveScene(scene);
        }
    }

    /// <summary>
    /// 转换当前GameObject中的中文
    /// </summary>
    /// <param name="go"></param>
    static void ConvertScenesText(Transform go)
    {
        Text text = go.GetComponent<Text>();
        if (text != null)
        {
            if (HasChinese(text.text))
            {
                if (ContentList.ContainsKey(text.text))
                {
                    Debug.LogFormat("替换：{0} to {1}", text.text, ContentList[text.text]);
                    text.text = ContentList[text.text];   //替换代码
                }
                // Debug.Log(text.text);
            }
        }
        if (go.childCount > 0)
        {
            for (int i = 0; i < go.childCount; i++)
            {
                ConvertScenesText(go.GetChild(i));
            }
        }
    }

    static void ConverTable()
    {
        string tablePath = Application.streamingAssetsPath + "/Data/Csv";
        var fileList = new DirectoryInfo(tablePath).GetFiles();
        foreach (var file in fileList)
        {
            if (!file.FullName.EndsWith(".csv"))
            {
                continue;
            }
            CsvFile csv = new CsvFile(file.FullName, 1);
            foreach (var line in csv.Lines)
            {
                for (int i = 0; i < line.rowValues.Count; i++)
                {
                    var sb = line.rowValues[i];
                    string chinese = sb;
                    chinese = enter.Replace(chinese, "\\n");
                    //if(chinese.EndsWith("\t"))
                    //{
                    //    chinese = chinese.Remove(chinese.Length - 1); 
                    //}
                    if (chinese.Contains("\t"))
                    {
                        Debug.LogErrorFormat("当前字段中存在 TAB 空格，请检查 表明：{0}，本行第一个字段：{1},字段:{2}", file.Name, line.rowValues[0], chinese);
                        continue;
                    }
                    if (chiness.IsMatch(sb))
                    {
                        if (ContentList.ContainsKey(chinese))
                        {
                            line.rowValues[i] = ContentList[chinese];
                        }
                    }

                }
            }
            csv.Save();
        }
    }

    #endregion

    [MenuItem("语言转换/这是一个测试按钮")]
    static void Test()
    {
        // Lang.Instance.Convert();

        // var allItems = Lang.Instance.GetAllTips(Lang.Instance);


        //// Debug.Log(Lang.Instance.str1);
        // string savePath = Application.dataPath + "/Alive/Content/Language/NewestTranslateText.xlsx";

        // var lines = File.ReadAllLines(savePath, Encoding.GetEncoding("GB2312"));

        // if (lines.Length <= 0)
        // {
        //     Debug.LogErrorFormat("翻译文件没找到：{0}", savePath);
        //     return;
        // }

        // foreach (var line in lines)
        // {
        //     var temp = line.Split('\t');
        //     if (temp.Length == 1)
        //     {
        //         Debug.LogErrorFormat("这一行没有翻译：{0}", temp[0]);
        //         continue;
        //     }
        //     if (ContentList.ContainsKey(temp[0]))
        //     {
        //         Debug.LogError("存在相同的KEY  " + temp[0]);
        //         continue;
        //     }
        //     ContentList.Add(temp[0], temp[1]);
        //     //Debug.Log(temp[1]);
        // }

        // string[] keyList = new string[allItems.Count];
        // string[] valueList = new string[allItems.Count];
        // allItems.Keys.CopyTo(keyList, 0);
        // allItems.Values.CopyTo(valueList, 0);

        // foreach(var key in keyList)
        // {
        //     if(ContentList.ContainsKey(allItems[key]))
        //     {
        //         allItems[key] = ContentList[allItems[key]];
        //     }
        //     else
        //     {
        //         Debug.LogErrorFormat("存在没有找到翻译的字段：{0}", allItems[key]);
        //     }
        // }

        // Lang.Instance.LoadLangPreference(allItems, Lang.Instance);
        // Debug.Log(Lang.str1);

        string path = Application.streamingAssetsPath + "/Data/Csv";
        var files = new DirectoryInfo(path).GetFiles();
        foreach (var file in files)
        {
            if (file.FullName.EndsWith(".csv"))
            {
                var lines = File.ReadAllLines(file.FullName, Encoding.GetEncoding("GB2312"));
                string savePath = file.FullName.Replace(@"\Csv", @"\Csv\Temp");
                File.WriteAllLines(savePath, lines, Encoding.UTF8);
            }
        }
        AssetDatabase.Refresh();
        Debug.Log("测试结束");
    }

    [MenuItem("语言转换/提取代码中的文本用于检查")]
    static void GetCodeTextForCheck()
    {
        AllTargetText.Clear();
        string savePath = Application.streamingAssetsPath + "/Data/Language/NewestTranslateText_Check.xls";        //输出提出所有的中文文件路径
        string path = Application.dataPath + "/Alive/Scripts";     //代码文件夹

        var codes = GetFileInfo(new DirectoryInfo(path));
        //float process = 0;
        foreach (var code in codes)
        {
            if (code.Name == "Lang.cs")
            {
                continue;
            }
            var lines = File.ReadAllLines(code.FullName);
            foreach (var line in lines)
            {
                StringBuilder sb = new StringBuilder(line);
                GetCodeChineseForCheck(line, code.Name);
            }
            // EditorUtility.DisplayProgressBar("提取中···", "正在提取代码 " + code.Name, process / codes.Count);
        }
        File.WriteAllLines(savePath, AllTargetText.ToArray(), ENCODING);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("提示", "提取完成了呢~~~  一个共提取:\r\n" + AllTargetText.Count + "个中文\r\n", "好的呢", "");
    }

    [MenuItem("保存数据到CSV/试一试")]
    static void SaveDataWithCSV()
    {
        string scenePath = Application.dataPath + "/Scenes";

        GetSceneChiniese(scenePath);

        var path = Application.streamingAssetsPath + "/Data/Csv/Test.csv";

        CsvFile csv = new CsvFile(path);

        csv.Add(AllTargetText);

        csv.Save();
    }

}
