using Gentings.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.ChatServers
{
    /// <summary>
    /// 用户群。
    /// </summary>
    [Table("chat_Groups")]
    public class Group : IIdObject
    {
        /// <summary>
        /// 群Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 名称。
        /// </summary>
        [Size(64)]
        public string Name { get; set; }

        /// <summary>
        /// 最多用户数。
        /// </summary>
        public int Max { get; set; } = 500;

        /// <summary>
        /// 加入方式。
        /// </summary>
        public JoinMode JoinMode { get; set; }

        /// <summary>
        /// 图标地址。
        /// </summary>
        [Size(256)]
        public string IconUrl { get; set; }

        /// <summary>
        /// 群所有者。
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// 是否禁言，只有管理员和所有者能够聊天。
        /// </summary>
        public bool IsDeny { get; set; }
    }
}
