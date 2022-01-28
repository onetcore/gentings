using Gentings.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.Extensions.Sites.TagHelpers
{
    /// <summary>
    /// 页面链接下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:page-link-dropdownlist")]
    public class PageLinkDropdownListTagHelper : DropdownListTagHelper
    {
        private readonly IPageManager _pageManager;
        /// <summary>
        /// 初始化类<see cref="PageLinkDropdownListTagHelper"/>。
        /// </summary>
        /// <param name="pageManager">菜单分类管理接口。</param>
        public PageLinkDropdownListTagHelper(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        /// <summary>
        /// 初始化选项列表。
        /// </summary>
        /// <returns>返回选项列表。</returns>
        protected override async Task<IEnumerable<SelectListItem>> InitAsync()
        {
            var items = await _pageManager.AsQueryable().WithNolock()
                .Select(x => new { x.Title, x.Key })
                .AsEnumerableAsync(reader => new SelectListItem(reader.GetString(0), reader.GetString(1).ToString()));
            foreach (var item in items)
            {
                if (item.Value != "/")
                    item.Value = $"/pages/{item.Value.TrimEnd('/')}";
            }
            return items;
        }
    }
}