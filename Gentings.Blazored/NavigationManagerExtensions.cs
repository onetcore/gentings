using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Components;

namespace Gentings.Blazored
{
    /// <summary>
    /// NavigationManager扩展类。
    /// </summary>
    public static class NavigationManagerExtensions
    {
        /// <summary>
        /// 跳转到ReturnUrl地址。
        /// </summary>
        /// <param name="navigationManager">导航管理实例对象。</param>
        /// <param name="defaultUrl">如果不存在返回地址，返回的默认值。</param>
        public static void NavigateToReturnUrl(this NavigationManager navigationManager, string defaultUrl = null)
        {
            var returnUrl = navigationManager.QueryString().Get("returnUrl");
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = defaultUrl ?? "";
            navigationManager.NavigateTo(returnUrl);
        }

        /// <summary>
        /// 获取当前导航的查询字符串。
        /// </summary>
        /// <param name="navigationManager">导航管理实例对象。</param>
        /// <returns>返回当前导航查询字符串实例。</returns>
        public static NameValueCollection QueryString(this NavigationManager navigationManager) =>
            HttpUtility.ParseQueryString(navigationManager.ToAbsoluteUri(navigationManager.Uri).Query);
    }
}