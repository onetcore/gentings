using Gentings.Data;
using Gentings.Extensions.Groups;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Extensions.Sites.Menus
{
    /// <summary>
    /// 页面菜单管理接口。
    /// </summary>
    public interface IPageMenuManager : IGroupManager<PageMenu>, ISingletonService
    {
        /// <summary>
        /// 上移一个位置。
        /// </summary>
        /// <param name="id">当前页面Id。</param>
        /// <returns>返回移动结果。</returns>
        Task<bool> MoveUpAsync(int id);

        /// <summary>
        /// 下移一个位置。
        /// </summary>
        /// <param name="id">当前页面Id。</param>
        /// <returns>返回移动结果。</returns>
        Task<bool> MoveDownAsync(int id);
    }

    /// <summary>
    /// 页面菜单管理实现类。
    /// </summary>
    public class PageMenuManager : GroupManager<PageMenu>, IPageMenuManager
    {
        private readonly IMenuCategoryManager _categoryManager;

        /// <summary>
        /// 初始化类<see cref="PageMenuManager"/>。
        /// </summary>
        /// <param name="context">数据库操作接口实例。</param>
        /// <param name="cache">缓存接口。</param>
        /// <param name="categoryManager">分类管理接口。</param>
        public PageMenuManager(IDbContext<PageMenu> context, IMemoryCache cache, IMenuCategoryManager categoryManager) : base(context, cache)
        {
            _categoryManager = categoryManager;
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回添加结果。</returns>
        public override async Task<bool> CreateAsync(PageMenu model, CancellationToken cancellationToken = default)
        {
            if (model.Order == 0)
                model.Order = 1 + await Context.MaxAsync(x => x.Order, x => x.ParentId == model.ParentId, cancellationToken);
            return await base.CreateAsync(model, cancellationToken);
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <returns>返回添加结果。</returns>
        public override bool Create(PageMenu model)
        {
            if (model.Order == 0)
                model.Order = 1 + Context.Max(x => x.Order, x => x.ParentId == model.ParentId);
            return base.Create(model);
        }

        /// <summary>
        /// 上移一个位置。
        /// </summary>
        /// <param name="id">当前页面Id。</param>
        /// <returns>返回移动结果。</returns>
        public virtual async Task<bool> MoveUpAsync(int id)
        {
            var menu = await Context.FindAsync(x => x.Id == id);
            if (menu == null)
                return false;
            if (await Context.MoveUpAsync(id, x => x.Order, x => x.ParentId == menu.ParentId, false))
            {
                Refresh();
                return true;
            }

            return false;
        }

        /// <summary>
        /// 下移一个位置。
        /// </summary>
        /// <param name="id">当前页面Id。</param>
        /// <returns>返回移动结果。</returns>
        public virtual async Task<bool> MoveDownAsync(int id)
        {
            var menu = await Context.FindAsync(x => x.Id == id);
            if (menu == null)
                return false;
            if (await Context.MoveDownAsync(id, x => x.Order, x => x.ParentId == menu.ParentId, false))
            {
                Refresh();
                return true;
            }

            return false;
        }
    }
}