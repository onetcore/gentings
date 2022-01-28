using Gentings.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.Extensions.Sites.TagHelpers
{
    /// <summary>
    /// 节点类型下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:section-dropdownlist")]
    public class SectionDropdownListTagHelper : DropdownListTagHelper
    {
        private readonly ISectionManager _sectionManager;

        /// <summary>
        /// 初始化类<see cref="SectionDropdownListTagHelper"/>。
        /// </summary>
        /// <param name="sectionManager">节点管理接口。</param>
        public SectionDropdownListTagHelper(ISectionManager sectionManager)
        {
            _sectionManager = sectionManager;
        }

        /// <summary>
        /// 初始化选项列表。
        /// </summary>
        /// <returns>返回选项列表。</returns>
        protected override IEnumerable<SelectListItem> Init()
        {
            var items = new List<SelectListItem>();
            foreach (var section in _sectionManager.SectionTypes)
            {
                items.Add(new SelectListItem(section.DisplayName ?? section.Name, section.Name));
            }

            return items;
        }
    }
}