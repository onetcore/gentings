using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps.Actions
{
    /// <summary>
    /// 操作下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:action-groupmenu")]
    public class ActionGroupMenuTagHelper : TagHelperBase
    {
        /// <summary>
        /// 显示文本字符串。
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 按钮类型。
        /// </summary>
        public ButtonType Mode { get; set; }

        /// <summary>
        /// 按钮类型。
        /// </summary>
        public bool Outline { get; set; }

        /// <summary>
        /// 选中了才显示按钮。
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.Render("a", builder =>
            {
                if (Disabled) builder.AddCssClass("checked-enabled disabled");
                builder.AddCssClass("btn");
                if (Outline) builder.AddCssClass("btn-outline-" + Mode.ToLowerString());
                else builder.AddCssClass("btn-" + Mode.ToLowerString());

                builder.MergeAttribute("href", "#");
                builder.AddCssClass("dropdown-toggle");
                builder.MergeAttribute("data-bs-toggle", "dropdown");
                builder.MergeAttribute("aria-expanded", "false");
                builder.InnerHtml.AppendHtml(Text);
            });

            var content = await output.GetChildContentAsync();
            var menu = new TagBuilder("div");
            menu.AddCssClass("dropdown-menu dropdown-menu-end");
            menu.InnerHtml.AppendHtml(content);
            output.PostElement.AppendHtml(menu);
        }
    }
}
