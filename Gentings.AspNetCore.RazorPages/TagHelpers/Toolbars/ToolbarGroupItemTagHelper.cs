using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.RazorPages.TagHelpers.Toolbars
{
    /// <summary>
    /// Toolbar按钮。
    /// </summary>
    [HtmlTargetElement("button", ParentTag = "gt:toolbar-group")]
    [HtmlTargetElement("a", ParentTag = "gt:toolbar-group")]
    public class ToolbarGroupItemTagHelper : TagHelperBase
    {
        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.AddCssClass("btn btn-sm btn-outline-secondary");
        }
    }
}