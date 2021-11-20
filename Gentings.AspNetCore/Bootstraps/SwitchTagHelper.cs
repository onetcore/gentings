using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.Bootstraps
{
    /// <summary>
    /// 开关按钮。
    /// </summary>
    [HtmlTargetElement("gt:switch")]
    public class SwitchTagHelper : ViewContextableTagHelperBase
    {
        /// <summary>
        /// 开关类型。
        /// </summary>
        public SwitchType Type { get; set; }

        /// <summary>
        /// 启用文字。
        /// </summary>
        public string On { get; set; }

        /// <summary>
        /// 关闭文字。
        /// </summary>
        public string Off { get; set; }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var id = $"switch-{GetCounter()}";
            if (context.AllAttributes.TryGetAttribute("id", out var attribute) && attribute.Value != null)
                id = attribute.Value.ToString();
            output.Render("input", builder =>
            {
                builder.GenerateId(id, "-");
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("data-switch", Type.ToString().ToLower());
            });
            var label = new TagBuilder("label");
            label.MergeAttribute("for", id);
            label.MergeAttribute("data-on-label", On);
            label.MergeAttribute("data-off-label", Off);
            output.PostElement.AppendHtml(label);
        }
    }
}
