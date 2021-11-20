using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Gentings.AspNetCore.Bootstraps
{
    /// <summary>
    /// 抽屉式模板组件。
    /// </summary>
    [HtmlTargetElement("gt:offcanvas")]
    public class OffcanvasTagHelper : TagHelperBase
    {
        /// <summary>
        /// 方向。
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// 标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            return output.RenderAsync("div", async builder =>
            {
                builder.AddCssClass("offcanvas");
                builder.AddCssClass("offcanvas-" + Direction.ToLowerString());
                builder.AppendTag("div", header =>
                {
                    header.AddCssClass("offcanvas-header");
                    header.AppendTag("h3", title => title.InnerHtml.AppendHtml(title));
                    header.InnerHtml.AppendHtml("<button type=\"button\" class=\"btn-close text-reset\" data-bs-dismiss=\"offcanvas\"></button>");
                });
                var content = await output.GetChildContentAsync();
                builder.AppendTag("div", body =>
                {
                    body.AddCssClass("offcanvas-body");
                    body.InnerHtml.AppendHtml(content);
                });
            });
        }
    }
}
