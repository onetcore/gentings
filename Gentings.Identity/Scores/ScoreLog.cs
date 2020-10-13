using System;
using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions;

namespace Gentings.Identity.Scores
{
    /// <summary>
    /// 用户积分日志。
    /// </summary>
    [Table("core_Scores_Logs")]
    public class ScoreLog : IIdObject
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
        /// 发生改变的积分。
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 原始积分。
        /// </summary>
        public int BeforeScore { get; set; }

        /// <summary>
        /// 改变后剩余的积分。
        /// </summary>
        public int AfterScore { get; set; }

        /// <summary>
        /// 添加时间。
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

        private string _securityKey;
        /// <summary>
        /// 安全码。
        /// </summary>
        [Size(36)]
        public string SecurityKey
        {
            get => _securityKey ??= HashedKey;
            set => _securityKey = value;
        }

        /// <summary>
        /// 备注。
        /// </summary>
        [Size(256)]
        public string Remark { get; set; }

        /// <summary>
        /// 目标Id。
        /// </summary>
        public int? TargetId { get; set; }

        /// <summary>
        /// 哈希码。
        /// </summary>
        protected virtual string HashedKey =>
            Cores.Md5(Cores.Sha1($"{UserId}:{Score}:{BeforeScore}:{AfterScore}:{CreatedDate:yyyy-MM-DD HH:mm:ss}") + $"{UserId}:{CreatedDate:yyyy-MM-DD HH:mm:ss}");

        /// <summary>
        /// 是否合法。
        /// </summary>
        public bool IsValid => SecurityKey.Equals(HashedKey, StringComparison.OrdinalIgnoreCase);
    }
}