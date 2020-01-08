using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public struct HyperTextFormat
    {
        public const string Size = "|{0}";
        public const string Color = "#{0}";
        public const string Click = "#{0}";

        /// <summary>
        /// Emoji(动态)表情
        ///     <para>
        ///         Usage:
        ///     </para>
        ///         {表情名称|宽度|高度（可空，默认字体大小）#传递参数（可空，否则表情响应点击，作为超链接处理并传递参数）}
        /// </summary>
        public const string Emoji = "{{0}{1}{2}}";
        /// <summary>
        /// 下划线，不响应点击
        /// </summary>
        //  Usage:    <material=uHTML色值（可空，下划线颜色，默认字体颜色）>下划线内容</material>
        public const string UnderLine = "<material=u{0}>{1}</material>";
        /// <summary>
        /// 文字超链接
        ///     <para>
        ///         Usage:
        ///     </para>
        ///         {0x01（默认）#HTML色值（可空，下划线颜色，默认字体颜色）#传递参数（非空，响应点击并传递参数）=超链接显示内容（非空）}
        /// </summary>
        public const string Link = "{0x01{0}{1}={2}}";
        /// <summary>
        /// 自定义表情图片及是否响应点击    (目前响应点击无效，需要设置层级)
        ///     <para>
        ///         Usage:
        ///     </para>
        ///         {0x02（默认）|宽度|高度（可空，默认字体大小）#传递参数（可空，否则表情响应点击，作为超链接处理并传递参数）=自定义加载参数（非空，路径）}
        /// </summary>
        public const string Custom = "{0x02{0}{1}={2}}";
        /// <summary>
        /// Prefab特效或者复杂表情    (目前响应点击无效，需要设置层级)
        ///     <para>
        ///         Usage:    
        ///     </para>
        ///         {0x03（默认）|宽度|高度（可空，默认字体大小）#传递参数（可空，否则表情响应点击，作为超链接处理并传递参数）=自定义加载参数（非空，路径）}
        /// </summary>
        public const string Effect = "{0x03{0}{1}={2}}";

        /*
        Usage:
            测试{AA}Emoji表情 AA 
            测试{AB|36#EmojiClick}自定义大小且可点击表情 AB
            测试{a|40#EmojiClick}自定义大小且可点击动态表情
            测试<material=u#00ff00>Underline下划线</material>
            测试{0x01##ff0000#HyperLink=[HyperLink超链接]} Hyperlink
            测试{0x02|30|50##00ffff#TextureClick=icons/1}显示自定义加载表情
            测试{0x03|64=aoman}自定义加载特效
         */

        public static string GetSize(float width = -1, float height = -1)
        {
            var format = string.Empty;
            if (width > 0) format += Size;
            if (height > 0) format += Size;
            if (string.IsNullOrEmpty(format)) return string.Empty;
            return string.Format(format, width, height);
        }
        public static string GetColor(string htmlColor = "")
        {
            if (string.IsNullOrEmpty(htmlColor))
                return string.Empty;
            else
                return string.Format(Color, htmlColor);
        }
        public static string GetTransfer(string transfer = "")
        {
            if (string.IsNullOrEmpty(transfer))
                return string.Empty;
            else
                return string.Format(Click, transfer);
        }
        public static string GetEmoji(string emojiName = "", string transfer = "", float width = -1, float height = -1)
        {
            if (string.IsNullOrEmpty(emojiName))
                return string.Empty;

            var emojiSize = GetSize(width, height);
            var emojiTransfer = GetTransfer(transfer);
            return string.Format(Emoji, emojiName, emojiSize, emojiTransfer);
        }
        public static string GetUnderLine(string content = "", string htmlColor = "")
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            return string.Format(UnderLine, htmlColor, content);
        }
        public static string GetLink(string content = "", string htmlColor = "", string transfer = "")
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            var color = GetColor(htmlColor);
            return string.Format(Link, color, transfer, content);
        }
        public static string GetCustom(string path, string transfer = "", float width = -1, float height = -1)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            var size = GetSize(width, height);
            return string.Format(Custom, size, transfer, path);
        }
        public static string GetEffect(string path, string transfer = "", float width = -1, float height = -1)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            var size = GetSize(width, height);
            return string.Format(Effect, size, transfer, path);
        }
    }
    [ExecuteInEditMode]
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(UIVertexOptimize))]
    public class HyperlinkText : Text, IPointerClickHandler
    {

        #region -----------------------------------------> 内部字段 <------------------------------------------

        protected string m_OutputText = "";
        //"\\([0-9A-Za-z]+)((\\|[0-9]+){0,2})(##[0-9a-f]{6})?(#[^=\\]]+)?(=[^\\]]+)?\\]"// [\\w*/]*? --路径匹配
        protected const string m_RegexTag = "\\{([0-9A-Za-z]+)((\\|[0-9]+){0,2})(##[0-9a-f]{6})?(#[^=\\}]+)?(=[^\\}]+)?\\}";// 坐标需要 [0,0] 格式，链接格式改为 {...}
        protected const string m_RegexCustom = "<material=u(#[0-9a-f]{6})?>(((?!</material>).)*)</material>";// 下划线
        protected readonly StringBuilder m_Builder = new StringBuilder();
        protected readonly Dictionary<int, EmojiInfo> m_Emojis = new Dictionary<int, EmojiInfo>();
        protected readonly Dictionary<string, List<GameObject>> m_GameObjects = new Dictionary<string, List<GameObject>>();
        protected readonly List<RectTransform> m_Rects = new List<RectTransform>();
        protected readonly List<Image> m_Images = new List<Image>();
        protected readonly List<HrefInfo> m_Hrefs = new List<HrefInfo>();
        protected readonly List<UnderlineInfo> m_Underlines = new List<UnderlineInfo>();
        protected readonly UIVertex[] m_TempVerts = new UIVertex[4];
        protected readonly MatchResult m_MatchResult = new MatchResult();
#if UNITY_EDITOR
        protected readonly List<GameObject> m_Effects = new List<GameObject>();
#endif

        private static Dictionary<string, SpriteInfo> m_EmojiData;
        protected Dictionary<string, SpriteInfo> EmojiData
        {
            get
            {
                if (m_EmojiData == null)
                {
                    m_EmojiData = new Dictionary<string, SpriteInfo>();
                    string emojiContent = EmojiConfig.text;
                    string[] lines = emojiContent.Split('\n');
                    for (int i = 1; i < lines.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(lines[i]))
                        {
                            string[] strs = lines[i].Split('\t');
                            SpriteInfo info = new SpriteInfo();
                            info.frame = int.Parse(strs[1]);
                            info.index = int.Parse(strs[2]);
                            m_EmojiData.Add(strs[0], info);
                        }
                    }
                }
                return m_EmojiData;
            }
        }

        #endregion

        #region -----------------------------------------> 资源读取 <------------------------------------------

        private TextAsset EmojiConfig { get { return ResourcesTool.Instance.GetEmojiConfig(); } }
        private Material EmojiMat { get { return ResourcesTool.Instance.GetEmojiMaterial(); } }

