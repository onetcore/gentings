namespace Gentings.Extensions.Sites
{
    /// <summary>
    /// 页面查询实例。
    /// </summary>
    public class PageQuery : QueryBase<Page>
    {
        /// <summary>
        /// 标题或者名称。
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<Page> context)
        {
            base.Init(context);
            context.Exclude(x => x.ExtendProperties);
            if (!string.IsNullOrEmpty(Name))
                context.Where(x => x.Name!.Contains(Name) || x.Title!.Contains(Name));
        }
    }
}