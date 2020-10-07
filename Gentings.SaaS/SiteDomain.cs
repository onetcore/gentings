using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions;

namespace Gentings.SaaS
{
    /// <summary>
    /// 网站。
    /// </summary>
    [Table("saas_Sites_Domains")]
    public class SiteDomain
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// 域名。
        /// </summary>
        [Key]
        [Size(64)]
        public string Domain { get; set; }
    }
}