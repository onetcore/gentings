using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.RazorPages.TagHelpers.Html
{
    /// <summary>
    /// 是否添加当前按钮样式。
    /// </summary>
    [HtmlTargetElement("*", Attributes = AttributeName)]
    public class ActiveTagHelper : ViewContextableTagHelperBase
    {
        private const string AttributeName = ".active";
        private const string CurrentAttributeName = ".current";

        /// <summary>
        /// 激活值。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public string ActiveValue { get; set; }

        /// <summary>
        /// 当前值。
        /// </summary>
        [HtmlAttributeName(CurrentAttributeName)]
        public string CurrentValue { get; set; }

        /// <summary>
        /// 激活的样式。
        /// </summary>
        [HtmlAttributeName(".class")]
        public string ActiveClass { get; set; } = "active";

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (CurrentValue == null)
                CurrentValue = ViewContext.ViewBag.Current as string;
            if (string.Equals(ActiveValue, CurrentValue, StringComparison.OrdinalIgnoreCase))
            {
                output.AddCssClass(ActiveClass);
            }
        }
    }
}