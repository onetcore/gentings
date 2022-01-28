using Gentings.Data;
using Gentings.Extensions.Categories;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Extensions.Sites.Menus
{
    /// <summary>
    /// 菜单分类管理实现类。
    /// </summary>
    public class MenuCategoryManager : CachableCategoryManager<MenuCategory>, IMenuCategoryManager
    {
        /// <summary>
        /// 初始化类<see cref="MenuCategoryManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="cache">缓存接口。</param>
        public MenuCategoryManager(IDbContext<MenuCategory> context, IMemoryCache cache) : base(context, cache)
        {

        }
    }
}