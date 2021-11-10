using GS.Extensions;

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
    }
}
