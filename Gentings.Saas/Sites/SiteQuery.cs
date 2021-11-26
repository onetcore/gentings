namespace Gentings.Saas.Sites
{
    /// <summary>
    /// 网站查询实例。
    /// </summary>
    public class SiteQuery : Data.QueryBase<SiteAdapter>
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 禁用状态。
        /// </summary>
        public bool? Disabled { get; set; }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected override void Init(IQueryContext<SiteAdapter> context)
        {
            context.WithNolock();
            if (!string.IsNullOrWhiteSpace(Name))
                context.Where(x => x.SiteName.Contains(Name) || x.SiteKey.Contains(Name));
            if (Disabled != null)
                context.Where(x => x.Disabled == Disabled);
        }
    }
}