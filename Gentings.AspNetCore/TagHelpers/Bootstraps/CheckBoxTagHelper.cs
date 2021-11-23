using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 复选按钮。
    /// </summary>
    [HtmlTargetElement("gt:checkbox", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class CheckBoxTagHelper : ViewContextableTagHelperBase
    {
        /// <summary>
        /// 名称。
        /// </summary>
        [HtmlAttributeName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 选定对象值。
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 是否为开关。
        /// </summary>
        public bool Switch { get; set; }

        /// <summary>
        /// 设置属性模型。
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        /// <summary>
        /// 是否选中。
        /// </summary>
        [HtmlAttributeName("checked")]
        public bool IsChecked { get; set; }

        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            if (string.IsNullOrEmpty(Name) && For != null)
            {
                Name = ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name);
                IsChecked = Convert.ToBoolean(For.Model);
            }
        }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var builder = new TagBuilder("input");
            builder.AddCssClass("form-check-input");
            builder.MergeAttribute("type", "checkbox");
            if (!string.IsNullOrEmpty(Name))
                builder.MergeAttribute("name", Name);
            builder.MergeAttribute("value", Value?.ToString() ?? "true");
            if (IsChecked)
                builder.MergeAttribute("checked", null);
            builder.TagRenderMode = TagRenderMode.SelfClosing;
            var content = await output.GetChildContentAsync();
            if (Switch || !content.IsEmptyOrWhiteSpace)
            {
                output.Render("label", label =>
                {
                    label.AddCssClass("form-check");
                    if (Switch)
                        label.AddCssClass("form-switch");
                    label.InnerHtml.AppendHtml(builder);
                    label.InnerHtml.AppendHtml(content);
                });
            }
            else
                output.Render(builder);
        }
    }
}
