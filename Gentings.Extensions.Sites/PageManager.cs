using Gentings.Data;
using Gentings.Extensions.Sites.Properties;

namespace Gentings.Extensions.Sites
{
    /// <summary>
    /// 页面类型管理接口。
    /// </summary>
    public interface IPageManager : IObjectManager<Page>, ISingletonService
    {
        /// <summary>
        /// 通过唯一键获取页面实例。
        /// </summary>
        /// <param name="key">唯一键。</param>
        /// <returns>返回页面实例。</returns>
        Task<Page> FindAsync(string key);
    }

    /// <summary>
    /// 页面类型管理实现类。
    /// </summary>
    public class PageManager : ObjectManager<Page>, IPageManager
    {
        /// <summary>
        /// 初始化类<see cref="Page"/>。
        /// </summary>
        /// <param name="context">数据库操作接口实例。</param>
        public PageManager(IDbContext<Page> context) : base(context)
        {
        }

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回判断结果。</returns>
        public override bool IsDuplicated(Page model)
        {
            return Context.Any(x => x.Key == model.Key && x.Id != model.Id);
        }

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回判断结果。</returns>
        public override Task<bool> IsDuplicatedAsync(Page model, CancellationToken cancellationToken = default)
        {
            return Context.AnyAsync(x => x.Key == model.Key && x.Id != model.Id, cancellationToken);
        }

        /// <summary>
        /// 通过唯一键获取页面实例。
        /// </summary>
        /// <param name="key">唯一键。</param>
        /// <returns>返回页面实例。</returns>
        public virtual async Task<Page> FindAsync(string key)
        {
            var page = await Context.FindAsync(x => x.Key == key);
            if(page == null && key == "/")//首页不存在，则自动创建一个
            {
                page = new Page()
                {
                    Key = key,
                    Title = Resources.HomePage
                };
                await CreateAsync(page);
            }
            return page;
        }
    }
}

