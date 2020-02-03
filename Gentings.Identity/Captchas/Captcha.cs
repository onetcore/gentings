using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions;

namespace Gentings.Identity.Captchas
{
    /// <summary>
    /// 验证码。
    /// </summary>
    [Table("core_Users_Captchas")]
    public class Captcha
    {
        /// <summary>
        /// 用户Id。
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
        public virtual DateTimeOffset CaptchaExpiredDate { get; set; } = DateTimeOffset.Now;
    }
}