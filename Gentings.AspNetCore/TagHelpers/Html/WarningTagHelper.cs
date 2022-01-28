using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections;

namespace Gentings.AspNetCore.TagHelpers.Html
{
    /// <summary>
    /// 如果数据为空显示警告标签。
    /// </summary>
    [HtmlTargetElement("*", Attributes = AttributeName)]
    [HtmlTargetElement("*", Attributes = DataAttributeName)]
    public class WarningTagHelper : TagHelperBase
    {
        private const string AttributeName = ".warning-text";
        /// <summary>
        /// 警告消息。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public string? Text { get; set; }

        /// <summary>
        /// 横跨列数，在tbody,tr,td上使用。
        /// </summary>
        [HtmlAttributeName("colspan")]
        public int? Colspan { get; set; }

        private const string DataAttributeName = ".warning";
        /// <summary>
        /// 是否显示警告标签。
        /// </summary>
        [HtmlAttributeName(DataAttributeName)]
        public object? Data { get; set; }

        /// <summary>
        /// 是否显示。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        protected bool IsAttached()
        {
            if (Data is null)
                return true;
            if (Data is bool bValue)
                return !bValue;
            if (Data is IEnumerable value)
                return !value.GetEnumerator().MoveNext();
            return false;
        }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (IsAttached())
            {
                output.Attributes.Clear();
                switch (context.TagName.ToLower())
                {
                    case "tr":
                        output.AppendHtml("td", RenderWarning);
                        break;
                    case "td":
                        output.Render("td", RenderWarning);
                        break;
                    case "tbody":
                        output.AppendHtml("tr", builder => builder.AppendTag("td", RenderWarning));
                        break;
                    default:
                        output.Render("div", RenderWarning);
                        break;
                }
            }
        }

        private void RenderWarning(TagBuilder builder)
        {
            if (builder.TagName == "td")
                builder.MergeAttribute("colspan", Colspan?.ToString() ?? "100");
            builder.AddCssClass("null-warning");
            builder.AppendTag("i", i => i.AddCssClass("bi-exclamation-circle"));
            builder.AppendTag("div", div => div.InnerHtml.AppendHtml(Text));
        }
    }
}
