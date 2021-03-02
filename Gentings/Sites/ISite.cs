namespace Gentings.Sites
{
    /// <summary>
    /// 支持SaaS的接口。
    /// </summary>
    public interface ISite
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        int SiteId { get; set; }
    }
}