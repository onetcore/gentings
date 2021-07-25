using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gentings.Blazored.Components.Menu
{
    /// <summary>
    /// 菜单项目。
    /// </summary>
    public class MenuItem : IEnumerable<MenuItem>
    {
        /// <summary>
        /// 获取当前层级唯一Id。
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 全局唯一Id。
        /// </summary>
        public string UniqueId { get; private set; }

        private readonly IDictionary<string, MenuItem> _children = new Dictionary<string, MenuItem>(StringComparer.OrdinalIgnoreCase);
        internal MenuItem(string id)
        {
            Id = id;
        }

        /// <summary>
        /// 父级菜单。
        /// </summary>
        public MenuItem Parent { get; private set; }

        /// <summary>
        /// 添加子菜单。
        /// </summary>
        /// <param name="id">菜单Id。</param>
        /// <param name="init">初始化菜单实例。</param>
        /// <returns>返回父级菜单实例。</returns>
        public MenuItem AddMenu(string id, Action<MenuItem> init)
        {
            id = id.Replace(" ", "");
            if (!_children.TryGetValue(id, out var item))
            {
                item = new MenuItem(id);
                _children[id] = item;
                item.UniqueId = $"{UniqueId}_{id}";
                item.Parent = this;
                item.Level = Level + 1;
            }
            init?.Invoke(item);
            return this;
        }

        /// <summary>
        /// 获取菜单迭代实例。
        /// </summary>
        /// <returns>返回菜单项目迭代器。</returns>
        public IEnumerator<MenuItem> GetEnumerator()
        {
            return _children.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 显示名称。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 图标样式。
        /// </summary>
        public string IconName { get; private set; }

        private readonly IDictionary<string, object> _markeds = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 注释选项。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <returns>返回当前菜单实例。</returns>
        public MenuItem Marked(string key, object value = null)
        {
            _markeds[key] = value;
            return this;
        }

        /// <summary>
        /// 是否有注释。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>返回判断结果。</returns>
        public bool IsMarked(string key) => _markeds.ContainsKey(key);

        /// <summary>
        /// 尝试获取注释实例。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <returns>返回是否有获取结果。</returns>
        public bool TryGetMarked(string key, out object value) => _markeds.TryGetValue(key, out value);

        /// <summary>
        /// 获取注释值。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>返回当前键值。</returns>
        protected string GetMarked(string key)
        {
            _markeds.TryGetValue(key, out var value);
            return value?.ToString();
        }

        /// <summary>
        /// 显示设置。
        /// </summary>
        /// <param name="name">显示名称。</param>
        /// <param name="iconName">图标样式。</param>
        /// <returns>返回当前实例。</returns>
        public MenuItem Display(string name, string iconName = null)
        {
            Name = name;
            IconName = iconName;
            return this;
        }

        /// <summary>
        /// 设置附加标记。
        /// </summary>
        /// <param name="value">值。</param>
        /// <returns>返回当前实例。</returns>
        public MenuItem Badged(object value) => Marked("badge", value);

        /// <summary>
        /// 添加链接地址。
        /// </summary>
        /// <param name="href">链接地址。</param>
        /// <returns>返回当前实例。</returns>
        public MenuItem Href(string href) => Marked("href", href);

        /// <summary>
        /// 链接地址。
        /// </summary>
        public string LinkUrl => GetMarked("href");

        /// <summary>
        /// 层级。
        /// </summary>
        public int Level { get; internal set; }

        /// <summary>
        /// 优先级。
        /// </summary>
        internal int Priority { get; set; }

        /// <summary>
        /// 是否为顶级菜单项目。
        /// </summary>
        public bool IsRoot { get; internal set; }

        /// <summary>
        /// 通过链接地址查询菜单。
        /// </summary>
        /// <param name="linkUrl">链接地址。</param>
        /// <returns>返回当前菜单项目。</returns>
        public MenuItem Find(string linkUrl)
        {
            if (linkUrl.Equals(LinkUrl, StringComparison.OrdinalIgnoreCase))
                return this;
            foreach (var item in _children.Values)
            {
                var current = item.Find(linkUrl);
                if (current != null)
                    return current;
            }

            return null;
        }

        /// <summary>
        /// 获取第一个可导航的菜单项。
        /// </summary>
        public MenuItem First => FindFirst(this);

        private MenuItem FindFirst(MenuItem item)
        {
            if (item.Any()) return FindFirst(item.First());
            return item;
        }
    }
}