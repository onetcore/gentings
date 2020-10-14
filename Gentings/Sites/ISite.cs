namespace Gentings.Sites
{
    /// <summary>
    /// 支持SaaS的接口。
    /// </summary>
    public interface ISite
    {
        /// <summary>
        /// 用户Id。
        /// </summary>
        int SiteId { get; set; }
    }
}