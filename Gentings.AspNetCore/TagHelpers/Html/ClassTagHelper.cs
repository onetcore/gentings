using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Html
{
    /// <summary>
    /// 样式标签。
    /// </summary>
    [HtmlTargetElement("*", Attributes = AttributeName)]
    [HtmlTargetElement("*", Attributes = ClassValuesPrefix + "*")]
    public class ClassTagHelper : TagHelperBase
    {
        private const string AttributeName = ".class";
        private const string ClassValuesPrefix = ".class-";
        private const string ClassValuesDictionaryName = ".class-data";

        /// <summary>
        /// 样式列表。
        /// </summary>
        [HtmlAttributeName(ClassValuesDictionaryName, DictionaryAttributePrefix = ClassValuesPrefix)]
        public IDictionary<string, bool?> ClassNames { get; set; } = new Dictionary<string, bool?>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 样式名称。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public object? ClassName { get; set; }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var classNames = ClassNames.Where(x => x.Value == true).Select(x => x.Key).ToList();
            var className = ClassName?.ToString()?.Trim();
            if (!string.IsNullOrEmpty(className))
                classNames.AddRange(className.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            output.AddCssClass(classNames);
        }
    }
}