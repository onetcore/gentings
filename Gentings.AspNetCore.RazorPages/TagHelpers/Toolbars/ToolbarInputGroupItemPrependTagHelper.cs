using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.RazorPages.TagHelpers.Toolbars
{
    /// <summary>
    /// Toolbar前置按钮。
    /// </summary>
    [HtmlTargetElement("gt:prepend", ParentTag = "gt:toolbar-input-group")]
    public class ToolbarInputGroupItemPrependTagHelper : TagHelperBase
    {
        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddCssClass("input-group-prepend");
            output.Content.AppendHtml(await output.GetChildContentAsync());
        }
    }
}