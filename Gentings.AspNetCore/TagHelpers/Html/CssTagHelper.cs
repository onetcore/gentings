﻿using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Html
{
    /// <summary>
    /// 样式标签。
    /// </summary>
    [HtmlTargetElement("*", Attributes = CssValuesPrefix + "*")]
    public class CssTagHelper : TagHelperBase
    {
        private const string CssValuesPrefix = ".css-";
        private const string CssValuesDictionaryName = ".css-data";
        private IDictionary<string, object>? _styles;

        /// <summary>
        /// 样式列表。
        /// </summary>
        [HtmlAttributeName(CssValuesDictionaryName, DictionaryAttributePrefix = CssValuesPrefix)]
        public IDictionary<string, object> Styles
        {
            get
            {
                if (_styles == null)
                {
                    _styles = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                }

                return _styles;
            }
            set => _styles = value;
        }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var builder = new StringBuilder();
            var tag = new TagBuilder("div");
            foreach (var style in Styles)
            {
                var value = style.Value?.ToString()?.Trim();
                tag.MergeAttribute(style.Key, value, true);
                builder.Append($"{style.Key}:{value};");
            }

            builder.Append(output.GetAttribute("style"));
            if (builder.Length > 0)
                tag.MergeAttribute("style", builder.ToString(), true);
            output.MergeAttributes(tag);
        }
    }
}