using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.RazorPages.TagHelpers.Toolbars
{
    /// <summary>
    /// 范围输入框分割项。
    /// </summary>
    [HtmlTargetElement("divider", ParentTag = "gt:toolbar-range-group", TagStructure = TagStructure.WithoutEndTag)]
    public class ToolbarInputRangeGroupDividerTagHelper : TagHelperBase
    {
        /// <summary>
        /// 分割字符串。
        /// </summary>
        [HtmlAttributeName("text")]
        public string Text { get; set; } = "~";

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "label";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddCssClass("pr-2 pl-2");
            output.Content.AppendHtml(Text);
        }
    }
}