﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Toolbars
{
    /// <summary>
    /// Toolbar范围输入框分组。
    /// </summary>
    [HtmlTargetElement("gt:toolbar-range-group")]
    public class ToolbarInputRangeGroupTagHelper : ToolbarInputGroupTagHelper
    {
        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddCssClass("input-group input-group-sm input-group-range");
            output.AppendHtml(await output.GetChildContentAsync());
        }
    }
}