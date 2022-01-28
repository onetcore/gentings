namespace Gentings.Extensions.Sites
{
    /// <summary>
    /// 网站配置。
    /// </summary>
    public class SiteSettings
    {
        /// <summary>
        /// 网站名称。
        /// </summary>
        public string SiteName { get; set; } = "云顶创联";

        /// <summary>
        /// 版权信息：${year}，${site}，${version}。
        /// </summary>
        public string? Copyright { get; set; }

        private string? _displayCopyright;
        /// <summary>
        /// 获取显示的版权信息。
        /// </summary>
        public string? DisplayCopyright => _displayCopyright ??= Copyright?
            .Replace("&year;", DateTime.Today.Year.ToString())
            .Replace("&site;", SiteName)
            .Replace("&version;", Cores.Version.ToString(2));

        /// <summary>
        /// 头部代码，全站通用。
        /// </summary>
        public string? Header { get; set; }

        /// <summary>
        /// 尾部代码，全站通用。
        /// </summary>
        public string? Footer { get; set; }

        /// <summary>
        /// 是否宽屏。
        /// </summary>
        public bool IsFluid { get; set; }
    }
}
