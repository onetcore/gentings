using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 图标。
    /// </summary>
    [HtmlTargetElement("gt:icon", Attributes = "name")]
    [HtmlTargetElement("gt:icon", Attributes = "type")]
    public class IconTagHelper : TagHelperBase
    {
        /// <summary>
        /// 图标类型。
        /// </summary>
        [HtmlAttributeName("type")]
        public IconType Type { get; set; } = IconType.None;

        /// <summary>
        /// 图标样式名称。
        /// </summary>
        [HtmlAttributeName("name")]
        public string? IconName { get; set; }

        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            if (string.IsNullOrEmpty(IconName) && Type != IconType.None)
                IconName = Type.ToDescriptionString();
        }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(IconName))
                return;

            output.Process("span", builder =>
            {
                builder.AddCssClass(IconName);
            });
        }
    }
}
