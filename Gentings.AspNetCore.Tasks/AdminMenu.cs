﻿using Gentings.AspNetCore.Menus;
using Gentings.Security;

namespace Gentings.AspNetCore.Tasks
{
    /// <summary>
    /// 后台菜单提供者。
    /// </summary>
    public class AdminMenu : MenuProvider
    {
        /// <summary>
        /// 区域名称。
        /// </summary>
        public const string AreaName = "Tasks";

        /// <summary>
        /// 初始化菜单实例。
        /// </summary>
        /// <param name="root">根目录菜单。</param>
        public override void Init(MenuItem root)
        {
            root.AddMenu("tasks", menu => menu.Texted("TasksManager", IconType.Terminal, -1).Page("/backend/Index", area: AreaName).Allow(CorePermissions.Developer)
                .AddMenu("index", item => item.Texted("Task_Title").Page("/backend/Index", area: AreaName).Allow(CorePermissions.Developer))
                .AddMenu("bgservice", item => item.Texted("BackgroundService_Title").Page("/backend/BackgroundService", area: AreaName).Allow(CorePermissions.Developer))
            );
        }
    }
}
