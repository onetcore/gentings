﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 单选框列表标签。
    /// </summary>
    public abstract class RadioboxListTagHelper : ViewContextableTagHelperBase
    {
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
        /// 每项样式类型。
        /// </summary>
        [HtmlAttributeName("iclass")]
        public string ItemClass { get; set; }

        /// <summary>
        /// 每项选中样式类型。
        /// </summary>
        [HtmlAttributeName("istyle")]
        public CheckedStyle CheckedStyle { get; set; }

        /// <summary>
        /// 是否禁用。
        /// </summary>
        [HtmlAttributeName("disabled")]
        public bool Disabled { get; set; }

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
            var items = new Dictionary<string, object>();
            Init(items);
            foreach (var item in items)
            {
                output.Content.AppendHtml(Create(item.Key, item.Value.ToString(), IsChecked(item.Value)));
            }
            var builder = new TagBuilder("div");
            builder.AddCssClass("gt-radioboxlist");
            output.TagName = builder.TagName;
            output.MergeAttributes(builder);
        }

        /// <summary>
        /// 判断当前值是否为选中值。
        /// </summary>
        /// <param name="itemValue">当前选项的值。</param>
        /// <returns>返回判断结果。</returns>
        protected virtual bool IsChecked(object itemValue) =>
            string.Equals(itemValue.ToString(), Value);

        /// <summary>
        /// 附加复选项目列表，文本/值。
        /// </summary>
        /// <param name="items">复选框项目列表实例。</param>
        protected abstract void Init(IDictionary<string, object> items);

        private TagBuilder Create(string text, string value, bool isChecked)
        {
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("gt-radiobox");
            if (ItemClass != null)
                wrapper.AddCssClass(ItemClass);
            if (isChecked)
                wrapper.AddCssClass("checked");
            if (Disabled)
                wrapper.AddCssClass("disabled");
            wrapper.AddCssClass("checked-style-" + CheckedStyle.ToString().ToLower());

            var input = new TagBuilder("input");
            input.MergeAttribute("type", "radio");
            input.MergeAttribute("name", Name);
            input.MergeAttribute("value", value);
            if (isChecked)
                input.MergeAttribute("checked", "checked");
            input.TagRenderMode = TagRenderMode.SelfClosing;
            wrapper.InnerHtml.AppendHtml(input);

            var label = new TagBuilder("label");
            label.AddCssClass("box-wrapper");
            label.InnerHtml.AppendHtml("<div class=\"box-checked\"></div>");
            wrapper.InnerHtml.AppendHtml(label);

            var span = new TagBuilder("span");
            span.InnerHtml.AppendHtml(text);
            wrapper.InnerHtml.AppendHtml(span);
            return wrapper;
        }
    }
}