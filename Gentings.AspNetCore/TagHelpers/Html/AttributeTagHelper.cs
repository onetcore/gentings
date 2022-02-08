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
        private IDictionary<string, object>? _attributes;

        /// <summary>
        /// 样式列表。
        /// </summary>
        [HtmlAttributeName(ValuesDictionaryName, DictionaryAttributePrefix = ValuesPrefix)]
        public IDictionary<string, object> Attributes
        {
            get => _attributes ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            set => _attributes = value;
        }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Attributes.Count == 0)
                return;
            foreach (var attributeName in Attributes.Keys)
            {
                var value = Attributes[attributeName];
                switch (attributeName)
                {
                    case "readonly":
                    case "checked":
                        {
                            if (value is bool bValue && bValue)
                                output.SetAttribute(attributeName, attributeName);
                        }
                        break;
                    case "disabled":
                        {
                            if (value is bool bValue && bValue)
                            {
                                output.SetAttribute(attributeName, attributeName);
                                output.AddClass("disabled");
                            }
                        }
                        break;
                    default:
                        {
                            if (value is bool bValue && bValue)
                                output.SetAttribute(attributeName, bValue.ToLowerString());
                            else if (value is Enum eValue)
                                output.SetAttribute(attributeName, eValue.ToString("d"));
                            else
                            {
                                var attributeValue = value?.ToString();
                                if (attributeValue != null)
                                    output.SetAttribute(attributeName, attributeValue);
                            }
                        }
                        break;
                }
            }
        }
    }
}