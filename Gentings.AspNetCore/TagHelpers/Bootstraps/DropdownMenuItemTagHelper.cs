using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 下拉列表框菜单项。
    /// </summary>
    [HtmlTargetElement("a", ParentTag = "gt:dropdown-menu")]
    [HtmlTargetElement("button", ParentTag = "gt:dropdown-menu")]
    public class DropdownMenuItemTagHelper : TagHelperBase
    {
        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.AddCssClass("dropdown-item");
        }
    }
}