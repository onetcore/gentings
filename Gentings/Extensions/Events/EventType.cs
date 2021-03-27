using System.ComponentModel.DataAnnotations;
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
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 名称。
        /// </summary>
        [NotUpdated]
        [Size(12)]
        public string Name { get; set; }

        /// <summary>
        /// 图标地址。
        /// </summary>
        [Size(256)]
        public string IconUrl { get; set; }

        /// <summary>
        /// 背景颜色。
        /// </summary>
        [Size(20)]
        public string BgColor { get; set; }

        /// <summary>
        /// 字体颜色。
        /// </summary>
        [Size(20)]
        public string Color { get; set; }
    }
}