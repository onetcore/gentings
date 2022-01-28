using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 单选框列表标签。
    /// </summary>
    public abstract class RadioListTagHelper : CheckBoxListTagHelper
    {
        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            if (string.IsNullOrEmpty(Name) && For != null)
            {
                Name = ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name);
                if (Value == null)
                    Value = For.Model?.ToString();
            }
        }

        /// <summary>
        /// 判断选中的状态。
        /// </summary>
        /// <param name="current">当前项目值。</param>
        /// <returns>返回判断结果。</returns>
        protected override bool IsChecked(object? current)
        {
            if (Value == null && current == null)
                return true;
            return Value?.ToString().Equals(current?.ToString(), StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Process(output, "radio");
        }
    }
}
