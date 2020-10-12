using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions;

namespace Gentings.SaaS.Security.Roles
{
    /// <summary>
    /// 角色基类。
    /// </summary>
    [Table("saas_Roles")]
    public abstract class RoleBase : Identity.Roles.RoleBase, ISite
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        [NotUpdated]
        public int SiteId { get; set; }
    }
}