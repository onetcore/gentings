using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.ChatServers
{
    /// <summary>
    /// 群组用户。
    /// </summary>
    [Table("chat_Groups_Users")]
    public class GroupUser
    {
        /// <summary>
        /// 群Id。
        /// </summary>
        [Key]
        public int GroupId { get; set; }

        /// <summary>
        /// 用户Id。
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// 用户类型。
        /// </summary>
        public GroupUserType UserType { get; set; }

        /// <summary>
        /// 群状态。
        /// </summary>
        public GroupUserStatus Status { get; set; }
    }
}
