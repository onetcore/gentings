using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Gentings.Blazored.Components.Menu;
using Microsoft.AspNetCore.Components;

namespace Gentings.Blazored.Components
{
    /// <summary>
    /// MenuView组件。
    /// </summary>
    public partial class MenuView
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// 菜单提供者名称。
        /// </summary>
        [Parameter]
        public string Name { get; set; } = MenuProvider.DefaultName;

        /// <summary>
        /// 网站名称。
        /// </summary>
        [Parameter]
        public string SiteName { get; set; }

        /// <summary>
        /// 获取当前选中菜单。
        /// </summary>
        public MenuItem Current { get; set; }

        /// <summary>
        /// 菜单列表。
        /// </summary>
        public IEnumerable<MenuItem> Items { get; set; }

        [Inject]
        private IMenuFactory Factory { get; set; }
        
        /// <summary>
        /// 初始化菜单数据。
        /// </summary>
        protected override void OnInitialized()
        {
            var root = Factory.Load(Name);
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).AbsolutePath;
            Current = root.Find(uri);
            Items = root.OrderByDescending(x => x.Priority).ToList();
            Current ??= Items.First().First;
        }

        /// <summary>
        /// 激活当前菜单。
        /// </summary>
        /// <param name="item">当前菜单项。</param>
        public async Task ActivedMenuAsync(MenuItem item)
        {
            NavigationManager.NavigateTo(item.LinkUrl);
            Current = item;
            if (!string.IsNullOrEmpty(SiteName))
                await JSRuntime.SetTitleAsync($"{item.Name} - {SiteName}");
            StateHasChanged();
        }

        /// <summary>
        /// 判断当前菜单是否激活。
        /// </summary>
        /// <param name="item">当前菜单项。</param>
        /// <returns>返回判断结果。</returns>
        public bool IsActived(MenuItem item)
        {
            var current = Current;
            while (!current.IsRoot)
            {
                if (current.UniqueId == item.UniqueId)
                    return true;
                current = current.Parent;
            }

            return false;
        }
    }
}

