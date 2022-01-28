using Gentings.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.Extensions.Sites.TagHelpers
{
    /// <summary>
    /// 页面Id下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:page-id-dropdownlist")]
    public class PageIdDropdownListTagHelper : DropdownListTagHelper
    {
        private readonly IPageManager _pageManager;
        /// <summary>
        /// 初始化类<see cref="PageIdDropdownListTagHelper"/>。
        /// </summary>
        /// <param name="pageManager">菜单分类管理接口。</param>
        public PageIdDropdownListTagHelper(IPageManager pageManager)
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
                .Select(x => new { x.Title, x.Id })
                .AsEnumerableAsync(reader => new SelectListItem(reader.GetString(0), reader.GetInt32(1).ToString()));
            return items;
        }
    }
}