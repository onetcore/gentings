using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.RazorPages.TagHelpers.Toolbars
{
    /// <summary>
    /// Toolbar按钮。
    /// </summary>
    [HtmlTargetElement("a", ParentTag = "gt:toolbar-btn-group")]
    [HtmlTargetElement("a", ParentTag = "gt:toolbar-input-group")]
    [HtmlTargetElement("a", ParentTag = "gt:append")]
    [HtmlTargetElement("a", ParentTag = "gt:prepend")]
    [HtmlTargetElement("button", ParentTag = "gt:toolbar-btn-group")]
    [HtmlTargetElement("button", ParentTag = "gt:toolbar-input-group")]
    [HtmlTargetElement("button", ParentTag = "gt:append")]
    [HtmlTargetElement("button", ParentTag = "gt:prepend")]
    public class ToolbarGroupItemTagHelper : TagHelperBase
    {
        /// <summary>
        /// 是否为默认样式。
        /// </summary>
        [HtmlAttributeName(".default")]
        public bool IsDefault { get; set; } = true;

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var classNames = "btn btn-sm";
            if (IsDefault)
                classNames += " btn-outline-secondary";
            output.AddCssClass(classNames);
        }
    }
}