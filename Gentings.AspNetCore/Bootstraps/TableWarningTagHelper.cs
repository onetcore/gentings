using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections;
using System.Threading.Tasks;

namespace Gentings.AspNetCore.Bootstraps
{
    /// <summary>
    /// 警告标签。
    /// </summary>
    [HtmlTargetElement("gt:warning", ParentTag = "tbody", Attributes = AttributeName)]
    public class TableWarningTagHelper : TagHelperBase
    {
        private const string AttributeName = "colspan";

        /// <summary>
        /// 横跨列数。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public int Colspan { get; set; }

        /// <summary>
        /// 是否显示。
        /// </summary>
        [HtmlAttributeName("data")]
        public object Data { get; set; }

        /// <summary>
        /// 是否显示。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        protected bool IsAttached()
        {
            if (Data is bool bValue)
                return !bValue;
            if (Data is IEnumerable value)
                return !value.GetEnumerator().MoveNext();
            return Data == null;
        }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (IsAttached())
            {
                var content = await output.GetChildContentAsync();
                output.TagName = "tr";
                var builder = new TagBuilder("td");
                builder.AddCssClass("text-muted null-warning");
                if (Colspan > 1)
                    builder.MergeAttribute("colspan", Colspan.ToString());
                builder.InnerHtml.AppendHtml("<i class=\"bi-exclamation-circle\"></i> ");
                if (!content.IsEmptyOrWhiteSpace)
                    builder.InnerHtml.AppendHtml(content);
                output.Content.AppendHtml(builder);
            }
            else
            {
                output.SuppressOutput();
            }
        }
    }
}
