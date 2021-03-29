using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions;

namespace Gentings.Security.Scores
{
    /// <summary>
    /// 用户积分。
    /// </summary>
    [Table("sec_Scores")]
    public class UserScore
    {
        /// <summary>
        /// 用户Id。
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// 积分。
        /// </summary>
        [NotUpdated]
        public int Score { get; set; }

        /// <summary>
        /// 锁定积分。
        /// </summary>
        [NotUpdated]
        public int LockedScore { get; set; }

        /// <summary>
        /// 更新版本。
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// 改变积分时间。
        /// </summary>
        [NotUpdated]
        public DateTimeOffset? ScoredDate { get; set; }
    }
}