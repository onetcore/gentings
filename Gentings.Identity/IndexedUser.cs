using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Identity
{
    /// <summary>
    /// 子用户索引表格。
    /// </summary>
    [Table("sec_Users_Indexed")]
    public class IndexedUser
    {
        /// <summary>
        /// 用户Id。
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// 子用户Id。
        /// </summary>
        [Key]
        public int IndexedId { get; set; }
    }
}