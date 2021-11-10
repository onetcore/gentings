namespace GS.Extensions
{
    /// <summary>
    /// 网站配置。
    /// </summary>
    public class SiteSettings
    {
        /// <summary>
        /// 网站名称。
        /// </summary>
        public string SiteName { get; set; } = "Gentings Site";

        /// <summary>
        /// 开启登录验证码。
        /// </summary>
        public bool ValidCode { get; set; }
    }
}
