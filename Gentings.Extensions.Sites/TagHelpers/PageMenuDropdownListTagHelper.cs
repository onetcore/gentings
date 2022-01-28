using Gentings.AspNetCore.TagHelpers;
using Gentings.Extensions.Sites.Menus;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.Extensions.Sites.TagHelpers
{
    /// <summary>
    /// 菜单下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:page-menu-dropdownlist")]
    public class PageMenuDropdownListTagHelper : DropdownListTagHelper
    {
        private readonly IPageMenuManager _menuManager;
        private readonly IMenuCategoryManager _categoryManager;

        /// <summary>
        /// 初始化类<see cref="PageMenuDropdownListTagHelper"/>。
        /// </summary>
        /// <param name="menuManager">菜单分类管理接口。</param>
        /// <param name="categoryManager">分类管理。</param>
        public PageMenuDropdownListTagHelper(IPageMenuManager menuManager, IMenuCategoryManager categoryManager)
        {
            _menuManager = menuManager;
            _categoryManager = categoryManager;
        }

        /// <summary>
        /// 排除菜单Id。
        /// </summary>
        [HtmlAttributeName("exclude")]
        public int ExcludeId { get; set; }

        /// <summary>
        /// 分类Id。
        /// </summary>
        [HtmlAttributeName("cid")]
        public int CategoryId { get; set; }

        /// <summary>
        /// 分类名称。
        /// </summary>
        [HtmlAttributeName("cname")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 初始化选项列表。
        /// </summary>
        /// <returns>返回选项列表。</returns>
        protected override async Task<IEnumerable<SelectListItem>> InitAsync()
        {
            if (CategoryId == 0 && !string.IsNullOrEmpty(CategoryName))
            {
                var category = await _categoryManager.FindAsync(x => x.Name.Equals(CategoryName, StringComparison.OrdinalIgnoreCase));
                if (category != null)
                    CategoryId = category.Id;
            }
            var menus = CategoryId > 0 ?
                await _menuManager.FetchAsync(x => x.CategoryId == CategoryId) :
                await _menuManager.FetchAsync();
            var items = new List<SelectListItem>();
            if (ExcludeId > 0)
                InitChildren(items, menus, xx => xx.Where(menu => menu.Id != ExcludeId).OrderBy(x => x.Order), getText: x => x.DisplayName!);
            else
                InitChildren(items, menus, xx => xx.OrderBy(x => x.Order), getText: x => x.DisplayName!);
            return items;
        }
    }
}