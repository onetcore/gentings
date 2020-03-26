using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.RazorPages.TagHelpers.Toolbars
{
    /// <summary>
    /// Toolbar附加按钮。
    /// </summary>
    [HtmlTargetElement("gt:append-search", ParentTag = "gt:toolbar-input-group", TagStructure = TagStructure.WithoutEndTag)]
    public class ToolbarSearchTagHelper : TagHelperBase
    {
        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Render("div", tag =>
            {
                tag.AddCssClass("input-group-append");
                tag.AppendTag("button", x =>
                {
                    x.MergeAttribute("type", "submit");
                    x.AddCssClass("btn btn-sm btn-outline-secondary");
                    x.AppendTag("span", i => i.MergeAttribute("data-feather", "search"));
                });
            });
        }
    }
}