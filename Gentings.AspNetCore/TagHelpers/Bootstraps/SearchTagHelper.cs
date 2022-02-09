using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 检索组件。
    /// </summary>
    [HtmlTargetElement("gt:search")]
    public class SearchTagHelper : ViewContextableTagHelperBase
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 选定对象值。
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        /// 提示字符串。
        /// </summary>
        public string? Placeholder { get; set; }

        /// <summary>
        /// 类型。
        /// </summary>
        public string Type { get; set; } = "text";

        /// <summary>
        /// 是否为小号输入框。
        /// </summary>
        public bool Small { get; set; }

        /// <summary>
        /// 设置属性模型。
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression? For { get; set; }

        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            if (string.IsNullOrEmpty(Name) && For != null)
            {
                Name = ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name);
                Value = For.Model;
            }
        }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Process("div", builder =>
            {
                builder.AppendHtml("input", input =>
                {
                    input.AddCssClass("form-control");
                    if (Small)
                        input.AddCssClass("form-control-sm");
                    if (!string.IsNullOrEmpty(Placeholder))
                        input.MergeAttribute("placeholder", Placeholder);
                    input.MergeAttribute("name", Name);
                    input.GenerateId(Name, "_");
                    foreach (var attr in output.Attributes)
                    {
                        input.MergeAttribute(attr.Name, attr.Value?.ToString(), true);
                    }
                    input.MergeAttribute("type", Type);
                    if (Value != null)
                        input.MergeAttribute("value", Value?.ToString());
                    input.TagRenderMode = TagRenderMode.SelfClosing;
                });
                builder.AppendHtml("button", button =>
                {
                    button.MergeAttribute("type", "submit");
                    button.AppendHtml("span", span => span.AddCssClass("bi-search"));
                });
                output.Attributes.Clear();
                builder.AddCssClass("input-group-append");
            });
        }
    }
}
