using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gentings.AspNetCore.Bootstraps;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Gentings.AspNetCore.AdminMenus
{
    /// <summary>
    /// 菜单项。
    /// </summary>
    public class MenuItem : IEnumerable<MenuItem>
    {
        internal MenuItem()
        {
        }

        /// <summary>
        /// 初始化类<see cref="MenuItem"/>。
        /// </summary>
        /// <param name="name">唯一名称，需要保证同一级下的名称唯一。</param>
        /// <param name="parent">父级菜单。</param>
        public MenuItem(string name, MenuItem? parent = null)
        {
            name = name.ToLower();
            Parent = parent ?? new MenuItem();
            if (parent?.Name == null)
                Name = name;
            else
                Name = $"{parent.Name}.{name}";
            Level = Parent.Level + 1;
        }

        /// <summary>
        /// 菜单项在父级下的唯一名称。
        /// </summary>
        public string? Name { get; private set; }

        /// <summary>
        /// 层级。
        /// </summary>
        public int Level { get; private set; } = -1;

        /// <summary>
        /// 排序。
        /// </summary>
        public int Priority { get; private set; }

        private string? _controller;
        private string? _action;
        private string? _page;
        private string? _pageHandler;
        private RouteValueDictionary? _routeValues;
        private string? _href;

        private readonly IDictionary<string, MenuItem> _children =
            new Dictionary<string, MenuItem>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns>
        /// 可用于循环访问集合的 <see cref="T:System.Collections.Generic.IEnumerator`1"/>。
        /// </returns>
        public IEnumerator<MenuItem> GetEnumerator()
        {
            return _children.Values.OrderByDescending(x => x.Priority).GetEnumerator();
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns>
        /// 可用于循环访问集合的 <see cref="T:System.Collections.IEnumerator"/> 对象。
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 父级菜单项。
        /// </summary>
        public MenuItem? Parent { get; private set; }

        /// <summary>
        /// 只添加标注字符串。
        /// </summary>
        /// <param name="name">唯一名称。</param>
        /// <param name="title">标题。</param>
        /// <param name="priority">优先级。</param>
        /// <returns>返回当前菜单选项。</returns>
        public MenuItem AddMenu(string name, string title, int priority = 0)
        {
            return AddMenu(name, item =>
            {
                item.Texted(title, priority: priority);
                item.IsTitle = true;
            });
        }

        /// <summary>
        /// 是否为标题。
        /// </summary>
        public bool IsTitle { get; private set; }

        /// <summary>
        /// 添加子菜单。
        /// </summary>
        /// <param name="name">唯一名称。</param>
        /// <param name="action">菜单实例化代理方法。</param>
        /// <returns>返回当前项目实例。</returns>
        public MenuItem AddMenu(string name, Action<MenuItem> action)
        {
            var menu = new MenuItem(name, this);
            action(menu);
            _children.Add(menu.Name!, menu);
            return this;
        }

        /// <summary>
        /// 添加子菜单。
        /// </summary>
        /// <param name="action">菜单实例化代理方法。</param>
        /// <returns>返回当前项目实例。</returns>
        public MenuItem AddMenu(Action<MenuItem> action)
        {
            var menu = new MenuItem();
            menu.Parent = this;
            menu.Level = Level + 1;
            action(menu);
            if (menu.Name == null)
                throw new Exception("菜单唯一名称不能为空！");
            if (Name != null)
                menu.Name = $"{Name}.{menu.Name}";
            _children.Add(menu.Name, menu);
            return this;
        }

        /// <summary>
        /// 标记颜色。
        /// </summary>
        public string? BadgeClassName { get; private set; }

        /// <summary>
        /// 标记字符串。
        /// </summary>
        public string? BadgeText { get; private set; }

        /// <summary>
        /// 设置标记示例。
        /// </summary>
        /// <param name="text">标记显示字符串。</param>
        /// <param name="className">样式类型名称。</param>
        /// <returns>返回当前项目实例。</returns>
        public MenuItem Badged(string text, string? className = null)
        {
            BadgeText = text;
            BadgeClassName = className;
            return this;
        }

        /// <summary>
        /// 图标名称，一般为awesome标签fa-后面的部分。
        /// </summary>
        public string? IconName { get; private set; }

        /// <summary>
        /// 链接地址。
        /// </summary>
        public string? LinkUrl(IUrlHelper urlHelper, string? defaultUrl = "#")
        {
            if (_href != null) return _href;
            if (_page != null)
                _href = urlHelper.Page(_page, _pageHandler, _routeValues);
            else if (_action != null)
                _href = urlHelper.Action(_action, _controller, _routeValues);
            else
                _href = defaultUrl;
            return _href;
        }

        /// <summary>
        /// 添加子菜单链接地址。
        /// </summary>
        /// <param name="href">链接地址。</param>
        /// <returns>返回当前菜单实例。</returns>
        public MenuItem Href(string href)
        {
            _href = href?.TrimStart('~');
            return this;
        }

        /// <summary>
        /// 添加子菜单链接地址。
        /// </summary>
        /// <param name="page">页面。</param>
        /// <param name="pageHandler">处理方法。</param>
        /// <param name="area">区域。</param>
        /// <param name="routeValues">路由实例。</param>
        /// <returns>返回当前菜单实例。</returns>
        public MenuItem Page(string page, string? pageHandler = null, string area = "", object? routeValues = null)
        {
            _page = page;
            _pageHandler = pageHandler;
            var routes = routeValues == null ? new RouteValueDictionary() : new RouteValueDictionary(routeValues);
            routes["Area"] = area;
            if (!routes.ContainsKey("id"))
                routes["id"] = null;
            _routeValues = routes;
            return this;
        }

        /// <summary>
        /// 添加子菜单链接地址。
        /// </summary>
        /// <param name="action">试图。</param>
        /// <param name="controller">控制器名称。</param>
        /// <param name="area">区域。</param>
        /// <param name="routeValues">路由实例。</param>
        /// <returns>返回当前菜单实例。</returns>
        public MenuItem Action(string action, string controller, string area = "", object? routeValues = null)
        {
            _controller = controller;
            _action = action;
            var routes = routeValues == null ? new RouteValueDictionary() : new RouteValueDictionary(routeValues);
            routes["Area"] = area;
            if (!routes.ContainsKey("id"))
                routes["id"] = null;
            _routeValues = routes;
            return this;
        }

        /// <summary>
        /// 设置内容以及链接地址。
        /// </summary>
        /// <param name="text">字符串。</param>
        /// <param name="iconName">图标样式名称。</param>
        /// <param name="priority">优先级，越大越靠前。</param>
        /// <returns>返回当前实例。</returns>
        public MenuItem Texted(string text, string? iconName = null, int priority = 0)
        {
            Priority = priority;
            Text = text;
            IconName = iconName;
            return this;
        }

        /// <summary>
        /// 设置内容以及链接地址。
        /// </summary>
        /// <param name="text">字符串。</param>
        /// <param name="iconName">图标类型。</param>
        /// <param name="priority">优先级，越大越靠前。</param>
        /// <returns>返回当前实例。</returns>
        public MenuItem Texted(string text, IconType iconName, int priority = 0)
        {
            Priority = priority;
            Text = text;
            IconName = iconName.ToDescriptionString();
            return this;
        }

        /// <summary>
        /// 显示文本字符串。
        /// </summary>
        public string? Text { get; private set; }

        internal void Merge(MenuItem item)
        {
            Text ??= item.Text;
            IconName ??= item.IconName;
            BadgeClassName ??= item.BadgeClassName;
            BadgeText ??= item.BadgeText;
            PermissionName ??= item.PermissionName;
            Priority = Math.Max(Priority, item.Priority);
            Level = Math.Max(Level, item.Level);
            if (Parent?.Name == null)
                Parent = item.Parent;
            _href ??= item._href;
            _controller ??= item._controller;
            _action ??= item._action;
            _routeValues ??= item._routeValues;

            foreach (var it in item)
            {
                if (_children.TryGetValue(it.Name!, out var i))
                    i.Merge(it);
                else
                {
                    it.Parent = this;
                    _children.Add(it.Name!, it);
                }
            }
        }

        /// <summary>
        /// 依赖权限。
        /// </summary>
        /// <param name="permissionName">权限名称。</param>
        /// <returns>返回当前实例。</returns>
        public MenuItem Allow(string? permissionName = null)
        {
            PermissionName = permissionName ?? Name;
            return this;
        }

        /// <summary>
        /// 权限名称。
        /// </summary>
        public string? PermissionName { get; private set; }

        private readonly IDictionary<string, object> _extensions =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 获取或设置扩展属性实例。
        /// </summary>
        /// <param name="name">扩展属性名称。</param>
        /// <returns>返回扩展属性值。</returns>
        public object? this[string name]
        {
            get
            {
                if (_extensions.TryGetValue(name, out var value))
                    return value;
                return null;
            }
            set
            {
                if (value == null)
                    _extensions.Remove(name);
                else
                    _extensions[name] = value;
            }
        }

        /// <summary>
        /// 设置扩展属性。
        /// </summary>
        /// <param name="name">扩展属性名称。</param>
        /// <param name="value">扩展属性值。</param>
        public void Set(string name, object value) => this[name] = value;

        /// <summary>
        /// 获取当前扩展属性值。
        /// </summary>
        /// <param name="name">扩展属性名称。</param>
        /// <returns>返回当前扩展属性值。</returns>
        public string? Get(string name) => this[name]?.ToString();

        /// <summary>
        /// 获取当前扩展属性值。
        /// </summary>
        /// <param name="name">扩展属性名称。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>返回当前扩展属性值。</returns>
        public TValue? Get<TValue>(string name, TValue? defaultValue = default)
        {
            var value = this[name];
            if (value == null)
                return defaultValue;
            return (TValue)value;
        }
    }
}