#if UNITY_EDITOR
        private T LoadAssets<T>(string path) where T : Object
        {
            return Resources.Load<T>("HyperlinkText/" + path);
        }
#endif

        #endregion

        #region -----------------------------------------> 回调 <------------------------------------------

        /// <summary>0x02 Fill Image，(Image, link) </summary>
        public Action<Image, string> EmojiFillHandler;
        /// <summary>0x02 Fill Image，(Image, link) </summary>
        public void FillEmoji(Action<Image, string> callback, bool isClear = true)
        {
            if (isClear)
                EmojiFillHandler = callback;
            else
                EmojiFillHandler += callback;
        }

        /// <summary>0x03 Custom Fill (RectTransform, link)</summary>
        public Action<RectTransform, string> CustomFillHandler;
        /// <summary>0x03 Custom Fill (RectTransform, link)</summary>
        public void FillCustom(Action<RectTransform, string> callback, bool isClear = true)
        {
            if (isClear)
                CustomFillHandler = callback;
            else
                CustomFillHandler += callback;
        }

        /// <summary>Hyper Link Click Event</summary>
        public Action<string> HyperlinkClickEvent;
        /// <summary>Hyper Link Click Event</summary>
        public void SetHyperlinkListener(Action<string> callback, bool isClear = true)
        {
            if (isClear)
                HyperlinkClickEvent = callback;
            else
                HyperlinkClickEvent += callback;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Vector2 lp;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out lp);
            for (int h = 0; h < m_Hrefs.Count; h++)
            {
                var hrefInfo = m_Hrefs[h];
                var boxes = hrefInfo.boxes;
                for (var i = 0; i < boxes.Count; ++i)
                {
                    if (boxes[i].Contains(lp))
                    {
                        if (HyperlinkClickEvent == null)
                            continue;

                        HyperlinkClickEvent.Invoke(hrefInfo.url);
                        return;
                    }
                }
            }
        }

        #endregion

        #region -----------------------------------------> 重写方法 <------------------------------------------

        public override string text
        {
            get { return m_Text; }

            set
            {
                ParseText(value);
                base.text = value;
            }
        }
        public override float preferredWidth
        {
            get
            {
                var settings = GetGenerationSettings(Vector2.zero);
                return cachedTextGeneratorForLayout.GetPreferredWidth(m_OutputText, settings) / pixelsPerUnit;
            }
        }
        public override float preferredHeight
        {
            get
            {
                var settings = GetGenerationSettings(new Vector2(rectTransform.rect.size.x, 0.0f));
                return cachedTextGeneratorForLayout.GetPreferredHeight(m_OutputText, settings) / pixelsPerUnit;
            }
        }
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (font == null)
                return;

            ParseText(m_Text);

            // We don't care if we the font Texture changes while we are doing our Update.
            // The end result of cachedTextGenerator will be valid for this instance.
            // Otherwise we can get issues like Case 619238.
            m_DisableFontTextureRebuiltCallback = true;

            Vector2 extents = rectTransform.rect.size;

            var settings = GetGenerationSettings(extents);
            cachedTextGenerator.Populate(m_OutputText, settings);

            // Apply the offset to the vertices
            IList<UIVertex> verts = cachedTextGenerator.verts;
            float unitsPerPixel = 1 / pixelsPerUnit;
            //Last 4 verts are always a new line... (\n)
            int vertCount = verts.Count - 4;

            // We have no verts to process just return (case 1037923)
            if (vertCount <= 0)
            {
                toFill.Clear();
                return;
            }

            Vector3 repairVec = new Vector3(0, fontSize * 0.1f);
            Vector2 roundingOffset = new Vector2(verts[0].position.x, verts[0].position.y) * unitsPerPixel;
            roundingOffset = PixelAdjustPoint(roundingOffset) - roundingOffset;
            toFill.Clear();
            if (roundingOffset != Vector2.zero)
            {
                for (int i = 0; i < vertCount; ++i)
                {
                    int tempVertsIndex = i & 3;
                    m_TempVerts[tempVertsIndex] = verts[i];
                    m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                    m_TempVerts[tempVertsIndex].position.x += roundingOffset.x;
                    m_TempVerts[tempVertsIndex].position.y += roundingOffset.y;
                    if (tempVertsIndex == 3)
                        toFill.AddUIVertexQuad(m_TempVerts);
                }
            }
            else
            {
                Vector2 uv = Vector2.zero;
                for (int i = 0; i < vertCount; ++i)
                {
                    EmojiInfo info;
                    int index = i / 4;
                    int tempVertIndex = i & 3;

                    if (m_Emojis.TryGetValue(index, out info))
                    {
                        m_TempVerts[tempVertIndex] = verts[i];
                        m_TempVerts[tempVertIndex].position -= repairVec;
                        if (info.type == MatchType.Emoji)
                        {
                            uv.x = info.sprite.index;
                            uv.y = info.sprite.frame;
                            m_TempVerts[tempVertIndex].uv0 += uv * 10;
                        }
                        else
                        {
                            if (tempVertIndex == 3)
                                info.texture.position = m_TempVerts[tempVertIndex].position;
                            m_TempVerts[tempVertIndex].position = m_TempVerts[0].position;
                        }

                        m_TempVerts[tempVertIndex].position *= unitsPerPixel;
                        if (tempVertIndex == 3)
                            toFill.AddUIVertexQuad(m_TempVerts);
                    }
                    else
                    {
                        m_TempVerts[tempVertIndex] = verts[i];
                        m_TempVerts[tempVertIndex].position *= unitsPerPixel;
                        if (tempVertIndex == 3)
                            toFill.AddUIVertexQuad(m_TempVerts);
                    }
                }
                ComputeBoundsInfo(toFill);
                DrawUnderLine(toFill);
            }

            m_DisableFontTextureRebuiltCallback = false;

            StartCoroutine(ShowImages());
        }

        #endregion

        #region -----------------------------------------> 文本解析 <------------------------------------------

        protected void ParseText(string mText)
        {
            if (EmojiData == null)//|| !Application.isPlaying)
            {
                m_OutputText = mText;
                return;
            }

            ResetField();
            CheckMaterial();

            MatchCollection matches = Regex.Matches(mText, m_RegexTag);
            if (matches.Count > 0)
            {
                int textIndex = 0;
                int imgIdx = 0;
                int rectIdx = 0;
                for (int i = 0; i < matches.Count; i++)
                {
                    var match = matches[i];
                    m_MatchResult.Parse(match, fontSize);

                    switch (m_MatchResult.type)
                    {
                        case MatchType.Emoji:
                            {
                                SpriteInfo info;
                                if (EmojiData.TryGetValue(m_MatchResult.title, out info))
                                {
                                    m_Builder.Append(mText.Substring(textIndex, match.Index - textIndex));
                                    int temIndex = m_Builder.Length;

                                    m_Builder.Append("<quad size=");
                                    m_Builder.Append(m_MatchResult.height);
                                    m_Builder.Append(" width=");
                                    m_Builder.Append((m_MatchResult.width * 1.0f / m_MatchResult.height).ToString("f2"));
                                    m_Builder.Append(" />");

                                    m_Emojis.Add(temIndex, new EmojiInfo()
                                    {
                                        type = MatchType.Emoji,
                                        sprite = info,
                                        width = m_MatchResult.width,
                                        height = m_MatchResult.height
                                    });

                                    if (m_MatchResult.HasUrl)
                                    {
                                        var hrefInfo = new HrefInfo()
                                        {
                                            show = false,
                                            startIndex = temIndex * 4,
                                            endIndex = temIndex * 4 + 3,
                                            url = m_MatchResult.url,
                                            color = m_MatchResult.GetColor(color)
                                        };
                                        m_Hrefs.Add(hrefInfo);
                                        m_Underlines.Add(hrefInfo);
                                    }

                                    textIndex = match.Index + match.Length;
                                }
                                break;
                            }
                        case MatchType.HyperLink:
                            {
                                //NOTE: Unity 2019.2.11f1 版本的富文本处理调整了在 OnPopulateMesh 之前进行
                                //      因此需要将富文本内容的 Index 去除这样点击区域和下划线的位置才是正确的
                                //      Unity 2017.3.1f1 版本已进行对比验证此问题
                                m_Builder.Append(mText.Substring(textIndex, match.Index - textIndex));
                                int temIndex = m_Builder.Length;
                                m_Builder.Append("<color=");
                                m_Builder.Append(m_MatchResult.GetHexColor(color));
                                m_Builder.Append(">");

                                var href = new HrefInfo();
                                href.show = true;
#if UNITY_2019_2_11
                                href.startIndex = temIndex * 4;
#else
                                href.startIndex = m_Builder.Length * 4;
#endif
                                m_Builder.Append(m_MatchResult.link);
#if UNITY_2019_2_11
                                href.endIndex = (temIndex + m_MatchResult.link.Length) * 4 - 1;
#else
                                href.endIndex = m_Builder.Length * 4 - 1;
#endif
                                href.url = m_MatchResult.url;
                                href.color = m_MatchResult.GetColor(color);

                                m_Hrefs.Add(href);
                                m_Underlines.Add(href);
                                m_Builder.Append("</color>");

                                textIndex = match.Index + match.Length;
                                break;
                            }
                        case MatchType.CustomFill:
                        case MatchType.Texture:
                            {
                                m_Builder.Append(mText.Substring(textIndex, match.Index - textIndex));

                                int temIndex = m_Builder.Length;

                                m_Builder.Append("<quad size=");
                                m_Builder.Append(m_MatchResult.height);
                                m_Builder.Append(" width=");
                                m_Builder.Append((m_MatchResult.width * 1.0f / m_MatchResult.height).ToString("f2"));
                                m_Builder.Append(" />");

                                m_Emojis.Add(temIndex, new EmojiInfo()
                                {
                                    type = m_MatchResult.type,
                                    width = m_MatchResult.width,
                                    height = m_MatchResult.height,
                                    texture = new TextureInfo() { link = m_MatchResult.link, index = m_MatchResult.type == MatchType.Texture ? imgIdx++ : rectIdx++ }
                                });
                                if (m_MatchResult.HasUrl)
                                {
                                    var hrefInfo = new HrefInfo()
                                    {
                                        show = false,
                                        startIndex = temIndex * 4,
                                        endIndex = temIndex * 4 + 3,
                                        url = m_MatchResult.url,
                                        color = m_MatchResult.GetColor(color)
                                    };

                                    m_Hrefs.Add(hrefInfo);
                                    m_Underlines.Add(hrefInfo);
                                }

                                textIndex = match.Index + match.Length;
                                break;
                            }
                    }
                }
                m_Builder.Append(mText.Substring(textIndex, mText.Length - textIndex));
                m_OutputText = m_Builder.ToString();
            }
            else
            {
                m_OutputText = mText;
            }

            matches = Regex.Matches(m_OutputText, m_RegexCustom);
            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if (match.Success && match.Groups.Count == 4)
                {
                    string v1 = match.Groups[1].Value;
                    Color lineColor;
                    if (!string.IsNullOrEmpty(v1) && ColorUtility.TryParseHtmlString(v1, out lineColor)) { }
                    else lineColor = color;

                    var underline = new UnderlineInfo()
                    {
                        show = true,
                        startIndex = match.Groups[2].Index * 4,
                        endIndex = match.Groups[2].Index * 4 + match.Groups[2].Length * 4 - 1,
                        color = lineColor
                    };
                    m_Underlines.Add(underline);
                }
            }
        }
        protected void ResetField()
        {
            m_Builder.Length = 0;
            m_Emojis.Clear();
            m_Hrefs.Clear();
            m_Underlines.Clear();
            ClearImages();
        }
        protected void CheckMaterial()
        {
            if (material != null)
                return;

            material = EmojiMat;
        }
        protected void ComputeBoundsInfo(VertexHelper toFill)
        {
            UIVertex vert = new UIVertex();
            for (int u = 0; u < m_Underlines.Count; u++)
            {
                var underline = m_Underlines[u];
                underline.boxes.Clear();
                if (underline.startIndex >= toFill.currentVertCount)
                    continue;

                // Add hyper text vector index to bounds
                toFill.PopulateUIVertex(ref vert, underline.startIndex);
                var pos = vert.position;
                var bounds = new Bounds(pos, Vector3.zero);
                for (int i = underline.startIndex, m = underline.endIndex; i < m; i++)
                {
                    if (i >= toFill.currentVertCount) break;

                    toFill.PopulateUIVertex(ref vert, i);
                    pos = vert.position;
                    if (pos.x < bounds.min.x)
                    {
                        //if in different lines
                        underline.boxes.Add(new Rect(bounds.min, bounds.size));
                        bounds = new Bounds(pos, Vector3.zero);
                    }
                    else
                    {
                        bounds.Encapsulate(pos); //expand bounds
                    }

                }
                //add bound
                underline.boxes.Add(new Rect(bounds.min, bounds.size));
            }
        }
        protected void DrawUnderLine(VertexHelper toFill)
        {
            if (m_Underlines.Count <= 0)
                return;

            Vector2 extents = rectTransform.rect.size;
            var settings = GetGenerationSettings(extents);
            cachedTextGenerator.Populate("_", settings);
            IList<UIVertex> uList = cachedTextGenerator.verts;
            float h = uList[2].position.y - uList[1].position.y;
            Vector3[] temVecs = new Vector3[4];

            for (int i = 0; i < m_Underlines.Count; i++)
            {
                var info = m_Underlines[i];
                if (!info.show)
                    continue;

                for (int j = 0; j < info.boxes.Count; j++)
                {
                    if (info.boxes[j].width <= 0 || info.boxes[j].height <= 0)
                        continue;

                    temVecs[0] = info.boxes[j].min;
                    temVecs[1] = temVecs[0] + new Vector3(info.boxes[j].width, 0);
                    temVecs[2] = temVecs[0] + new Vector3(info.boxes[j].width, -h);
                    temVecs[3] = temVecs[0] + new Vector3(0, -h);

                    for (int k = 0; k < 4; k++)
                    {
                        m_TempVerts[k] = uList[k];
                        m_TempVerts[k].color = info.color;
                        m_TempVerts[k].position = temVecs[k];
                    }

                    toFill.AddUIVertexQuad(m_TempVerts);
                }
            }
        }
        protected void ClearImages()
        {
            for (int i = 0; i < m_Images.Count; i++)
                m_Images[i].rectTransform.localScale = Vector3.zero;

            for (int i = 0; i < m_Rects.Count; i++)
                m_Rects[i].localScale = Vector3.zero;
        }
        protected IEnumerator ShowImages()
        {
            yield return null;
            foreach (var emojiInfo in m_Emojis.Values)
            {
                if (emojiInfo.type == MatchType.Texture)
                {
                    emojiInfo.texture.image = GetImage(emojiInfo.texture, emojiInfo.width, emojiInfo.height);
#if UNITY_EDITOR
                    if (Application.isEditor)
                        emojiInfo.texture.image.sprite = LoadAssets<Sprite>(emojiInfo.texture.link);
#endif
                    if (EmojiFillHandler != null)
                        EmojiFillHandler(emojiInfo.texture.image, emojiInfo.texture.link);
                }
                else if (emojiInfo.type == MatchType.CustomFill)
                {
                    emojiInfo.texture.rect = GetRectTransform(emojiInfo.texture, emojiInfo.width, emojiInfo.height);
#if UNITY_EDITOR
                    if (Application.isEditor)
                    {
                        GameObject obj = null;
                        var index = emojiInfo.texture.index;
                        if (m_Effects.Count > index)
                            obj = m_Effects[index];

                        if (obj == null)
                        {
                            obj = GameObject.Instantiate(LoadAssets<GameObject>(emojiInfo.texture.link));

                            if (m_Effects.Count > index)
                                m_Effects[index] = obj;
                            else
                                m_Effects.Add(obj);
                        }
                        var objRect = obj.transform as RectTransform;
                        objRect.SetParent(emojiInfo.texture.rect);
                        objRect.localScale = Vector3.one;
                        objRect.anchoredPosition = Vector2.zero;
                    }
#endif
                    if (CustomFillHandler != null)
                        CustomFillHandler(emojiInfo.texture.rect, emojiInfo.texture.link);
                }
            }
        }
        protected Image GetImage(TextureInfo info, int width, int height)
        {
            Image img = null;
            if (m_Images.Count > info.index)
                img = m_Images[info.index];

            if (img == null)
            {
                img = GetOrAddComponent<Image>("emoji_", info.index);
                img.transform.SetParent(transform);
                img.rectTransform.pivot = Vector2.zero;
                img.raycastTarget = false;

                if (m_Images.Count > info.index)
                    m_Images[info.index] = img;
                else
                    m_Images.Add(img);
            }

            img.rectTransform.localScale = Vector3.one;
            img.rectTransform.sizeDelta = new Vector2(width, height);
            img.rectTransform.anchoredPosition = info.position;
            return img;
        }
        protected RectTransform GetRectTransform(TextureInfo info, int width, int height)
        {
            RectTransform rect = null;
            if (m_Rects.Count > info.index)
                rect = m_Rects[info.index];

            if (rect == null)
            {
                rect = GetOrAddComponent<RectTransform>("custom_", info.index);
                rect.SetParent(transform);
                rect.pivot = Vector2.zero;

                if (m_Rects.Count > info.index)
                    m_Rects[info.index] = rect;
                else
                    m_Rects.Add(rect);
            }
            rect.localScale = Vector3.one;
            rect.sizeDelta = new Vector2(width, height);
            rect.anchoredPosition = info.position;
            return rect;
        }
        protected T GetOrAddComponent<T>(string name, int index) where T : Component
        {
            var obj = GetGameObject(name, index);
            var com = obj.GetComponent<T>();
            if (com == null)
                com = obj.AddComponent<T>();

            return com;
        }
        protected GameObject GetGameObject(string name, int index)
        {
            var key = name + index;
            List<GameObject> list = new List<GameObject>();
            if (m_GameObjects.ContainsKey(key))
                list = m_GameObjects[key];
            else
                m_GameObjects.Add(key, list);

            GameObject go = null;
            if (list.Count > index)
                go = list[index];

            if (go == null)
            {
                go = new GameObject(name + index);

                if (list.Count > index)
                    list[index] = go;
                else
                    list.Add(go);
            }
            return go;
        }

        #endregion

        #region -----------------------------------------> 构造器 <------------------------------------------

        protected class SpriteInfo
        {
            public int index;
            public int frame;
        }
        protected class TextureInfo
        {
            public int index;
            public Image image;
            public RectTransform rect;
            public Vector3 position;
            public string link;
        }
        protected class EmojiInfo
        {
            public MatchType type;
            public int width;
            public int height;
            public SpriteInfo sprite;
            public TextureInfo texture;
        }
        protected enum MatchType
        {
            None,
            Emoji,
            HyperLink,
            Texture,
            CustomFill,
        }
        protected class MatchResult
        {
            public MatchType type;
            public string title;
            public string url;
            public string link;
            public int height;
            public int width;
            private string htmlColor;
            private Color color;

            public bool HasUrl { get { return !string.IsNullOrEmpty(url); } }

            void Reset()
            {
                type = MatchType.None;
                title = String.Empty;
                width = 0;
                height = 0;
                htmlColor = string.Empty;
                url = string.Empty;
                link = string.Empty;
            }

            public void Parse(Match match, int fontSize)
            {
                Reset();
                if (!match.Success || match.Groups.Count != 7)
                    return;

                title = match.Groups[1].Value;
                if (match.Groups[2].Success)
                {
                    string sizeStr = match.Groups[2].Value;
                    string[] size = sizeStr.Split('|');
                    height = size.Length > 1 ? int.Parse(size[1]) : fontSize;
                    width = size.Length == 3 ? int.Parse(size[2]) : height;
                }
                else
                {
                    height = fontSize;
                    width = fontSize;
                }
                if (match.Groups[4].Success)
                {
                    htmlColor = match.Groups[4].Value.Substring(1);
                }
                if (match.Groups[5].Success)
                {
                    url = match.Groups[5].Value.Substring(1);
                }
                if (match.Groups[6].Success)
                {
                    link = match.Groups[6].Value.Substring(1);
                }

                if (title.Equals("0x01")) //hyper link
                {
                    if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(link))
                        type = MatchType.HyperLink;
                }
                else if (title.Equals("0x02"))
                {
                    if (!string.IsNullOrEmpty(link))
                        type = MatchType.Texture;
                }
                else if (title.Equals("0x03"))
                {
                    if (!string.IsNullOrEmpty(link))
                        type = MatchType.CustomFill;
                }

                if (type == MatchType.None)
                    type = MatchType.Emoji;
            }

            public Color GetColor(Color fontColor)
            {
                if (string.IsNullOrEmpty(htmlColor))
                    return fontColor;

                ColorUtility.TryParseHtmlString(htmlColor, out color);
                return color;
            }

            public string GetHexColor(Color fontColor)
            {
                if (!string.IsNullOrEmpty(htmlColor))
                    return htmlColor;

                return ColorUtility.ToHtmlStringRGBA(fontColor);
            }
        }
        protected class UnderlineInfo
        {
            public bool show;
            public int startIndex;
            public int endIndex;
            public Color color;
            public readonly List<Rect> boxes = new List<Rect>();
        }
        protected class HrefInfo : UnderlineInfo
        {
            public string url;
        }

        #endregion

    }
}
