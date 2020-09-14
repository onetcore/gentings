using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions.SensitiveWords
{
    /// <summary>
    /// 敏感词汇。
    /// </summary>
    [Table("core_SensitiveWords")]
    public class SensitiveWord : IIdObject
    {
        /// <summary>
        /// 自增长ID。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 敏感词汇。
        /// </summary>
        [Size(32)]
        public string Word { get; set; }

        /// <summary>
        /// 添加时间。
        /// </summary>
        [NotUpdated]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
    }
}