using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(SkillInfo))]
public class SkillInfoEditor : EditorWindow
{
    private static string path = "Assets/SkillInfo";

    private static string name = "New SkillInfo";

    private static SkillInfo skillInfo;

    private Editor editor;

    [MenuItem("创建技能信息/SkillInfo Window")]
    public static void SkillInfoWindow()
    {
        var window = EditorWindow.GetWindow<SkillInfoEditor>(true, "SkillInfo Window", true);
        // 直接根据ScriptableObject构造一个Editor
        //skillInfo = Create(path, name);
        window.editor = Editor.CreateEditor(skillInfo = new SkillInfo());
    }

    private void OnGUI()
    {
        this.editor.OnInspectorGUI();

        name = GUILayout.TextField(name);
        if (GUILayout.Button("确定"))
        {
            TTT(path, name);
        }
    }

    void TTT(string assetPath, string assetName)
    {
        bool doCreate = true;
        string path = Path.Combine(assetPath, assetName + ".asset");
        FileInfo fileInfo = new FileInfo(path);
        if (fileInfo.Exists)
        {
            doCreate = EditorUtility.DisplayDialog(assetName + " already exists.",
                                                    "Do you want to overwrite the old one?",
                                                    "Yes", "No");
        }
        if (doCreate)
        {
            SkillInfo SkillInfo = Create(assetPath, assetName);
            Selection.activeObject = SkillInfo;
            EditorGUIUtility.PingObject(SkillInfo);
        }
    }

    //SkillInfo skillInfo;

    //private void OnEnable()
    //{
    //    skillInfo = target as SkillInfo;
    //}

    //public override void OnInspectorGUI()
    //{
    //    //EditorGUI.DrawPreviewTexture(new Rect(20, 20, 40, 40), GetTextureByType(skillInfo.SkillType));

    //    base.OnInspectorGUI();
    //}

    [MenuItem("Assets/Create/SkillInfo")]
    static void CCC()
    {
        string assetName = "New SkillInfo";
        string assetPath = "Assets/SkillInfo";
        //if (Selection.activeObject)
        //{
        //    assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        //    if (Path.GetExtension(assetPath) != "")
        //    {
        //        assetPath = Path.GetDirectoryName(assetPath);
        //    }
        //}

        bool doCreate = true;
        string path = Path.Combine(assetPath, assetName + ".asset");
        FileInfo fileInfo = new FileInfo(path);
        if (fileInfo.Exists)
        {
            doCreate = EditorUtility.DisplayDialog(assetName + " already exists.",
                                                    "Do you want to overwrite the old one?",
                                                    "Yes", "No");
        }
        if (doCreate)
        {
            SkillInfo SkillInfo = Create(assetPath, assetName);
            Selection.activeObject = SkillInfo;
            EditorGUIUtility.PingObject(SkillInfo);
        }
    }

    public static SkillInfo Create(string _path, string _name)
    {
        if (!Directory.Exists(_path))
            Directory.CreateDirectory(_path);

        var p = Path.Combine(_path, _name + ".asset");

        SkillInfo info = ScriptableObject.CreateInstance<SkillInfo>();
        info = skillInfo;
        AssetDatabase.CreateAsset(info, p);
        Selection.activeObject = info;

        return info;
    }

    public static Texture2D GetTextureByType(SkillInfo.Type _type)
    {
        switch (_type)
        {
            case SkillInfo.Type.None:
                return AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/Ability/Unknown.png", typeof(Texture2D)) as Texture2D;

            case SkillInfo.Type.Attack:
                return AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/Ability/FireBall.png", typeof(Texture2D)) as Texture2D;

            case SkillInfo.Type.Defence:
                return AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/Ability/FireWall.png", typeof(Texture2D)) as Texture2D;

            case SkillInfo.Type.Poison:
                return AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/Ability/IceBall.png", typeof(Texture2D)) as Texture2D;

            case SkillInfo.Type.Boom:
                return AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/Ability/DarkForce.png", typeof(Texture2D)) as Texture2D;
        }

        return null;
    }

}
