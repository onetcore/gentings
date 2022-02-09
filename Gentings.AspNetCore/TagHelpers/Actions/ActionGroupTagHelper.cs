using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Actions
{
    /// <summary>
    /// 操作下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:action-group")]
    public class ActionGroupTagHelper : TagHelperBase
    {
        /// <summary>
        /// 对齐方式。
        /// </summary>
        public AlignMode Align { get; set; }

        /// <summary>
        /// 选中了才显示按钮。
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            if (Align == AlignMode.Vertical)
                output.AddCssClass("btn-group-vertical");
            else
                output.AddCssClass("btn-group");
            if (Disabled) output.AddCssClass("checked-enabled disabled");
        }
    }
}
