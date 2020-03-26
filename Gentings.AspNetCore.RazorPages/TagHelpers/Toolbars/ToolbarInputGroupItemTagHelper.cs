using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.RazorPages.TagHelpers.Toolbars
{
    /// <summary>
    /// Toolbar输入框和下拉列表框。
    /// </summary>
    [HtmlTargetElement("input", ParentTag = "gt:toolbar-input-group")]
    [HtmlTargetElement("input", ParentTag = "gt:toolbar-range-group")]
    [HtmlTargetElement("input", ParentTag = "gt:toolbar-form-group")]
    [HtmlTargetElement("select", ParentTag = "gt:toolbar-input-group")]
    [HtmlTargetElement("select", ParentTag = "gt:toolbar-range-group")]
    [HtmlTargetElement("select", ParentTag = "gt:toolbar-form-group")]
    [HtmlTargetElement("gt:datetimepicker", ParentTag = "gt:toolbar-input-group")]
    [HtmlTargetElement("gt:datetimepicker", ParentTag = "gt:toolbar-range-group")]
    [HtmlTargetElement("gt:datetimepicker", ParentTag = "gt:toolbar-form-group")]
    public class ToolbarInputGroupItemTagHelper : TagHelperBase
    {
        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.AddCssClass("form-control form-control-sm");
        }
    }
}