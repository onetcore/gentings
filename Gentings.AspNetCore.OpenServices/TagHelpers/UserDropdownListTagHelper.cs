using Gentings.AspNetCore.TagHelpers;
using Gentings.Extensions.OpenServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.OpenServices.TagHelpers
{
    /// <summary>
    /// 用户下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:user-dropdownlist")]
    public class UserDropdownListTagHelper : DropdownListTagHelper
    {
        private readonly IApplicationManager _applicationManager;
        /// <summary>
        /// 初始化类<see cref="UserDropdownListTagHelper"/>。
        /// </summary>
        /// <param name="applicationManager">应用程序管理实例。</param>
        public UserDropdownListTagHelper(IApplicationManager applicationManager)
        {
            _applicationManager = applicationManager;
        }

        /// <summary>
        /// 初始化选项列表。
        /// </summary>
        /// <returns>返回选项列表。</returns>
        protected override async Task<IEnumerable<SelectListItem>> InitAsync()
        {
            var users = await _applicationManager.LoadUsersAsync();
            return users.Select(x => new SelectListItem(x.Value, x.Key.ToString())).ToList();
        }
    }
}
