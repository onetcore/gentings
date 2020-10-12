using Gentings.Extensions;

namespace Gentings.SaaS.Security
{
    /// <summary>
    /// 用户基类。
    /// </summary>
    public abstract class UserBase : Identity.UserBase, ISite
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        [NotUpdated]
        public int SiteId { get; set; }
    }
}