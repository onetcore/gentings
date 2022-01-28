using Gentings.AspNetCore.TagHelpers;
using Gentings.Documents;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.Extensions.Sites.TagHelpers
{
    /// <summary>
    /// 页面头部代码。
    /// </summary>
    [HtmlTargetElement("gt:page-header")]
    public class PageHeaderTagHelper : PageTagHelperBase
    {
        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Context == null)
            {
                output.SuppressOutput();
                return;
            }
            output.TagName = null;
            // 添加头部代码
            var header = Context.Settings.Header;
            if (!string.IsNullOrWhiteSpace(header))
                header += "\r\n";
            header += Context.Page.Header;
            if (!string.IsNullOrWhiteSpace(header))
                output.AppendHtml(header.Trim());

            // 添加样式
            var styles = Context.Sections
                .Where(x => !string.IsNullOrWhiteSpace(x.Style))
                .Select(x => new CssSyntaxMixed(x.Style)
                .ToString($"#{x.UniqueId}"))
                .Join(" ")
                .Trim();
            if (!string.IsNullOrWhiteSpace(styles))
                output.AppendHtml("style", builder => builder.InnerHtml.AppendHtml(styles));
        }
    }
}
