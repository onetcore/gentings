using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 日期时间组件。
    /// </summary>
    [HtmlTargetElement("gt:datetimepicker", TagStructure = TagStructure.WithoutEndTag)]
    public class DateTimePickerTagHelper : ViewContextableTagHelperBase
    {
        /// <summary>
        /// 类型。
        /// </summary>
        public enum PickerType
        {
            /// <summary>
            /// 日期时间。
            /// </summary>
            DateTime,
            /// <summary>
            /// 日期。
            /// </summary>
            Date,
            /// <summary>
            /// 时间。
            /// </summary>
            Time,
        }

        /// <summary>
        /// 类型。
        /// </summary>
        [HtmlAttributeName("type")]
        public PickerType Type { get; set; }

        /// <summary>
        /// 模型表达式。
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }
        /// <summary>
        /// 名称。
        /// </summary>
        [HtmlAttributeName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 当前值。
        /// </summary>
        [HtmlAttributeName("value")]
        public string Value { get; set; }

        /// <summary>
        /// 关联最小值。
        /// </summary>
        [HtmlAttributeName("min-selector")]
        public string MinSelector { get; set; }

        /// <summary>
        /// 关联最大值。
        /// </summary>
        [HtmlAttributeName("max-selector")]
        public string MaxSelector { get; set; }

        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            if (string.IsNullOrEmpty(Name) && For != null)
            {
                Name = ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name);
                Value = For.Model?.ToString();
            }
        }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Render("input", tag =>
            {
                tag.MergeAttribute("type", "text");
                tag.GenerateId(Name, "_");
                tag.MergeAttribute("name", Name);
                tag.MergeAttribute("autocomplete", "off");
                tag.MergeAttribute("js-date", Type.ToString().ToLower());
                if (MinSelector != null)
                    tag.MergeAttribute("js-date-min", MinSelector);
                if (MaxSelector != null)
                    tag.MergeAttribute("js-date-max", MaxSelector);
                if (Value != null)
                    tag.MergeAttribute("value", Value);
            });
        }
    }
}