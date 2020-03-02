using Gentings.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.ChatServers
{
    /// <summary>
    /// 聊天用户。
    /// </summary>
    [Table("chat_Users")]
    public class User : IIdObject
    {
        /// <summary>
        /// 用户Id。
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 用户名称。
        /// </summary>
        [Size(64)]
        public string Name { get; set; }

        /// <summary>
        /// 是否在线。
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// 连接时间。
        /// </summary>
        public DateTimeOffset? ConnectedDate { get; set; }

        /// <summary>
        /// 断开连接时间。
        /// </summary>
        public DateTimeOffset? DisconnectedDate { get; set; }
    }
}
