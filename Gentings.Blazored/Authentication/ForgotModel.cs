using System.ComponentModel.DataAnnotations;

namespace Gentings.Blazored.Authentication
{
    /// <summary>
    /// 忘记密码模型。
    /// </summary>
    public class ForgotModel
    {
        /// <summary>
        /// 邮件地址。
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}