using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Html
{
    /// <summary>
    /// 图片错误标签。
    /// </summary>
    [HtmlTargetElement("img", Attributes = AttributeName)]
    public class ImageErrorTagHelper : TagHelperBase
    {
        private const string AttributeName = ".error";

        /// <summary>
        /// 默认地址。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public string? Defsrc { get; set; }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(string.IsNullOrWhiteSpace(Defsrc))
                return;
            if (Defsrc.StartsWith("~/"))
                Defsrc = Defsrc[1..];
            output.SetAttribute("def", Defsrc);
            output.SetAttribute("onerror", "if(this.src!=this.getAttribute('def'))this.src=this.getAttribute('def');");
        }
    }
}
