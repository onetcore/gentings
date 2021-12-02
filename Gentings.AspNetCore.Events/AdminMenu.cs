using Gentings.AspNetCore.Menus;

namespace Gentings.AspNetCore.Events
{
    /// <summary>
    /// 后台菜单提供者。
    /// </summary>
    public class AdminMenu : MenuProvider
    {
        /// <summary>
        /// 区域名称。
        /// </summary>
        public const string AreaName = "Events";

        /// <summary>
        /// 初始化菜单实例。
        /// </summary>
        /// <param name="root">根目录菜单。</param>
        public override void Init(MenuItem root)
        {
            root.AddMenu("events", menu => menu.Texted("事件日志管理", IconType.Alarm).Page("/backend/Index", area: AreaName)
                .AddMenu("index", item => item.Texted("日志列表").Page("/backend/Index", area: AreaName))
                .AddMenu("types", item => item.Texted("类型管理").Page("/backend/Categories/Index", area: AreaName))
            );
        }
    }
}
