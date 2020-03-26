using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.RazorPages.TagHelpers.Bootstraps
{
    /// <summary>
    /// 下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:dropdown-menu")]
    public class DropdownMenuTagHelper : TagHelperBase
    {
        /// <summary>
        /// 图标。
        /// </summary>
        [HtmlAttributeName("icon")]
        public string IconName { get; set; }

        /// <summary>
        /// 显示文本。
        /// </summary>
        [HtmlAttributeName("text")]
        public string Text { get; set; }

        /// <summary>
        /// 菜单位置。
        /// </summary>
        [HtmlAttributeName("position")]
        public DropdownMenuPosition Position { get; set; }

        /// <summary>
        /// 标签。
        /// </summary>
        [HtmlAttributeName("tag")]
        public string TagName { get; set; } = "div";

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            return output.RenderAsync(TagName, async tag =>
            {
                tag.AddCssClass("dropdown");
                tag.AppendTag("button", x =>
                {
                    x.MergeAttribute("type", "button");
                    x.AddCssClass("btn btn-sm btn-outline-secondary dropdown-toggle");
                    x.MergeAttribute("data-toggle", "dropdown");
                    if (!string.IsNullOrEmpty(IconName))
                        x.AppendTag("span", span =>
                        {
                            span.MergeAttribute("data-feather", IconName);
                            span.AddCssClass("mr-1");
                        });
                    x.InnerHtml.AppendHtml(Text);
                });
                var content = await output.GetChildContentAsync();
                tag.AppendTag("div", x =>
                {
                    x.AddCssClass("dropdown-menu");
                    x.AddCssClass("dropdown-menu-" + Position.ToString().ToLower());
                    x.InnerHtml.AppendHtml(content);
                });
            });
        }
    }
}