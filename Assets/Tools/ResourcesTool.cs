using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResourcesTool
{
    private ResourcesTool() { }
    private static ResourcesTool m_Instance;
    public static ResourcesTool Instance { get { return m_Instance ?? (m_Instance = new ResourcesTool()); } }

    private const string HyperlinkPath = "HyperlinkText/";

    public TextAsset GetEmojiConfig()
    {
        // 根据用户选择的 表情进行加载，加载方式也要改
        return Resources.Load<TextAsset>(HyperlinkPath + "emoji");
    }

    public Material GetEmojiMaterial()
    {
        // 根据用户选择的 表情进行加载，加载方式也要改
        return Resources.Load<Material>(HyperlinkPath + "emoji");
    }


    public T HyperlinkEmojiPath<T>(string emoji) where T : Object
    {
        return Resources.Load<T>(HyperlinkPath + emoji);
    }

}
