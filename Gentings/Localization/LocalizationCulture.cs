namespace Gentings.Localization
{
    /// <summary>
    /// 默认语言接口。
    /// </summary>
    public class LocalizationCulture : ILocalizationCulture
    {
        /// <summary>
        /// 默认语言名称。
        /// </summary>
        public virtual string DefaultCultureName => "zh-CN";

        /// <summary>
        /// 支持的语言列表，键值对：{zh-CN:简体中文}。
        /// </summary>
        /// <returns>返回支持语言的键值对列表。</returns>
        public virtual IDictionary<string, string> SupportedLanguages { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
            { "zh-CN", "简体中文" },
            {"en", "English" }
        };

        /// <summary>
        /// 获取显示名称。
        /// </summary>
        /// <param name="cultureName">当前资源键值。</param>
        /// <returns>返回显示名称。</returns>
        public virtual string GetDisplayName(string cultureName)
        {
            cultureName = GetSupportedLanguage(cultureName);
            return SupportedLanguages[cultureName];
        }

        /// <summary>
        /// 获取当前语言支持的语言，不过存在则返回存在的语言，否则直接返回默认语言。
        /// </summary>
        /// <param name="cultureName">当前语言。</param>
        /// <returns>返回支持的语言。</returns>
        public string GetSupportedLanguage(string cultureName)
        {
            if (SupportedLanguages.ContainsKey(cultureName))
                return cultureName;
            var index = cultureName.IndexOf('-');
            if (index != -1)
            {
                cultureName = cultureName[..index];
                if (SupportedLanguages.ContainsKey(cultureName))
                    return cultureName;
            }
            return DefaultCultureName;
        }

        /// <summary>
        /// 判断是不是默认语言。
        /// </summary>
        /// <param name="cultureName">当前语言。</param>
        /// <returns>返回判断结果。</returns>
        public virtual bool IsDefault(string cultureName)
        {
            return cultureName.IsCulture(DefaultCultureName);
        }
    }
}
