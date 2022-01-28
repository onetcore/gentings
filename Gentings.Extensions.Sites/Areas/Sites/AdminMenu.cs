using Gentings.AspNetCore.Menus;
using Gentings.Security;

namespace Gentings.Extensions.Sites.Areas.Sites
{
    /// <summary>
    /// 管理菜单。
    /// </summary>
    public class AdminMenu : MenuProvider
    {
        /// <summary>
        /// 扩展名称。
        /// </summary>
        public const string ExtensionName = "sites";

        /// <summary>
        /// 初始化菜单实例。
        /// </summary>
        /// <param name="root">根目录菜单。</param>
        public override void Init(MenuItem root)
        {
            root.AddMenu("pages", menu => menu.Texted("网页管理", "bi-columns").Page("/Backend/Pages/Index", area: ExtensionName).Allow(CorePermissions.Administrator)
                .AddMenu("banners", item => item.Texted("广告管理").Page("/Backend/Banners/Index", area: ExtensionName))
                .AddMenu("menus", item => item.Texted("菜单管理").Page("/Backend/Menus/Index", area: ExtensionName))
                .AddMenu("index", item => item.Texted("页面管理").Page("/Backend/Pages/Index", area: ExtensionName).Allow(SitePermissions.ViewPages))
                .AddMenu("templates", item => item.Texted("模板列表").Page("/Backend/Templates/Index", area: ExtensionName))
            );
        }
    }
}