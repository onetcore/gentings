namespace Gentings.Data
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
            if (this is IOrderBy order && order.Order != null)
                context.OrderBy<TModel>(order.Order.ToString(), order.Desc);
        }

        private int _pageIndex;
        /// <summary>
        /// 页码。
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (_pageIndex < 0)
                    _pageIndex = 1;
                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
            }
        }

        /// <summary>
        /// 每页显示记录数。
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}