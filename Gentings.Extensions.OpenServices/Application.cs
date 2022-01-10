using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions.OpenServices
{
    /// <summary>
    /// 应用实例。
    /// </summary>
    [Table("core_Applications")]
    public class Application : ExtendBase, IIdObject<Guid>
    {
        /// <summary>
        /// 获取或设置唯一Id。
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 密钥。
        /// </summary>
        [Size(128)]
        public string AppSecret { get; set; } = Cores.GeneralKey(128);

        /// <summary>
        /// 名称。
        /// </summary>
        [Size(12)]
        public string Name { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        [Size(256)]
        public string Summary { get; set; }

        /// <summary>
        /// 添加时间。
        /// </summary>
        [NotUpdated]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// 到期时间。
        /// </summary>
        public DateTimeOffset ExpiredDate { get; set; } = DateTimeOffset.Now.AddYears(1);

        /// <summary>
        /// 状态。
        /// </summary>
        public ApplicationStatus Status { get; set; }

        /// <summary>
        /// 所属用户Id。
        /// </summary>
        public int UserId { get; set; }
    }
}
