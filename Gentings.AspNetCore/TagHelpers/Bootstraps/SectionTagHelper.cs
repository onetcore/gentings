using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 页面节点标签。
    /// </summary>
    [HtmlTargetElement("section", Attributes = AttributeName)]
    public class SectionTagHelper : ViewContextableTagHelperBase
    {
        private const string AttributeName = ".name";
        /// <summary>
        /// 当前Section的样式名称。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public string ClassName { get; set; }

        /// <summary>
        /// 内部样式。
        /// </summary>
        [HtmlAttributeName(".inner")]
        public string InnerClassName { get; set; }

        /// <summary>
        /// 是否宽屏。
        /// </summary>
        public bool? IsFluid { get; set; }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // 初始设置
            var root = new TagBuilder("section");
            root.AddCssClass(ClassName);

            //宽屏设置
            var wrapper = root;
            if (IsFluid == null && ViewContext.ViewData.TryGetValue("IsFluid", out var fluid) && fluid is bool isFluid)
                IsFluid = isFluid;
            if (IsFluid != null)
            {
                var container = new TagBuilder("div");
                wrapper.InnerHtml.AppendHtml(container);
                container.AddCssClass(IsFluid == true ? "container-fluid" : "container");
                wrapper = container;
            }

            //内部容器设置
            if (!string.IsNullOrEmpty(InnerClassName))
            {
                var container = new TagBuilder("div");
                wrapper.InnerHtml.AppendHtml(container);
                container.AddCssClass(InnerClassName);
                wrapper = container;
            }

            //呈现内容
            output.MergeAttributes(root);
            output.Content.AppendHtml(root.InnerHtml);
            var content = await output.GetChildContentAsync();
            wrapper.InnerHtml.AppendHtml(content);
        }
    }
}