using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public const string Emoji = "{0}{1}{2}";
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
        ///         {0x01（默认）#HTML色值（可空，下划线颜色，默认字体颜色）#传递参数（非空，响应点击并传递参数）= 超链接显示内容（非空）}
        /// </summary>
        public const string Link = "0x01{0}{1}={2}";
        /// <summary>
        /// 自定义表情图片及是否响应点击    (目前响应点击无效，需要设置层级)
        ///     <para>
        ///         Usage:
        ///     </para>
        ///         {0x02（默认）|宽度|高度（可空，默认字体大小）#传递参数（可空，否则表情响应点击，作为超链接处理并传递参数）= 自定义加载参数（非空，路径）}
        /// </summary>
        public const string Custom = "0x02{0}{1}={2}";
        /// <summary>
        /// Prefab特效或者复杂表情    (目前响应点击无效，需要设置层级)
        ///     <para>
        ///         Usage:    
        ///     </para>
        ///         {0x03（默认）|宽度|高度（可空，默认字体大小）#传递参数（可空，否则表情响应点击，作为超链接处理并传递参数）= 自定义加载参数（非空，路径）}
        /// </summary>
        public const string Effect = "0x03{0}{1}={2}";

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

        private static string PackHyperFormat(string content)
        {
            return "{" + content + "}";
        }
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
        public static string GetTransfer(string transfer = "", string content = "")
        {
            if (string.IsNullOrEmpty(transfer) && string.IsNullOrEmpty(content))
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
            var format = string.Format(Emoji, emojiName, emojiSize, emojiTransfer);
            return PackHyperFormat(format);
        }
        public static string GetUnderLine(string content = "", string htmlColor = "")
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            var format = string.Format(UnderLine, htmlColor, content);
            return PackHyperFormat(format);
        }
        public static string GetLink(string content = "", string htmlColor = "", string transfer = "")
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            var color = GetColor(htmlColor);
            var transferLink = GetTransfer(transfer, content);
            var format = string.Format(Link, color, transferLink, content);
            return PackHyperFormat(format);
        }
        public static string GetCustom(string path, string transfer = "", float width = -1, float height = -1)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            var size = GetSize(width, height);
            var transferLink = GetTransfer(transfer);
            var format = string.Format(Custom, size, transferLink, path);
            return PackHyperFormat(format);
        }
        public static string GetEffect(string path, string transfer = "", float width = -1, float height = -1)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            var size = GetSize(width, height);
            var transferLink = GetTransfer(transfer);
            var format = string.Format(Effect, size, transferLink, path);
            return PackHyperFormat(format);
        }
    }
}