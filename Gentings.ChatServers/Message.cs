using Gentings.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.ChatServers
{
    /// <summary>
    /// 聊天信息。
    /// </summary>
    [Table("chat_Messages")]
    public class Message : IIdObject<long>
    {
        /// <summary>
        /// 消息Id。
        /// </summary>
        [Identity]
        public long Id { get; set; }

        /// <summary>
        /// 发送者Id。
        /// </summary>
        public int Sender { get; set; }

        /// <summary>
        /// 接收者。
        /// </summary>
        public int Receiver { get; set; }

        /// <summary>
        /// 内容。
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否已读。
        /// </summary>
        public bool Readed { get; set; }

        /// <summary>
        /// 发送时间。
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// 读取时间。
        /// </summary>
        public DateTimeOffset? ReadedDate { get; set; }
    }
}
