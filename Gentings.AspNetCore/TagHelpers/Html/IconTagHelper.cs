using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Html
{
    /// <summary>
    /// 图标。
    /// </summary>
    [HtmlTargetElement("*", Attributes = ".icon")]
    [HtmlTargetElement("*", Attributes = ".icon-type")]
    public class IconTagHelper : TagHelperBase
    {
        /// <summary>
        /// 图标类型。
        /// </summary>
        [HtmlAttributeName(".icon-type")]
        public IconType? Type { get; set; }

        /// <summary>
        /// 图标样式名称。
        /// </summary>
        [HtmlAttributeName(".icon")]
        public string IconName { get; set; }

        /// <summary>
        /// 图标位置是否为末尾。
        /// </summary>
        [HtmlAttributeName(".icon-append")]
        public bool IsAppend { get; set; }

        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            if (string.IsNullOrEmpty(IconName) && Type != null)
                IconName = Type.ToDescriptionString();
        }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(IconName))
                return;
            var icon = new TagBuilder("span");
            icon.AddCssClass(IconName);
            if (!IsAppend)
                output.Content.AppendHtml(icon);
            output.Content.AppendHtml(await output.GetChildContentAsync());
            if (IsAppend)
                output.Content.AppendHtml(icon);
        }
    }
}
