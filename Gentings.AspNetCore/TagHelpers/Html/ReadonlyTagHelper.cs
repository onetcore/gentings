using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Html
{
    /// <summary>
    /// 只读属性。
    /// </summary>
    [HtmlTargetElement("*", Attributes = AttributeName)]
    public class ReadonlyTagHelper : TagHelper
    {
        private const string AttributeName = ".readonly";
        /// <summary>
        /// 是否只读。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public bool? Readonly { get; set; }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Readonly == true)
                output.SetAttribute("readonly", "readonly");
        }
    }
}