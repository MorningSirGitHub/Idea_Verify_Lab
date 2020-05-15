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
    [ExecuteInEditMode]
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(UIVertexOptimize))]
    public class HyperlinkText : Text, IPointerClickHandler
    {

        #region -----------------------------------------> 内部字段 <------------------------------------------

        protected string m_HyperOutputText;
        //"\\([0-9A-Za-z]+)((\\|[0-9]+){0,2})(##[0-9a-f]{6})?(#[^=\\]]+)?(=[^\\]]+)?\\]"// [\\w*/]*? --路径匹配
        protected const string m_RegexTag = "\\{([0-9A-Za-z]+)((\\|[0-9]+){0,2})(##[0-9a-fA-F]{6})?(#[^=\\}]*)?(=[^\\}]*)?\\}";// 坐标需要 [0,0] 格式，链接格式改为 {...}
        protected const string m_RegexCustom = "<material=u(#[0-9a-f]{6})?>(((?!</material>).)*)</material>";// 下划线
        protected const string m_RegexRichFormat = "<.*?>";// 富文本
        protected const string m_RegexNumber = "([0-9]+)";// 字体
        protected readonly StringBuilder m_Builder = new StringBuilder();
        protected readonly Dictionary<int, EmojiInfo> m_Emojis = new Dictionary<int, EmojiInfo>();
        protected readonly Dictionary<int, EmojiInfo> m_EmojisReallyIndex = new Dictionary<int, EmojiInfo>();
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

        public bool IsDrawUnderLine = true;
        public bool IsMultiLine { get { return horizontalOverflow == HorizontalWrapMode.Wrap && cachedTextGenerator.lineCount > 1; } }
        public bool IsLimitMultiLine { get { return preferredHeight > cachedTextGenerator.rectExtents.height; } }
        protected bool m_IsMultiLine { get { return IsMultiLine || IsLimitMultiLine; } }
        protected Dictionary<int, EmojiInfo> m_EmojisDic { get { return m_IsMultiLine ? m_Emojis : m_EmojisReallyIndex; } }

        #endregion

        #region -----------------------------------------> 资源读取 <------------------------------------------

        protected static Dictionary<string, Dictionary<string, SpriteInfo>> m_EmojiData = new Dictionary<string, Dictionary<string, SpriteInfo>>();
        protected Dictionary<string, SpriteInfo> EmojiData
        {
            get
            {
                Dictionary<string, SpriteInfo> selectEmoji = null;
                if (m_EmojiData.TryGetValue(EmojiType, out selectEmoji))
                    return selectEmoji;

                ReadEmojiConfig(ref selectEmoji);
                return selectEmoji;
            }
        }

        protected const string EmojiTest = "Test";
        protected const string EmojiName = "emoji";
        protected const string EmojiPath = "Emoji/";
        public string EmojiType = EmojiTest;//获取用户选择的Emoji名字（或者以后改为其他方式）
        protected void ReadEmojiConfig(ref Dictionary<string, SpriteInfo> selectEmoji)
        {
            var config = EmojiConfig();
            if (config == null)
                return;

            if (selectEmoji == null)
                selectEmoji = new Dictionary<string, SpriteInfo>();

            string emojiContent = config.text;
            string[] lines = emojiContent.Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                if (!string.IsNullOrEmpty(lines[i]))
                {
                    string[] strs = lines[i].Split('\t');
                    SpriteInfo info = new SpriteInfo();
                    info.frame = int.Parse(strs[1]);
                    info.index = int.Parse(strs[2]);
                    selectEmoji.Add(strs[0], info);
                }
            }
            m_EmojiData.Add(EmojiType, selectEmoji);
        }
        protected TextAsset EmojiConfig()
        {
            return LoadAssets<TextAsset>(EmojiName + "_config");
        }
        protected Material EmojiMat()
        {
            return LoadAssets<Material>(EmojiName + "_mat");
        }
        protected T LoadAssets<T>(string path) where T : Object
        {
            // 根据用户选择的 表情进行加载
            //if (EmojiType == EmojiTest)
            //    Debug.Log("Before use this component, must be set EmojiType first !! \n" +
            //                   "This Error is only for Test and must be have \"Emoji/Test\" path !!");

            var type = string.IsNullOrEmpty(EmojiType) ? string.Empty : "/";
            var loadPath = EmojiPath + EmojiType + type + path;
#if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                var extension = string.Empty;
                if (typeof(T).IsAssignableFrom(typeof(Material)))
                    extension = ".mat";
                else if (typeof(T).IsAssignableFrom(typeof(TextAsset)))
                    extension = ".txt";
                else if (typeof(T).IsAssignableFrom(typeof(Sprite)))
                    extension = ".png";

                loadPath = "Assets/HyperlinkText/" + loadPath + extension;
                return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(loadPath);
            }
