using Gentings.Extensions;
using System;

namespace Gentings.Saas
{
    /// <summary>
    /// 查询基类。
    /// </summary>
    /// <typeparam name="TModel">数据库实体类型。</typeparam>
    public abstract class QueryBase<TModel> : Extensions.QueryBase<TModel>
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

    /// <summary>
    /// 排序规则，注意<typeparamref name="TEnum"/>枚举的默认值为默认排序列。
    /// </summary>
    /// <typeparam name="TModel">数据库实体类型。</typeparam>
    /// <typeparam name="TEnum">排序枚举类型。</typeparam>
    public abstract class OrderableQueryBase<TModel, TEnum> : QueryBase<TModel>, IOrderBy
        where TModel : ISite
        where TEnum : Enum
    {
        /// <summary>
        /// 是否降序。
        /// </summary>
        public bool? Desc { get; set; }

        /// <summary>
        /// 排序列枚举。
        /// </summary>
        public TEnum Order { get; set; }
        Enum IOrderBy.Order => Order;

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<TModel> context)
        {
            base.Init(context);
            context.OrderBy<TModel>(Order.ToString(), Desc ?? IsDesc);
        }

        /// <summary>
        /// 未设置排序规则时候的默认排序，默认值为降序：true。
        /// </summary>
        protected virtual bool IsDesc => true;
    }
}