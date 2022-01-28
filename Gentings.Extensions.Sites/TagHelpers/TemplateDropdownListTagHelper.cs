using Gentings.AspNetCore.TagHelpers;
using Gentings.Extensions.Sites.Templates;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.Extensions.Sites.TagHelpers
{
    /// <summary>
    /// 模板下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:template-dropdownlist")]
    public class TemplateDropdownListTagHelper : DropdownListTagHelper
    {
        private readonly ITemplateManager _templateManager;

        /// <summary>
        /// 初始化类<see cref="TemplateDropdownListTagHelper"/>。
        /// </summary>
        /// <param name="templateManager">模板管理接口。</param>
        public TemplateDropdownListTagHelper(ITemplateManager templateManager)
        {
            _templateManager = templateManager;
        }

        /// <summary>
        /// 初始化选项列表。
        /// </summary>
        /// <returns>返回选项列表。</returns>
        protected override IEnumerable<SelectListItem> Init()
        {
            foreach (var template in _templateManager.Templates)
            {
                yield return new SelectListItem(template.Name, template.Name);
            }
        }
    }
}