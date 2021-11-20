using Gentings.AspNetCore.AdminMenus;

namespace GS.Areas.Backend
{
    /// <summary>
    /// 后台菜单。
    /// </summary>
    public class AdminMenu : MenuProvider
    {
        /// <summary>
        /// 区域名称。
        /// </summary>
        public const string AreaName = "Backend";

        /// <summary>
        /// 初始化菜单实例。
        /// </summary>
        /// <param name="root">根目录菜单。</param>
        public override void Init(MenuItem root)
        {
            root.AddMenu("nav", "导航")
                .AddMenu("home", item => item.Texted("后台管理", "bi-house-door").Page("/Index", area: AreaName))
                .AddMenu("users", item => item.Texted("用户管理", "bi-people").Page("/Users/Index", area: AreaName)
                    .AddMenu("index", it => it.Texted("用户列表").Page("/Users/Index", area: AreaName))
                    .AddMenu("index1", it => it.Texted("用户列表1").Page("/Users/Index", area: AreaName))
                    .AddMenu("index2", it => it.Texted("用户列表2").Page("/Users/Index", area: AreaName))
                    .AddMenu("users", item => item.Texted("用户管理22").Page("/Users/Index", area: AreaName)
                        .AddMenu("index", it => it.Texted("用户列表22").Page("/Users/Index", area: AreaName))
                        .AddMenu("index1", it => it.Texted("用户列表22").Page("/Users/Index", area: AreaName))
                        .AddMenu("index2", it => it.Texted("用户列表22").Page("/Users/Index", area: AreaName))
                        .AddMenu("users", item => item.Texted("用户管理33").Page("/Users/Index", area: AreaName)
                            .AddMenu("index", it => it.Texted("用户列表33").Page("/Users/Index", area: AreaName))
                            .AddMenu("index1", it => it.Texted("用户列表33").Page("/Users/Index", area: AreaName))
                            .AddMenu("index2", it => it.Texted("用户列表33").Page("/Users/Index", area: AreaName))
                        )
                    )
                )
                .AddMenu("group", "网站分隔符")
                .AddMenu("settings", item => item.Texted("网站配置", "bi-gear").Page("/Settings", area: AreaName));
        }
    }
}
