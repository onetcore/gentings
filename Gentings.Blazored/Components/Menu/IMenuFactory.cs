using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Gentings.Blazored.Components.Menu
{
    /// <summary>
    /// 菜单工厂类。
    /// </summary>
    public interface IMenuFactory
    {
        /// <summary>
        /// 加载当前名称的菜单列表。
        /// </summary>
        /// <param name="name">菜单提供者名称。</param>
        /// <returns>返回菜单实例对象。</returns>
        MenuItem Load(string name = null);
    }

    /// <summary>
    /// 菜单工厂实现类。
    /// </summary>
    public class MenuFactory : IMenuFactory
    {
        private readonly ConcurrentDictionary<string, MenuItem> _menus = new ConcurrentDictionary<string, MenuItem>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 初始化类<see cref="MenuFactory"/>。
        /// </summary>
        /// <param name="menus">菜单列表。</param>
        public MenuFactory(IEnumerable<IMenuProvider> menus)
        {
            foreach (var groupMenus in menus.GroupBy(x => x.Name))
            {
                if (!_menus.TryGetValue(groupMenus.Key, out var menu))
                {
                    menu = new MenuItem(groupMenus.Key) {IsRoot = true};
                    _menus.TryAdd(groupMenus.Key, menu);
                }

                var currentMenus = groupMenus.OrderBy(x => x.Priority).ToList();
                foreach (var currentMenu in currentMenus)
                {
                    currentMenu.Init(menu);
                }
            }
        }

        /// <summary>
        /// 加载当前名称的菜单列表。
        /// </summary>
        /// <param name="name">菜单提供者名称。</param>
        /// <returns>返回菜单实例对象。</returns>
        public MenuItem Load(string name = null)
        {
            _menus.TryGetValue(name ?? MenuProvider.DefaultName, out var menu);
            return menu;
        }
    }
}