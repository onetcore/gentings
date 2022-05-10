using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 事件类型。
    /// </summary>
    [Table("core_Events_Types")]
    public class EventType
    {
        /// <summary>
        /// 事件类型Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 名称。
        /// </summary>
        [Size(12)]
        [NotUpdated]
        public string? Name { get; set; }

        /// <summary>
        /// 图标样式名称。
        /// </summary>
        [Size(64)]
        public string? IconName { get; set; }

        /// <summary>
        /// 背景颜色。
        /// </summary>
        [Size(20)]
        public string? BgColor { get; set; }

        /// <summary>
        /// 字体颜色。
        /// </summary>
        [Size(20)]
        public string? Color { get; set; }
    }
}