using System;
using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions;

namespace Gentings.Identity.Scores
{
    /// <summary>
    /// 报表。
    /// </summary>
    [Table("sec_Scores_Reporters")]
    public class ScoreReporter : IIdObject
    {
        /// <summary>
        /// 获取或设置唯一Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 用户Id。
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 积分使用类型。
        /// </summary>
        public ScoreType ScoreType { get; set; }

        /// <summary>
        /// 模式。
        /// </summary>
        public IndexedMode Mode { get; set; }

        /// <summary>
        /// 时间或者日期，如年yyyy，月yyyyMM，日yyyyMMdd，时yyyyMMddHH。
        /// </summary>
        public int Date { get; set; }

        /// <summary>
        /// 消费积分。
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 生成报表时间。
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }
    }
}