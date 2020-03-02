using Gentings.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.ChatServers
{
    /// <summary>
    /// 朋友。
    /// </summary>
    [Table("chat_Friends")]
    public class Friend
    {
        /// <summary>
        /// 用户Id。
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// 好友Id。
        /// </summary>
        [Key]
        public int FriendId { get; set; }

        /// <summary>
        /// 好友别名。
        /// </summary>
        [Size(64)]
        public string Alias { get; set; }

        /// <summary>
        /// 状态。
        /// </summary>
        public FriendStatus Status { get; set; }

        /// <summary>
        /// 未读消息数量。
        /// </summary>
        public int Unreads { get; set; }
    }
}
