using Gentings.AspNetCore.TagHelpers;
using Gentings.Extensions.Sites.Sections;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.Extensions.Sites.TagHelpers
{
    /// <summary>
    /// 呈现自定义的节点。
    /// </summary>
    [HtmlTargetElement("gt:sections")]
    public class SectionTagHelper : PageTagHelperBase
    {
        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Context == null)
            {
                output.SuppressOutput();
                return;
            }
            var sectionManager = GetRequiredService<ISectionManager>();
            foreach (var section in Context.Sections)
            {
                var isFluid = section.IsFluid ?? Context.Page.IsFluid ?? Context.Settings.IsFluid;
                await output.AppendHtmlAsync(section.TagName ?? "section", async builder =>
                {
                    if (section.Name != null)
                        builder.AddCssClass(section.Name);
                    if (isFluid == true)
                        builder.AddCssClass("container-fluid");
                    else if (isFluid == false)
                        builder.AddCssClass("container");
                    var context = new SectionContext(section, Context, ViewContext, builder);
                    var sectionType = sectionManager.GetSection(section.SectionType);
                    await sectionType.ProcessAsync(context, output);
                });
            }
        }
    }
}
