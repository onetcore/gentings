﻿namespace Gentings.Extensions
{
    /// <summary>
    /// 查询基类。
    /// </summary>
    /// <typeparam name="TModel">数据库实体类型。</typeparam>
    public abstract class QueryBase<TModel>
    {
        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected internal virtual void Init(IQueryContext<TModel> context)
        {
            context.WithNolock();
        }

        /// <summary>
        /// 页码，缩写。
        /// </summary>
        public int PI { get; set; }

        /// <summary>
        /// 页码。
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (PI <= 0)
                    PI = 1;
                return PI;
            }
            set
            {
                PI = value;
            }
        }

        /// <summary>
        /// 每页显示记录数，缩写。
        /// </summary>
        public int PS { get; set; } = 20;

        /// <summary>
        /// 每页显示记录数。
        /// </summary>
        public int PageSize { get => PS; set => PS = value; }
    }

    /// <summary>
    /// 排序规则，注意<typeparamref name="TEnum"/>枚举的默认值为默认排序列。
    /// </summary>
    /// <typeparam name="TModel">数据库实体类型。</typeparam>
    /// <typeparam name="TEnum">排序枚举类型。</typeparam>
    public abstract class OrderableQueryBase<TModel, TEnum> : QueryBase<TModel>, IOrderBy
        where TEnum : struct, Enum
    {
        /// <summary>
        /// 是否降序。
        /// </summary>
        public bool? Desc { get; set; }

        /// <summary>
        /// 排序列枚举。
        /// </summary>
        public TEnum? Order { get; set; }
        Enum? IOrderBy.Order => Order;

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected internal override void Init(IQueryContext<TModel> context)
        {
            base.Init(context);
            if (Order != null)
                context.OrderBy<TModel>(Order.ToString()!, Desc ?? true);
        }
    }
}