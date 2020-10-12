using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;

namespace Gentings.AspNetCore.TagHelpers.Html
{
    /// <summary>
    /// 在开发模式下使用的属性。
    /// </summary>
    [HtmlTargetElement("*", Attributes = ".dev-*")]
    public class DevTagHelper : TagHelperBase
    {
        private readonly IHostEnvironment _hostEnvironment;
        private const string DevValuesPrefix = ".dev-";
        private const string DevValuesDictionaryName = ".dev-data";
        private IDictionary<string, string> _classNames;
        /// <summary>
        /// 初始化类<see cref="DevTagHelper"/>。
        /// </summary>
        /// <param name="hostEnvironment">主机环境变量。</param>
        public DevTagHelper(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// 样式列表。
        /// </summary>
        [HtmlAttributeName(DevValuesDictionaryName, DictionaryAttributePrefix = DevValuesPrefix)]
        public IDictionary<string, string> DevNames
        {
            get => _classNames ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            set => _classNames = value;
        }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (_hostEnvironment.IsDevelopment())
            {
                foreach (var devName in DevNames)
                {
                    var value = devName.Value;
                    if (value.StartsWith('~'))
                        value = value.Substring(1);
                    output.Attributes.SetAttribute(devName.Key, value);
                }
            }
        }
    }
}