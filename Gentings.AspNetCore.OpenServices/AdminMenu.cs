using Gentings.AspNetCore.Menus;
using Gentings.Extensions.OpenServices;

namespace Gentings.AspNetCore.OpenServices
{
    /// <summary>
    /// 管理菜单。
    /// </summary>
    public class AdminMenu : MenuProvider
    {
        /// <summary>
        /// 初始化菜单实例。
        /// </summary>
        /// <param name="root">根目录菜单。</param>
        public override void Init(MenuItem root)
        {
            root.AddMenu("open", item => item.Texted("开放平台", "bi-app-indicator").Page("/Backend/Index", area: OpenServiceSettings.ExtensionName).Allow(Permissions.View)
                .AddMenu("apps", it => it.Texted("应用程序列表").Page("/Backend/Index", area: OpenServiceSettings.ExtensionName))
                .AddMenu("services", it => it.Texted("开放服务列表").Page("/Admin/Services/Index", area: OpenServiceSettings.ExtensionName))
            );
        }
    }
}