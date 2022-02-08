using Gentings.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.Extensions.Sites.TagHelpers
{
    /// <summary>
    /// 页面头部代码。
    /// </summary>
    [HtmlTargetElement("gt:page-footer")]
    public class PageFooterTagHelper : PageTagHelperBase
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
            // 添加尾部代码
            var footer = Context.Settings.Footer;
            if (!string.IsNullOrWhiteSpace(footer))
                footer += "\r\n";
            footer += Context.Page.Footer;
            if (!string.IsNullOrWhiteSpace(footer))
                output.AppendHtml(footer.Trim());

            // 添加脚本
            var scripts = Context.Sections
                .Where(x => !string.IsNullOrWhiteSpace(x.Script))
                .Select(x => $"({x.Script!.Trim().Trim(';')})($('#{x.UniqueId}'));")
                .Join("\r\n").Trim();
            if (!string.IsNullOrWhiteSpace(scripts))
                output.AppendHtml("script", builder =>
                {
                    builder.MergeAttribute("type", "text/javascript");
                    builder.InnerHtml.AppendHtml(scripts);
                });
        }
    }
}
