using GS.Extensions;
using GS.Extensions.Security;

namespace GS.Pages
{
    /// <summary>
    /// 模型基类。
    /// </summary>
    public abstract class ModelBase : Gentings.AspNetCore.ModelBase
    {
        private SiteSettings? _settings;
        /// <summary>
        /// 网站配置。
        /// </summary>
        public SiteSettings Settings => _settings ??= GetRequiredService<SiteSettings>();

        private User? _currentUser;
        /// <summary>
        /// 获取当前登录用户。
        /// </summary>
        public User CurrentUser => _currentUser ??= GetRequiredService<User>();
    }
}
