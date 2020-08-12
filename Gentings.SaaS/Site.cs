using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions;

namespace Gentings.SaaS
{
    /// <summary>
    /// 网站。
    /// </summary>
    [Table("saas_Sites")]
    public class Site : IIdObject
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 默认域名。
        /// </summary>
        [Size(64)]
        public string Domain { get; set; }

        /// <summary>
        /// 禁用。
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}