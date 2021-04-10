using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Html
{
    /// <summary>
    /// 属性标签，如果值不为空则添加标签值，为空不显示。
    /// </summary>
    [HtmlTargetElement("*", Attributes = ValuesPrefix + "*")]
    public class AttributeTagHelper : TagHelperBase
    {
        private const string ValuesPrefix = ".attr-";
        private const string ValuesDictionaryName = ".attr-data";
        private IDictionary<string, object> _names;

        /// <summary>
        /// 样式列表。
        /// </summary>
        [HtmlAttributeName(ValuesDictionaryName, DictionaryAttributePrefix = ValuesPrefix)]
        public IDictionary<string, object> Names
        {
            get => _names ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            set => _names = value;
        }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var names = Names
                .Select(x => new KeyValuePair<string, string>(x.Key, x.Value?.ToString()))
                .Where(x => !string.IsNullOrWhiteSpace(x.Value))
                .ToList();
            foreach (var name in names)
            {
                output.SetAttribute(name.Key, name.Value);
            }
        }
    }
}