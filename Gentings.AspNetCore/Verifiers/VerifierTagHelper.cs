using Gentings.AspNetCore.Properties;
using Gentings.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.Verifiers
{
    /// <summary>
    /// 验证码图片。
    /// </summary>
    [HtmlTargetElement("gt:verifier", Attributes = AttributeName)]
    public class VerifierTagHelper : TagHelperBase
    {
        /// <summary>
        /// 字符个数。
        /// </summary>
        [HtmlAttributeName("length")]
        public int Length { get; set; } = 6;

        /// <summary>
        /// 字体大小。
        /// </summary>
        [HtmlAttributeName("size")]
        public int FontSize { get; set; } = 16;

        /// <summary>
        /// 高度。
        /// </summary>
        [HtmlAttributeName("height")]
        public int Height { get; set; } = 32;

        private const string AttributeName = "key";
        /// <summary>
        /// 验证唯一键。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public string Key { get; set; } = "login";

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Process("img", builder =>
            {
                var url = $"/vcode-{Key}.png?n={Length}&s={FontSize}&h={Height}";
                builder.MergeAttribute("src", url);
                builder.MergeAttribute("onclick", $"this.src='{url}&_' + (+new Date());");
                builder.MergeAttribute("title", Resources.VerifierTagHelper_ClickRefresh);
            });
        }
    }
}