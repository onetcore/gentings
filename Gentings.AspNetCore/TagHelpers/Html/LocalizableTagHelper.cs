using Gentings.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Html
{
    /// <summary>
    /// 标签属性或者内容资源本地化。
    /// </summary>
    [HtmlTargetElement("gt:local")]
    [HtmlTargetElement("*", Attributes = AttributeName)]
    [HtmlTargetElement("*", Attributes = ValuesPrefix + "*")]
    public class LocalizableTagHelper : ViewContextableTagHelperBase
    {
        private const string AttributeName = "resource";
        private const string ValuesPrefix = ".res-";
        private const string ValuesDictionaryName = ".res-data";
        private readonly IResourceManager _resourceManager;
        private IDictionary<string, string>? _attributes;
        /// <summary>
        /// UI资源本地化标签。
        /// </summary>
        /// <param name="resourceManager">资源管理接口。</param>
        public LocalizableTagHelper(IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        /// <summary>
        /// 样式列表。
        /// </summary>
        [HtmlAttributeName(ValuesDictionaryName, DictionaryAttributePrefix = ValuesPrefix)]
        public IDictionary<string, string> Attributes
        {
            get => _attributes ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            set => _attributes = value;
        }

        /// <summary>
        /// 判断子项内容是否需要资源化。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public bool IsChild { get; set; }

        private string GetResource(string key)
        {
            if (ViewContext.ActionDescriptor is CompiledPageActionDescriptor cpage)
                return _resourceManager.GetResource(cpage.ModelTypeInfo, key);
            if (ViewContext.ActionDescriptor is ControllerActionDescriptor controller)
                return _resourceManager.GetResource(controller.ControllerTypeInfo, key);
            return key;
        }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Attributes.Count > 0)
            {
                foreach (var attributeName in Attributes.Keys)
                {
                    var value = Attributes[attributeName];
                    if (string.IsNullOrWhiteSpace(value)) continue;
                    value = GetResource(value);
                    output.SetAttribute(attributeName, value);
                }
            }
            var local = output.TagName == "gt:local";
            if (IsChild || local)
            {
                if (local) output.TagName = null;
                var content = await output.GetChildContentAsync();
                if (content.IsEmptyOrWhiteSpace) return;
                output.Content.SetHtmlContent(GetResource(content.GetContent().Trim()));
            }
        }
    }
}
