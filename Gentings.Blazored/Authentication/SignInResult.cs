namespace Gentings.Blazored.Authentication
{
    /// <summary>
    /// 登录用户返回数据类型。
    /// </summary>
    public class SignInResult
    {
        /// <summary>
        /// 需要二次登录验证。
        /// </summary>
        public bool RequiresTwoFactor { get; set; }

        /// <summary>
        /// 验证标识。
        /// </summary>
        public string Token { get; set; }
    }
}