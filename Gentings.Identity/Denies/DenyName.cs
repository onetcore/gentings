using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions;

namespace Gentings.Identity.Denies
{
    /// <summary>
    /// 非法用户名。
    /// </summary>
    [Table("core_Users_Names")]
    public class DenyName
    {
        /// <summary>
        /// 唯一Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 非法用户名。
        /// </summary>
        [Size(20)]
        public string Name { get; set; }
    }
}