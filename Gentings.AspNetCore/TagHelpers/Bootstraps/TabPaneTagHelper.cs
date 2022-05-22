using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// Tab内容项目。
    /// </summary>
    [HtmlTargetElement("gt:tab-pane", Attributes = "name")]
    public class TabPaneTagHelper : TagHelperBase
    {
        /// <summary>
        /// 唯一名称，和<see cref="TabHeaderTagHelper.Name"/>对应。
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
            output.Process("div", builder =>
            {
                builder.AddCssClass("tab-pane fade");
                if (Active)
                    builder.AddCssClass("show active");
                builder.GenerateId(Name!, "-");
                builder.InnerHtml.AppendHtml(content);
            });
        }
    }
}