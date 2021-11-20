﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Gentings.AspNetCore.Bootstraps
{
    /// <summary>
    /// 操作下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:action-dropdownmenu")]
    public class ActionDropDownMenuTagHelper : TagHelperBase
    {
        /// <summary>
        /// 图标类型。
        /// </summary>
        [HtmlAttributeName("type")]
        public AlignMode IconType { get; set; } = AlignMode.Horizontal;

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            return output.RenderAsync("div", async builder =>
            {
                builder.AddCssClass("dropdown");
                builder.AppendTag("a", anchor =>
                {
                    anchor.MergeAttribute("href", "#");
                    anchor.AddCssClass("dropdown-toggle");
                    anchor.AddCssClass("action-dropdown");
                    anchor.MergeAttribute("data-bs-toggle", "dropdown");
                    anchor.MergeAttribute("aria-expanded", "false");
                    anchor.AppendTag("span", span =>
                    {
                        if (IconType == AlignMode.Horizontal)
                            span.AddCssClass("bi-three-dots");
                        else
                            span.AddCssClass("bi-three-dots-vertical");
                    });
                });
                var content = await output.GetChildContentAsync();
                builder.AppendTag("div", menu =>
                {
                    menu.AddCssClass("dropdown-menu dropdown-menu-end");
                    menu.InnerHtml.AppendHtml(content);
                });
            });
        }
    }

    /// <summary>
    /// 操作下拉列表框。
    /// </summary>
    [HtmlTargetElement("a", ParentTag = "gt:action-dropdownmenu")]
    public class ActionMenuItemTagHelper : TagHelperBase
    {
        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.AddClass("dropdown-item");
        }
    }
}
