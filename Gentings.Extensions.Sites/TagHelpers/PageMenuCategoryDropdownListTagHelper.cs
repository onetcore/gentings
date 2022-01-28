using Gentings.AspNetCore.TagHelpers;
using Gentings.Extensions.Sites.Menus;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.Extensions.Sites.TagHelpers
{
    /// <summary>
    /// 菜单分类下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:page-menu-category-dropdownlist")]
    public class PageMenuCategoryDropdownListTagHelper : DropdownListTagHelper
    {
        private readonly IMenuCategoryManager _categoryManager;
        /// <summary>
        /// 初始化类<see cref="PageMenuCategoryDropdownListTagHelper"/>。
        /// </summary>
        /// <param name="categoryManager">菜单分类管理接口。</param>
        public PageMenuCategoryDropdownListTagHelper(IMenuCategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        /// <summary>
        /// 初始化选项列表。
        /// </summary>
        /// <returns>返回选项列表。</returns>
        protected override async Task<IEnumerable<SelectListItem>> InitAsync()
        {
            var categories = await _categoryManager.FetchAsync();
            var items = new List<SelectListItem>();
            foreach (var category in categories)
            {
                items.Add(new SelectListItem(category.DisplayName, category.Id.ToString()));
            }

            return items;
        }
    }
}