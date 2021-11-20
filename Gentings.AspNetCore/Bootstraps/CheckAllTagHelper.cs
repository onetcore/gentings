using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.Bootstraps
{
    /// <summary>
    /// 全选按钮。
    /// </summary>
    [HtmlTargetElement("gt:checkall", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class CheckAllTagHelper : TagHelperBase
    {
        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Render("input", builder =>
            {
                builder.TagRenderMode = TagRenderMode.SelfClosing;
                builder.AddCssClass("form-check-input");
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("onclick", "$(this).parents('.data-list').find('.data-item input[type=checkbox]').prop('checked', this.checked);");
            });
        }
    }
}
