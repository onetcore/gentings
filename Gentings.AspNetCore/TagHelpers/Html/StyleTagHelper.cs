using System.Threading.Tasks;
using Gentings.AspNetCore.Syntax;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Html
{
    /// <summary>
    /// 样式节点。
    /// </summary>
    [HtmlTargetElement("style", Attributes = AttributeName)]
    public class StyleTagHelper : TagHelperBase
    {
        private const string AttributeName = ".mix";
        /// <summary>
        /// 是否简化代码。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public bool IsMix { get; set; }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var content = await output.GetChildContentAsync();
            if (content.IsEmptyOrWhiteSpace)
            {
                output.SuppressOutput();
                return;
            }

            var style = new CssSyntaxMixed(content.GetContent().Trim());
            output.AppendHtml(style.ToString());
        }
    }
}