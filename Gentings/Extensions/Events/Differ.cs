using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 变更实体。
    /// </summary>
    [Table("core_Events_Objects")]
    public class Differ : IIdObject
    {
        /// <summary>
        /// 获取或设置唯一Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 日志操作实例。
        /// </summary>
        public DifferAction Action { get; set; }

        /// <summary>
        /// 类型名称。
        /// </summary>
        [Size(64)]
        public string? TypeName { get; set; }

        /// <summary>
        /// 属性名称。
        /// </summary>
        [Size(64)]
        public string? PropertyName { get; set; }

        /// <summary>
        /// 原始数据。
        /// </summary>
        [Size(64)]
        public string? Source { get; set; }

        /// <summary>
        /// 修改后得值。
        /// </summary>
        [Size(64)]
        public string? Value { get; set; }

        /// <summary>
        /// 用户Id。
        /// </summary>
        [NotUpdated]
        public int UserId { get; set; }

        /// <summary>
        /// 添加时间。
        /// </summary>
        [NotUpdated]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
    }
}