using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HyperlinkControl : MonoBehaviour
{

    [SerializeField]
    private HyperlinkText m_Hyperlink;
    private HyperlinkText Hyperlink { get { return m_Hyperlink ?? (m_Hyperlink = GameObject.FindObjectOfType<HyperlinkText>()); } }

    private void Start()
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.LogErrorFormat("Hyperlink.PreferredWidth --> {0}", LayoutUtility.GetPreferredWidth(Hyperlink.rectTransform));
            Debug.LogErrorFormat("Hyperlink.PreferredHeight --> {0}", LayoutUtility.GetPreferredHeight(Hyperlink.rectTransform));

            SetPivot(TextAnchor.UpperLeft);

            var axis = RectTransform.Axis.Vertical;
            var preferredHeight = LayoutUtility.GetPreferredSize(Hyperlink.rectTransform, (int)axis);
            Debug.LogErrorFormat("Hyperlink.SetSizeWithCurrentAnchors -> {0} -> {1}", axis, preferredHeight);
            //Hyperlink.rectTransform.SetSizeWithCurrentAnchors(axis, preferredHeight);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetPivot(TextAnchor.UpperLeft);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetPivot(TextAnchor.UpperRight);
        }
    }

    private void SetPivot(TextAnchor ancor)
    {
        var offset = new Vector2(18, 10);
        var width = Mathf.Clamp(Hyperlink.preferredWidth, 0, 400);
        var hyperSize = new Vector2(width, Hyperlink.preferredHeight);
        var parentRect = Hyperlink.rectTransform.parent as RectTransform;

        parentRect.sizeDelta = hyperSize + offset * 2;
        //Hyperlink.rectTransform.sizeDelta = hyperSize;

        if (ancor == TextAnchor.UpperLeft)
        {
            parentRect.pivot = new Vector2(0, 1);

            Hyperlink.rectTransform.pivot = new Vector2(0, 1);
            //Hyperlink.rectTransform.anchoredPosition = new Vector2(offset.x, -offset.y);
            Hyperlink.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, offset.x, hyperSize.x);
            Hyperlink.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, offset.y, hyperSize.y);
        }
        else if (ancor == TextAnchor.UpperRight)
        {
            parentRect.pivot = new Vector2(1, 1);

            Hyperlink.rectTransform.pivot = new Vector2(1, 1);
            //Hyperlink.rectTransform.anchoredPosition = new Vector2(width + offset.x, -offset.y);
            Hyperlink.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, offset.x, hyperSize.x);
            Hyperlink.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, offset.y, hyperSize.y);
        }
    }

}
