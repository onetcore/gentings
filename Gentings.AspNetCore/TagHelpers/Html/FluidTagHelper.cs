using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Html
{
    /// <summary>
    /// 是否为宽屏容器。
    /// </summary>
    [HtmlTargetElement("*", Attributes = AttributeName)]
    public class FluidTagHelper : TagHelperBase
    {
        private const string AttributeName = ".fluid";

        /// <summary>
        /// 是否宽屏。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public bool? IsFluid { get; set; }

        /// <summary>
        /// 居中对齐样式。
        /// </summary>
        [HtmlAttributeName(".cluid-container")]
        public string Container { get; set; } = "container-lg";

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (IsFluid == true)
                output.AddCssClass("container-fluid");
            else if (IsFluid == false)
                output.AddCssClass(Container);
        }
    }
}
