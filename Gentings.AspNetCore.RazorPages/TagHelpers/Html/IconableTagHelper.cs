using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.RazorPages.TagHelpers.Html
{
    /// <summary>
    /// 附加按钮的控件。
    /// </summary>
    [HtmlTargetElement("*", Attributes = ".icon")]
    public class IconableTagHelper : TagHelperBase
    {
        /// <summary>
        /// 位置。
        /// </summary>
        public enum Position
        {
            /// <summary>
            /// 前置。
            /// </summary>
            Prepend,
            /// <summary>
            /// 附加。
            /// </summary>
            Append,
        }

        /// <summary>
        /// 图标位置。
        /// </summary>
        [HtmlAttributeName(".icon-position")]
        public Position ThePosition { get; set; } = Position.Prepend;

        /// <summary>
        /// 图标名称。
        /// </summary>
        [HtmlAttributeName(".icon")]
        public string IconName { get; set; }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(IconName))
                return;
            var icon = new TagBuilder("span");
            icon.MergeAttribute("data-feather", IconName);
            if (ThePosition == Position.Prepend)
                output.Content.AppendHtml(icon);
            output.Content.AppendHtml(await output.GetChildContentAsync());
            if (ThePosition == Position.Append)
                output.Content.AppendHtml(icon);
        }
    }
}