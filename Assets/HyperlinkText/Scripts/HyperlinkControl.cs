using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HyperlinkControl : MonoBehaviour
{

    public HyperlinkText m_Hyperlink;
    private HyperlinkText Hyperlink { get { return m_Hyperlink ?? (m_Hyperlink = GameObject.FindObjectOfType<HyperlinkText>()); } }

    void Start()
    {
        Hyperlink.FillEmoji((image, emojiName) =>
        {
            Debug.LogError(emojiName + " --> EmojiFillHandler");
            //image.sprite = ResourcesTool.Instance.HyperlinkEmojiPath<Sprite>(emojiName);
        });
        Hyperlink.FillCustom((rect, prefabPath) =>
        {
            Debug.LogError(prefabPath + " --> CustomFillHandler");
            //Object prefab = ResourcesTool.Instance.HyperlinkEmojiPath<Object>(prefabPath);
            //var obj = GameObject.Instantiate(prefab) as GameObject;
            //var objRect = obj.transform as RectTransform;
            //objRect.SetParent(rect);
            //objRect.localScale = Vector3.one;
            //objRect.anchoredPosition = Vector2.zero;
        });
        Hyperlink.SetHyperlinkListener(content =>
        {
            Debug.LogError("HyperlinkText Clicked !! --> " + content);
        });
    }

}
