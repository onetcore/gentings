﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 事件消息。
    /// </summary>
    [Table("core_Events")]
    public class Event : ExtendBase, IIdObject
    {
        /// <summary>
        /// 获取或设置唯一Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 事件类型Id。
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// 当前用户Id。
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 事件等级。
        /// </summary>
        public EventLevel Level { get; set; } = EventLevel.Information;

        /// <summary>
        /// 活动时间。
        /// </summary>
        [NotUpdated]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// IP地址。
        /// </summary>
        [Size(32)]
        // ReSharper disable once InconsistentNaming
        public string? IPAdress { get; set; }

        /// <summary>
        /// 来源。
        /// </summary>
        [Size(64)]
        public string? Source { get; set; }

        /// <summary>
        /// 操作日志。
        /// </summary>
        [NotMapped]
        public string? Message
        {
            get => this[nameof(Message)]; set => this[nameof(Message)] = value;
        }

        /// <summary>
        /// 详细信息。
        /// </summary>
        [NotMapped]
        public string? Data
        {
            get => this[nameof(Data)]; set => this[nameof(Data)] = value;
        }
    }
}