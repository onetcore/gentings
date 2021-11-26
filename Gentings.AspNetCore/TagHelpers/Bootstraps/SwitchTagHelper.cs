using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 开关按钮。
    /// </summary>
    [HtmlTargetElement("gt:switch")]
    public class SwitchTagHelper : ViewContextableTagHelperBase
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
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var id = $"switch-{GetCounter()}";
            if (context.AllAttributes.TryGetAttribute("id", out var attribute) && attribute.Value != null)
                id = attribute.Value.ToString();
            output.Render("input", builder =>
            {
                builder.GenerateId(id, "-");
                builder.MergeAttribute("type", "checkbox");
                if (!string.IsNullOrEmpty(Name))
                    builder.MergeAttribute("name", Name);
                if (Value != null)
                    builder.MergeAttribute("value", Value.ToString());
                if (IsChecked)
                    builder.MergeAttribute("checked", null);
                builder.TagRenderMode = TagRenderMode.SelfClosing;
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
