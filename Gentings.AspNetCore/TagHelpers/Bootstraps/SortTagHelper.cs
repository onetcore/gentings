using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 排序标签。
    /// </summary>
    [HtmlTargetElement("th", Attributes = AttributeName)]
    public class SortTagHelper : ViewContextableTagHelperBase
    {
        private const string AttributeName = ".sort";
        /// <summary>
        /// 排序。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public Enum? OrderBy { get; set; }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.AddCssClass("sorting");
            var current = OrderBy?.ToString("d") ?? "0";
            output.SetAttribute("_ajax.order", current);
            if (HttpContext.Request.Query.TryGetValue("order", out var order) && current == order)
            {
                if (HttpContext.Request.Query.TryGetValue("desc", out var value) && bool.TryParse(value, out var desc) && desc)
                    output.AddCssClass("sorting-desc");
                else
                    output.AddCssClass("sorting-asc");
            }
        }
    }
}