using Gentings.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections;
using System.Collections.Generic;

namespace Gentings.AspNetCore.Bootstraps
{
    /// <summary>
    /// 复选框列表按钮。
    /// </summary>
    public abstract class CheckBoxListTagHelper : ViewContextableTagHelperBase
    {
        /// <summary>
        /// 名称。
        /// </summary>
        [HtmlAttributeName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 以“,”分割值。。
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 设置属性模型。
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

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
                {
                    if (For.Model is string str)
                        Value = str;
                    else if (For.Model is IEnumerable array)
                        Value = array.Join();
                    else
                        Value = For.Model?.ToString();
                }
            }
            if (Value != null)
                Value = $",{Value},";
        }

        /// <summary>
        /// 判断选中的状态。
        /// </summary>
        /// <param name="current">当前项目值。</param>
        /// <returns>返回判断结果。</returns>
        protected virtual bool IsChecked(object current)
        {
            return Value?.IndexOf($",{current},") >= 0;
        }

        /// <summary>
        /// 附加复选项目列表，文本/值。
        /// </summary>
        /// <param name="items">复选框项目列表实例。</param>
        protected abstract void Init(IDictionary<string, object> items);

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Process(output, "checkbox");
        }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="type">标签类型：checkbox,radio。</param>
        protected void Process(TagHelperOutput output, string type)
        {
            var items = new Dictionary<string, object>();
            Init(items);
            output.Render("div", builder =>
            {
                builder.AddCssClass($"{type}-list");
                foreach (var item in items)
                {
                    Append(builder, type, item);
                }
            });
        }

        private void Append(TagBuilder builder, string type, KeyValuePair<string, object> item)
        {
            var input = new TagBuilder("input");
            input.AddCssClass("form-check-input");
            input.MergeAttribute("type", type);
            if (!string.IsNullOrEmpty(Name))
                input.MergeAttribute("name", Name);
            if (item.Value != null)
                input.MergeAttribute("value", item.Value.ToString());
            if (IsChecked(item.Value))
                input.MergeAttribute("checked", null);
            var label = new TagBuilder("label");
            label.AddCssClass("form-check");
            label.AddCssClass($"{type}-item");
            label.InnerHtml.AppendHtml(input);
            builder.InnerHtml.AppendHtml(label);
        }
    }
}
