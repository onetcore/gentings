using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions;

namespace Gentings.Security
{
    /// <summary>
    /// 子用户索引表格。
    /// </summary>
    [Table("sec_Users_Indexed")]
    public class UserIndex : IParentIndex
    {
        /// <summary>
        /// 父级用户Id。
        /// </summary>
        [Key]
        public int ParentId { get; set; }

        /// <summary>
        /// 子用户Id。
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}