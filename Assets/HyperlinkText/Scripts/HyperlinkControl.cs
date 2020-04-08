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
        CreateBroadcast();
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



    #region 创建及缓存

    private static GameObject m_BroadcastRoot;
    private Image m_BroadcastBG;
    private HyperlinkText m_BroadcastContent;

    protected void CreateBroadcast()
    {
        if (m_BroadcastRoot == null)
        {
            m_BroadcastRoot = CreateUIRootGameObject("BroadcastRoot");
            m_BroadcastRoot.layer = LayerMask.NameToLayer("UI");
            m_BroadcastRoot.transform.SetAsLastSibling();
        }
        if (m_BroadcastBG == null)
        {
            var BGObj = CreateUIGameObject("System", m_BroadcastRoot.transform);
            BGObj.layer = LayerMask.NameToLayer("UI");
            BGObj.transform.SetAsFirstSibling();
            GetOrAddComponent<RectMask2D>(BGObj);
            m_BroadcastBG = GetOrAddComponent<Image>(BGObj);
            m_BroadcastBG.rectTransform.anchoredPosition = new Vector2(0, Screen.height / 2f - 200);
            m_BroadcastBG.rectTransform.sizeDelta = new Vector2(300, 30);
            m_BroadcastBG.raycastTarget = true;
            m_BroadcastBG.color = Color.clear;
        }
        else
        {
            m_BroadcastBG.gameObject.SetActive(true);
        }
        if (m_BroadcastContent == null)
        {
            var contentObj = CreateUIGameObject("Content", m_BroadcastBG.transform);
            contentObj.layer = LayerMask.NameToLayer("UI");
            contentObj.transform.SetAsLastSibling();
            m_BroadcastContent = GetOrAddComponent<HyperlinkText>(contentObj);
            m_BroadcastContent.rectTransform.anchoredPosition = Vector2.zero;
            m_BroadcastContent.rectTransform.sizeDelta = new Vector2(300, 30);
            m_BroadcastContent.font = Font.CreateDynamicFontFromOSFont("Arial", 24);
            m_BroadcastContent.fontSize = 24;
            m_BroadcastContent.raycastTarget = false;
            m_BroadcastContent.supportRichText = true;
            m_BroadcastContent.alignment = TextAnchor.MiddleCenter;
            m_BroadcastContent.horizontalOverflow = HorizontalWrapMode.Overflow;
            m_BroadcastContent.text = "<size=24><color=#ffffff>{0x02#a=Icons/AC}？？？</color></size>";
        }
    }
    protected GameObject CreateUIRootGameObject(string name, int layer = int.MaxValue, Transform parent = null)
    {
        var obj = CreateUIGameObject(name, parent);

        //var camera = GetOrAddComponent<Camera>(obj);
        //camera.clearFlags = CameraClearFlags.Depth;
        //camera.cullingMask = LayerMask.NameToLayer("UI");
        //camera.orthographic = true;
        //camera.depth = 999999;

        var canvas = GetOrAddComponent<Canvas>(obj);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = layer;

        var graphicRaycaster = GetOrAddComponent<GraphicRaycaster>(obj);

        GetOrAddComponent<CanvasScaler>(obj);

        return obj;
    }
    protected GameObject CreateUIGameObject(string name, Transform parent = null)
    {
        var obj = new GameObject(name);

        var rect = GetOrAddComponent<RectTransform>(obj);
        //rect.anchorMin = Vector3.zero;
        //rect.anchorMax = Vector3.one;
        //rect.sizeDelta = Vector2.zero;

        rect.SetParent(parent);
        rect.localEulerAngles = Vector3.zero;
        rect.localPosition = Vector3.zero;
        rect.localScale = Vector3.one;

        return obj;
    }
    protected T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        var component = obj.GetComponent<T>();
        if (component != null)
            return component;

        return obj.AddComponent<T>();
    }

    #endregion

}
