using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HyperlinkControl : MonoBehaviour
{

    public HyperlinkText m_HyperlinkText;

    void Start()
    {
        m_HyperlinkText.FillEmoji((image, emojiName) =>
        {
            Debug.LogError(emojiName + " --> EmojiFillHandler");
            //image.sprite = ResourcesTool.Instance.HyperlinkEmojiPath<Sprite>(emojiName);
        });
        m_HyperlinkText.FillCustom((rect, prefabPath) =>
        {
            Debug.LogError(prefabPath + " --> CustomFillHandler");
            //Object prefab = ResourcesTool.Instance.HyperlinkEmojiPath<Object>(prefabPath);
            //var obj = GameObject.Instantiate(prefab) as GameObject;
            //var objRect = obj.transform as RectTransform;
            //objRect.SetParent(rect);
            //objRect.localScale = Vector3.one;
            //objRect.anchoredPosition = Vector2.zero;
        });
        m_HyperlinkText.SetHyperlinkListener(content =>
        {
            Debug.LogError("HyperlinkText Clicked !! --> " + content);
        });
    }

}
