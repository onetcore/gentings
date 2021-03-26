using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Data.Extensions;

namespace Gentings.Extensions.SMS.Captchas
{
    /// <summary>
    /// 短信验证码。
    /// </summary>
    [Table("core_SMS_Captchas")]
    public class Captcha
    {
        /// <summary>
        /// 预留Id。
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 用户手机号码。
        /// </summary>
        [Key]
        [Size(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 验证码类型。
        /// </summary>
        [Key]
        [Size(20)]
        public string Type { get; set; }

        /// <summary>
        /// 验证码。
        /// </summary>
        [Size(10)]
        public virtual string Code { get; set; }

        /// <summary>
        /// 验证码过期时间。
        /// </summary>
        public virtual DateTimeOffset ExpiredDate { get; set; } = DateTimeOffset.Now;
    }
}