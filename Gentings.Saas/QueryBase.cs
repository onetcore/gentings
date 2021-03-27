using Gentings.Data;

namespace Gentings.Saas
{
    /// <summary>
    /// 查询基类。
    /// </summary>
    /// <typeparam name="TModel">数据库实体类型。</typeparam>
    public abstract class QueryBase<TModel> : Data.QueryBase<TModel>
        where TModel : ISite
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<TModel> context)
        {
            context.WithNolock().Where(x => x.SiteId == SiteId);
            base.Init(context);
        }
    }
}