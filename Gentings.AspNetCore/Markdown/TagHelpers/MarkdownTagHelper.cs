using Gentings.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.Markdown.TagHelpers
{
    /// <summary>
    /// Markdown语法标签实例。
    /// </summary>
    [HtmlTargetElement("markdown")]
    public class MarkdownTagHelper : TagHelperBase
    {
        /// <summary>
        /// 扩展标签。
        /// </summary>
        public MarkdownExtension Extensions { get; set; } = MarkdownExtension.Common;

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            var content = await output.GetChildContentAsync();
            if (content.IsEmptyOrWhiteSpace) return;
            var source = content.GetContent().Trim();
            source = MarkdownConvert.ToHtml(source, Extensions);
            output.AppendHtml(source);
        }
    }
}
