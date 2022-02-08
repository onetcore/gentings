using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// Tab标签头部项目。
    /// </summary>
    [HtmlTargetElement("gt:tab-header", Attributes = "name")]
    public class TabHeaderTagHelper : TagHelperBase
    {
        /// <summary>
        /// 唯一名称。
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 是否激活。
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var content = await output.GetChildContentAsync();
            output.Render("li", builder =>
            {
                builder.AddCssClass("nav-item");
                if (Active)
                    builder.AddCssClass("active");
                builder.AppendTag("a", anchor =>
                {
                    anchor.AddCssClass("nav-link");
                    if (Active)
                        anchor.AddCssClass("active");
                    anchor.GenerateId($"{Name}-tab", "-");
                    anchor.MergeAttribute("data-bs-toggle", "tab");
                    anchor.MergeAttribute("href", $"#{Name}");
                    anchor.MergeAttribute("role", "tab");
                    anchor.InnerHtml.AppendHtml(content);
                });
            });
        }
    }
}