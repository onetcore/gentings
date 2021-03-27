using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions.Emails
{
    /// <summary>
    /// 电子邮件配置。
    /// </summary>
    [Table("core_Emails_Settings")]
    public class EmailSettings : IIdObject
    {
        /// <summary>
        /// 扩展名称。
        /// </summary>
        public const string ExtensionName = "emails";

        /// <summary>
        /// 最大发送次数。
        /// </summary>
        public const int MaxTryTimes = 5;

        /// <summary>
        /// 启用。
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// SMTP地址。
        /// </summary>
        [Size(64)]
        public string SmtpServer { get; set; }

        /// <summary>
        /// SMTP地址。
        /// </summary>
        [Size(64)]
        public string SmtpUserName { get; set; }

        /// <summary>
        /// 端口。
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// 使用SSL。
        /// </summary>
        public bool UseSsl { get; set; }

        /// <summary>
        /// 密码。
        /// </summary>
        [Size(64)]
        public string SmtpPassword { get; set; }

        /// <summary>
        /// 发送个数。
        /// </summary>
        [NotUpdated]
        public int Count { get; set; }

        /// <summary>
        /// 获取或设置唯一Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        [Size(256)]
        public string Summary { get; set; }
    }
}