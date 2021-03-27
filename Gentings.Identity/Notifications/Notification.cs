using System;
using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions;

namespace Gentings.Identity.Notifications
{
    /// <summary>
    /// 系统通知。
    /// </summary>
    [Table("core_Notifications")]
    public class Notification : ExtendBase, IIdObject
    {
        /// <summary>
        /// 自增长Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 通知内容。
        /// </summary>
        [NotMapped]
        public string Html { get => this[nameof(Html)]; set => this[nameof(Html)] = value; }

        /// <summary>
        /// 通知内容。
        /// </summary>
        [NotMapped]
        public string Code { get => this[nameof(Code)]; set => this[nameof(Code)] = value; }

        /// <summary>
        /// 接收通知用户Id。
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 发送通知用户。
        /// </summary>
        public int SendId { get; set; }

        /// <summary>
        /// 标题。
        /// </summary>
        [Size(256)]
        public string Title { get; set; }

        /// <summary>
        /// 发生通知时间。
        /// </summary>
        [NotUpdated]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// 通知状态。
        /// </summary>
        public NotificationStatus Status { get; set; } = NotificationStatus.New;
    }
}