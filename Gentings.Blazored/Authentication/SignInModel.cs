using System.ComponentModel.DataAnnotations;

namespace Gentings.Blazored.Authentication
{
    /// <summary>
    /// 登录用户模型类型。
    /// </summary>
    public class SignInModel
    {
        /// <summary>
        /// 用户名称。
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 密码。
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 记住登录状态。
        /// </summary>
        public bool RememberedMe { get; set; }

        /// <summary>
        /// 验证码。
        /// </summary>
        public string ValidCode { get; set; }
    }
}