#endif
            return Resources.Load<T>("HyperlinkText/" + loadPath);
        }

        #endregion

        #region -----------------------------------------> 外部回调 <------------------------------------------

        protected Action<Image, string> m_EmojiFillHandler;
        /// <summary>0x02 Fill Image，(Image, link) </summary>
        public void FillEmoji(Action<Image, string> callback, bool isClear = true)
        {
            if (isClear)
                m_EmojiFillHandler = callback;
            else
                m_EmojiFillHandler += callback;
        }
        protected Action<RectTransform, string> m_CustomFillHandler;
        /// <summary>0x03 Custom Fill (RectTransform, link)</summary>
        public void FillCustom(Action<RectTransform, string> callback, bool isClear = true)
        {
            if (isClear)
                m_CustomFillHandler = callback;
            else
                m_CustomFillHandler += callback;
        }
        protected Action<int, string> m_HyperlinkClickEvent;
        /// <summary>0x01 Hyper Link Click Event (boxIndex, url)</summary>
        public void SetHyperlinkListener(Action<int, string> callback, bool isClear = true)
        {
            if (isClear)
                m_HyperlinkClickEvent = callback;
            else
                m_HyperlinkClickEvent += callback;
        }
        protected Action m_OnPopulateMeshOverEvent;
        /// <summary>OnPopulateMesh Event</summary>
        public void SetOnPopulateMeshOverListener(Action callback, bool isClear = true)
        {
            if (isClear)
                m_OnPopulateMeshOverEvent = callback;
            else
                m_OnPopulateMeshOverEvent += callback;
        }
        protected Action<Vector2> m_PointerClickEvent;
        /// <summary>PointerClick Event (localPoint)</summary>
        public void SetPointerClickListener(Action<Vector2> callback, bool isClear = true)
        {
            if (isClear)
                m_PointerClickEvent = callback;
            else
                m_PointerClickEvent += callback;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
            if (m_PointerClickEvent != null)
                m_PointerClickEvent(localPoint);

            for (int h = 0; h < m_Hrefs.Count; h++)
            {
                var hrefInfo = m_Hrefs[h];
                var boxes = hrefInfo.boxes;
                for (var i = 0; i < boxes.Count; ++i)
                {
                    if (boxes[i].Contains(localPoint))
                    {
                        if (m_HyperlinkClickEvent == null)
                            continue;

                        m_HyperlinkClickEvent.Invoke(h, hrefInfo.url);
                        return;
                    }
                }
            }
        }

        #region 为了不能包更而做的妥协，暂存

        //----------------------------------------------- 功能回调函数 ------------------------------------------------------

        //private Action<Vector2> m_PointerClick;
        //public void SetPointerClickListener(Action<Vector2> callback)
        //{
        //    m_PointerClick = callback;
        //}
        //private Func<string> m_GetHyperText;
        ///// <summary> 获取 m_HyperOutputText 的回调</summary>
        //public void SetHyperTextListener(Func<string> callback)
        //{
        //    m_GetHyperText = callback;
        //}
        //private Action<string> m_SetHyperText;
        ///// <summary> 设置 m_HyperOutputText 的回调</summary>
        //public void SetHyperTextListener(Action<string> callback)
        //{
        //    m_SetHyperText = callback;
        //}
        //private Action<string, Material, int, Color> m_ParseText;
        ///// <summary> 解析原字符串 的回调</summary>
        //public void SetParseTextListener(Action<string, Material, int, Color> callback)
        //{
        //    m_ParseText = callback;
        //}
        //private Func<UIVertex, int, UIVertex> m_DealWithEmojiData;
        ///// <summary> 填充emoji表情信息 的回调</summary>
        //public void SetDealWithEmojiDataListener(Func<UIVertex, int, UIVertex> callback)
        //{
        //    m_DealWithEmojiData = callback;
        //}
        //private Action<VertexHelper> m_ComputeBounds;
        ///// <summary> 计算边界 的回调</summary>
        //public void SetComputeBoundsListener(Action<VertexHelper> callback)
        //{
        //    m_ComputeBounds = callback;
        //}
        //private Func<bool> m_IsDraw;
        ///// <summary> 是否有必要绘制 的回调</summary>
        //public void SetIsDrawUnderLineListener(Func<bool> callback)
        //{
        //    m_IsDraw = callback;
        //}
        //private Action<VertexHelper, UIVertex[], IList<UIVertex>> m_DrawUnderLine;
        ///// <summary> 绘制下划线 的回调</summary>
        //public void SetDrawUnderLineListener(Action<VertexHelper, UIVertex[], IList<UIVertex>> callback)
        //{
        //    m_DrawUnderLine = callback;
        //}
        //private Action m_ShowImages;
        ///// <summary> 解析原字符串 的回调</summary>
        //public void SetShowImagesListener(Action callback)
        //{
        //    m_ShowImages = callback;
        //}

        #endregion

        #endregion

        #region -----------------------------------------> 重写方法 <------------------------------------------

        public override string text
        {
            get { return m_Text; }
            set
            {
                //NOTE:在文本长或高为0时，获取不到正确的解析
                ParseText(value);
                base.text = value;
            }
        }
        public override float preferredWidth
        {
            get
            {
                var settings = GetGenerationSettings(Vector2.zero);
                return cachedTextGeneratorForLayout.GetPreferredWidth(m_HyperOutputText, settings) / pixelsPerUnit;
            }
        }
        public override float preferredHeight
        {
            get
            {
                var settings = GetGenerationSettings(new Vector2(rectTransform.rect.size.x, 0.0f));
                return cachedTextGeneratorForLayout.GetPreferredHeight(m_HyperOutputText, settings) / pixelsPerUnit;
            }
        }
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (font == null)
                return;

            // NOTE: 如果GameObject是关闭状态可以不更新
            if (!gameObject.activeSelf || string.IsNullOrEmpty(m_Text) || base.preferredHeight <= 0 || base.preferredWidth <= 0)
            {
                ResetField();
                toFill.Clear();
                return;
            }

            //Debug.LogError("OnPopulateMesh !!\n");
            ParseText(m_Text);

            // We don't care if we the font Texture changes while we are doing our Update.
            // The end result of cachedTextGenerator will be valid for this instance.
            // Otherwise we can get issues like Case 619238.
            m_DisableFontTextureRebuiltCallback = true;

            var extents = rectTransform.rect.size;
            var settings = GetGenerationSettings(extents);
            cachedTextGenerator.PopulateWithErrors(m_HyperOutputText, settings, gameObject);

            // Apply the offset to the vertices
            IList<UIVertex> verts = cachedTextGenerator.verts;//如果使用富文本，这里的verts数量是除去富文本格式之后的数量
            float unitsPerPixel = 1 / pixelsPerUnit;
            // Last 4 verts are always a new line... (\n) 不做处理，可能会少字符
            // NOTE: 2019.1.5f1及更高的版本中cachedTextGenerator的行数不同会造成富文本的verts数量不同，所以这个需要区别
            int vertCount = verts.Count;// - 4;

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
                EmojiInfo info = null;
                Vector2 uv = Vector2.zero;
                for (int i = 0; i < vertCount; ++i)
                {
                    int tempVertIndex = i & 3;
                    m_TempVerts[tempVertIndex] = verts[i];
                    m_TempVerts[tempVertIndex].position *= unitsPerPixel;
                    // 处理emoji的情况
                    if (m_EmojisDic.TryGetValue(i / 4, out info))
                    {
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
                    }
                    if (tempVertIndex == 3)
                        toFill.AddUIVertexQuad(m_TempVerts);
                }
                ComputeBoundsInfo(toFill);
                DrawUnderLine(toFill);
            }

            m_DisableFontTextureRebuiltCallback = false;

            if (gameObject.activeSelf && gameObject.activeInHierarchy)
                StartCoroutine(ShowImages());

            if (m_OnPopulateMeshOverEvent != null)
                m_OnPopulateMeshOverEvent();
        }
        public override void Rebuild(CanvasUpdate update)
        {
            StopCoroutine(ShowImages());
            base.Rebuild(update);
        }

        #endregion

        #region -----------------------------------------> 文本解析 <------------------------------------------

        protected void ParseText(string mText)
        {
            if (EmojiData == null)//|| !Application.isPlaying)
            {
                m_HyperOutputText = mText;
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
                        //NOTE: 2019.1.5f1 及以后版本 对富文本的空占位符进行了优化，单行状态下verts数量是实际内容的数量
                        case MatchType.Emoji:
                            {
                                SpriteInfo info = null;
                                if (EmojiData.TryGetValue(m_MatchResult.title, out info))
                                {
                                    m_Builder.Append(mText.Substring(textIndex, match.Index - textIndex));
                                    var temIndex = m_Builder.Length;
                                    var temReallyIndex = GetReallyIndex(m_Builder.ToString());
                                    m_Builder.Append("<quad size=");
                                    m_Builder.Append(m_MatchResult.height);
                                    m_Builder.Append(" width=");
                                    m_Builder.Append((m_MatchResult.width * 1.0f / m_MatchResult.height).ToString("f2"));
                                    m_Builder.Append(" />");

                                    var emojiInfo = new EmojiInfo()
                                    {
                                        type = MatchType.Emoji,
                                        sprite = info,
                                        width = m_MatchResult.width,
                                        height = m_MatchResult.height
                                    };
                                    m_Emojis.Add(temIndex, emojiInfo);
                                    m_EmojisReallyIndex.Add(temReallyIndex, emojiInfo);

                                    if (m_MatchResult.HasUrl)
                                    {
                                        var hrefInfo = new HrefInfo()
                                        {
                                            show = false,
                                            startIndex = temIndex * 4,
                                            reallyStartIndex = temReallyIndex * 4,
                                            endIndex = temIndex * 4 + 3,
                                            reallyEndIndex = temReallyIndex * 4 + 3,
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
                                //      备注：在不同行数的时候引擎解析的长度会不同，所以Index是变化的，后面增加了Index的修正长度还是使用原长度
                                m_Builder.Append(mText.Substring(textIndex, match.Index - textIndex));
                                m_Builder.Append("<color=");
                                m_Builder.Append(m_MatchResult.GetHexColor(color));
                                m_Builder.Append(">");
                                var temIndex = m_Builder.Length;
                                var temReallyIndex = GetReallyIndex(m_Builder.ToString());
                                m_Builder.Append(m_MatchResult.link);
                                m_Builder.Append("</color>");

                                var href = new HrefInfo()
                                {
                                    show = true,
                                    //#if UNITY_2019_2_OR_NEWER
                                    startIndex = temIndex * 4,
                                    reallyStartIndex = temReallyIndex * 4,
                                    endIndex = (temIndex + m_MatchResult.link.Length) * 4 - 1,
                                    reallyEndIndex = (temReallyIndex + m_MatchResult.link.Length) * 4 - 1,
                                    url = m_MatchResult.url,
                                    color = m_MatchResult.GetColor(color),
                                };
                                m_Hrefs.Add(href);
                                m_Underlines.Add(href);

                                textIndex = match.Index + match.Length;
                                break;
                            }
                        case MatchType.CustomFill:
                        case MatchType.Texture:
                            {
                                m_Builder.Append(mText.Substring(textIndex, match.Index - textIndex));
                                var temIndex = m_Builder.Length;
                                var temReallyIndex = GetReallyIndex(m_Builder.ToString());
                                m_Builder.Append("<quad size=");
                                m_Builder.Append(m_MatchResult.height);
                                m_Builder.Append(" width=");
                                m_Builder.Append((m_MatchResult.width * 1.0f / m_MatchResult.height).ToString("f2"));
                                m_Builder.Append(" />");

                                var emojiInfo = new EmojiInfo()
                                {
                                    type = m_MatchResult.type,
                                    width = m_MatchResult.width,
                                    height = m_MatchResult.height,
                                    texture = new TextureInfo() { link = m_MatchResult.link, index = m_MatchResult.type == MatchType.Texture ? imgIdx++ : rectIdx++ }
                                };
                                m_Emojis.Add(temIndex, emojiInfo);
                                m_EmojisReallyIndex.Add(temReallyIndex, emojiInfo);

                                if (m_MatchResult.HasUrl)
                                {
                                    var hrefInfo = new HrefInfo()
                                    {
                                        show = false,
                                        startIndex = temIndex * 4,
                                        reallyStartIndex = temReallyIndex * 4,
                                        endIndex = temIndex * 4 + 3,
                                        reallyEndIndex = temReallyIndex * 4 + 3,
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
                m_HyperOutputText = m_Builder.ToString();
            }
            else
            {
                m_HyperOutputText = mText;
            }

            matches = Regex.Matches(m_HyperOutputText, m_RegexCustom);
            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if (match.Success && match.Groups.Count == 4)
                {
                    Color lineColor;
                    if (!ColorUtility.TryParseHtmlString(match.Groups[1].Value, out lineColor))
                        lineColor = color;

                    var subMatch = match.Groups[2];
                    var temIndex = GetReallyIndex(subMatch.Value);
                    var underline = new UnderlineInfo()
                    {
                        show = true,
                        startIndex = subMatch.Index * 4,
                        reallyStartIndex = temIndex * 4,
                        endIndex = (subMatch.Index + subMatch.Length) * 4 - 1,
                        reallyEndIndex = (temIndex + subMatch.Length) * 4 - 1,
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
            m_EmojisReallyIndex.Clear();
            m_Hrefs.Clear();
            m_Underlines.Clear();
            ClearImages();
        }
        protected void CheckMaterial()
        {
            if (material != null)
                return;

            // 这里判断可根据项目需求进行拓展
            material = EmojiMat();
        }
        protected int GetReallyIndex(string currentMatch)
        {
            var texture = 0;
            var total = currentMatch;
            total = total.Replace(" ", string.Empty);
            var matchList = Regex.Matches(total, m_RegexRichFormat);
            for (int i = 0; i < matchList.Count; i++)
            {
                var match = matchList[i];
                if (!match.Success)
                    continue;

                //TODO: 多个图片时的宽度要加入到实际长度中
                //if (match.Value.Contains("quad size="))
                //{
                //    Debug.LogError(match.Value);
                //    var sizeMatch = Regex.Match(match.Value, m_RegexNumber, RegexOptions.RightToLeft);
                //    if (!sizeMatch.Success)
                //        continue;

                //    Debug.LogError(sizeMatch.Value);
                //    texture += int.Parse(sizeMatch.Value);
                //    continue;
                //}

                total = total.Replace(match.Value, string.Empty);
            }
            return total.Length + texture;
        }
        protected void ComputeBoundsInfo(VertexHelper toFill)
        {
            //CharVertsInfo charInfo = new CharVertsInfo();
            UIVertex vert = new UIVertex();
            for (int u = 0; u < m_Underlines.Count; u++)
            {
                var underline = m_Underlines[u];
                underline.boxes.Clear();
                //underline.charswidth.Clear();
#if UNITY_2019_1_OR_NEWER
                underline.CorrectionIndex(m_IsMultiLine);
#endif
                if (underline.startIndex >= toFill.currentVertCount)
                    continue;

                // Add hyper text vector index to bounds
                toFill.PopulateUIVertex(ref vert, underline.startIndex);
                var pos = vert.position;
                var bounds = new Bounds(pos, Vector3.zero);
                for (int i = underline.startIndex, m = underline.endIndex; i < m; i++)
                {
                    if (i >= toFill.currentVertCount)
                        break;

                    toFill.PopulateUIVertex(ref vert, i);
                    pos = vert.position;
                    if (pos.x < bounds.min.x)
                    {
                        //if in different lines
                        underline.boxes.Add(new Rect(bounds.min, bounds.size));
                        bounds = new Bounds(pos, Vector3.zero);
                        //underline.charswidth.Add(charInfo);
                        //charInfo = new CharVertsInfo();
                    }
                    else
                    {
                        bounds.Encapsulate(pos); //expand bounds
                        //if (i % 4 == 2)
                        //{
                        //    toFill.PopulateUIVertex(ref vert, i + 1);
                        //    charInfo.charswidth.Add(vert.position.x - pos.x);
                        //    charInfo.minPoslist.Add(new Vector3(pos.x, bounds.min.y));
                        //}
                    }
                }
                //add bound
                underline.boxes.Add(new Rect(bounds.min, bounds.size));
                //underline.charswidth.Add(charInfo);
            }
        }
        protected void DrawUnderLine(VertexHelper toFill)
        {
            if (!IsDrawUnderLine)
                return;

            if (m_Underlines.Count <= 0)
                return;

            var extents = rectTransform.rect.size;
            var settings = GetGenerationSettings(extents);
            cachedTextGenerator.Populate("_", settings);

            IList<UIVertex> uList = cachedTextGenerator.verts;
            if (uList.Count < 4)
                return;

            float w_offset = 0.2f;
            float h_offset = 1.5f;
            float h = uList[2].position.y - uList[1].position.y;
            Vector3[] temVecs = new Vector3[4];

            for (int i = 0; i < m_Underlines.Count; i++)
            {
                var info = m_Underlines[i];
                if (!info.show)
                    continue;

                for (int j = 0; j < info.boxes.Count; j++)
                {
                    var box = info.boxes[j];
                    if (box.width <= 0 || box.height <= 0)
                        continue;

                    // NOTE: 如果使用一个下划线，文本太长时两端会被拉伸到透明

                    // 每个字单独计算下划线
                    //var widthinfo = info.charswidth[j];
                    //for (int o = 0; o < widthinfo.charswidth.Count; o++)
                    //{
                    //    var width = widthinfo.charswidth[o];
                    //    var minPos = widthinfo.minPoslist[o];
                    //    temVecs[0] = minPos + new Vector3(width * (0 - w_offset), h_offset + 0);
                    //    temVecs[1] = minPos + new Vector3(width * (1 + w_offset), h_offset + 0);
                    //    temVecs[2] = minPos + new Vector3(width * (1 + w_offset), h_offset + h);
                    //    temVecs[3] = minPos + new Vector3(width * (0 - w_offset), h_offset + h);

                    //    for (int k = 0; k < 4; k++)
                    //    {
                    //        m_TempVerts[k] = uList[k];
                    //        m_TempVerts[k].color = info.color;
                    //        m_TempVerts[k].position = temVecs[k];
                    //    }

                    //    toFill.AddUIVertexQuad(m_TempVerts);
                    //}

                    // 平均计算的下划线
                    var basePos = box.min;
                    var popWidth = (info.endIndex - info.startIndex) / 4f;
                    var w = box.width / popWidth;
                    for (int p = 0; p < popWidth; p++)
                    {
                        temVecs[0] = basePos + new Vector2(w * (0 - w_offset), h_offset + 0);
                        temVecs[1] = basePos + new Vector2(w * (1 + w_offset), h_offset + 0);
                        temVecs[2] = basePos + new Vector2(w * (1 + w_offset), h_offset + h);
                        temVecs[3] = basePos + new Vector2(w * (0 - w_offset), h_offset + h);

                        for (int k = 0; k < 4; k++)
                        {
                            m_TempVerts[k] = uList[k];
                            m_TempVerts[k].color = info.color;
                            m_TempVerts[k].position = temVecs[k];
                        }

                        toFill.AddUIVertexQuad(m_TempVerts);
                        basePos += new Vector2(w, 0);
                    }
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
            foreach (var emojiInfo in m_EmojisDic.Values)
            {
                if (emojiInfo.type == MatchType.Texture)
                {
                    emojiInfo.texture.image = GetImage(emojiInfo.texture, emojiInfo.width, emojiInfo.height);
#if UNITY_EDITOR
                    if (Application.isEditor && !Application.isPlaying)
                    {
                        emojiInfo.texture.image.sprite = LoadAssets<Sprite>(emojiInfo.texture.link);
                        continue;
                    }
#endif
                    if (m_EmojiFillHandler != null)
                        m_EmojiFillHandler(emojiInfo.texture.image, emojiInfo.texture.link);
                }
                else if (emojiInfo.type == MatchType.CustomFill)
                {
                    emojiInfo.texture.rect = GetRectTransform(emojiInfo.texture, emojiInfo.width, emojiInfo.height);
#if UNITY_EDITOR
                    if (Application.isEditor && !Application.isPlaying)
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
                        continue;
                    }
#endif
                    if (m_CustomFillHandler != null)
                        m_CustomFillHandler(emojiInfo.texture.rect, emojiInfo.texture.link);
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
                img.rectTransform.anchorMax = rectTransform.anchorMax;
                img.rectTransform.anchorMin = rectTransform.anchorMin;
                img.rectTransform.pivot = Vector2.zero;
                img.raycastTarget = false;

                if (m_Images.Count > info.index)
                    m_Images[info.index] = img;
                else
                    m_Images.Add(img);
            }

            img.rectTransform.localScale = Vector3.one;
            img.rectTransform.sizeDelta = new Vector2(width, height);
            img.rectTransform.anchoredPosition3D = info.position;
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
                rect.anchorMax = rectTransform.anchorMax;
                rect.anchorMin = rectTransform.anchorMin;
                rect.pivot = Vector2.zero;

                if (m_Rects.Count > info.index)
                    m_Rects[info.index] = rect;
                else
                    m_Rects.Add(rect);
            }
            rect.localScale = Vector3.one;
            rect.sizeDelta = new Vector2(width, height);
            rect.anchoredPosition3D = info.position;
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
            var list = new List<GameObject>();
            if (m_GameObjects.ContainsKey(key))
                list = m_GameObjects[key];
            else
                m_GameObjects.Add(key, list);

            GameObject go = null;
            if (list.Count > index)
                go = list[index];

            if (go == null)
            {
                var child = transform.Find(key);
                if (child == null)
                    go = new GameObject(key);
                else
                    go = child.gameObject;

                go.layer = gameObject.layer;
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

                if (title.Equals("0x01"))
                {
                    // url 是否为空 不影响超链接的判定
                    if (!string.IsNullOrEmpty(link))
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
            public int startIndex;//包含富文本的index
            public int reallyStartIndex;//祛除富文本的index
            public int endIndex;//包含富文本的index
            public int reallyEndIndex;//祛除富文本的index
            public Color color;
            public readonly List<Rect> boxes = new List<Rect>();
            public readonly List<CharVertsInfo> charswidth = new List<CharVertsInfo>();

            public void CorrectionIndex(bool isMultiLine)
            {
                if (isMultiLine)
                {

                }
                else
                {
                    startIndex = reallyStartIndex;
                    endIndex = reallyEndIndex;
                }
            }
        }
        protected class HrefInfo : UnderlineInfo
        {
            public string url;
        }
        protected class CharVertsInfo
        {
            public List<float> charswidth = new List<float>();
            public List<Vector3> minPoslist = new List<Vector3>();
        }

        #endregion

    }
}
