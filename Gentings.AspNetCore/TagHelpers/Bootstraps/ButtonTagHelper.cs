using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 按钮组件。
    /// </summary>
    [HtmlTargetElement("a", Attributes = AttributeName)]
    [HtmlTargetElement("button", Attributes = AttributeName)]
    [HtmlTargetElement("a", Attributes = OutlineAttributeName)]
    [HtmlTargetElement("button", Attributes = OutlineAttributeName)]
    public class ButtonTagHelper : TagHelperBase
    {
        private const string AttributeName = ".type";
        /// <summary>
        /// 按钮类型。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public ButtonType Type { get; set; } = ButtonType.None;

        private const string OutlineAttributeName = ".outline";
        /// <summary>
        /// 按钮类型。
        /// </summary>
        [HtmlAttributeName(OutlineAttributeName)]
        public ButtonType OutlineType { get; set; } = ButtonType.None;

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.AddClass("btn");
            var className = "btn-primary";
            if (Type != ButtonType.None) className = $"btn-{Type.ToLowerString()}";
            else if (OutlineType != ButtonType.None) className = $"btn-outline-{OutlineType.ToLowerString()}";
            output.AddClass(className);
        }
    }
}
