﻿using Gentings.AspNetCore.Properties;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 颜色组件。
    /// </summary>
    [HtmlTargetElement("gt:color")]
    public class ColorTagHelper : ViewContextableTagHelperBase
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
                    input.MergeAttributes(output);
                    input.TagRenderMode = TagRenderMode.SelfClosing;
                    input.MergeAttribute("autocomplete", "off");
                    input.AddCssClass("form-control");
                    input.AddCssClass("form-color");
                    input.MergeAttribute("name", Name);
                    input.GenerateId(Name, "_");
                    input.MergeAttribute("readonly", "readonly");
                    if (Value == null)
                    {
                        input.MergeAttribute("type", "text");
                    }
                    else
                    {
                        input.MergeAttribute("type", "color");
                        input.MergeAttribute("value", Value?.ToString());
                    }
                    input.MergeAttribute("onclick", "if(this.type!='color'){this.type='color';this.click(); return false;}");
                });
                builder.AppendHtml("button", button =>
                {
                    button.MergeAttribute("title", Resources.ColorTagHelper_Clear);
                    button.MergeAttribute("onclick", "$(this).prev().attr('type','text').val('');");
                    button.MergeAttribute("type", "button");
                    button.AppendHtml("span", span => span.AddCssClass("bi-x"));
                });
                builder.AddCssClass("input-group-append");
            });
        }
    }
}
