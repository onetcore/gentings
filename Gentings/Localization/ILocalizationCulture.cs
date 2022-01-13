namespace Gentings.Localization
{
    /// <summary>
    /// 默认语言接口。
    /// </summary>
    public interface ILocalizationCulture : ISingletonService
    {
        /// <summary>
        /// 默认语言名称。
        /// </summary>
        string DefaultCultureName { get; }

        /// <summary>
        /// 判断是不是默认语言。
        /// </summary>
        /// <param name="cultureName">当前语言。</param>
        /// <returns>返回判断结果。</returns>
        bool IsDefault(string cultureName);

        /// <summary>
        /// 支持的语言列表，键值对：{zh-CN:简体中文}。
        /// </summary>
        IDictionary<string, string> SupportedLanguages { get; }

        /// <summary>
        /// 获取显示名称。
        /// </summary>
        /// <param name="cultureName">当前资源键值。</param>
        /// <returns>返回显示名称。</returns>
        string GetDisplayName(string cultureName);

        /// <summary>
        /// 获取当前语言支持的语言，不过存在则返回存在的语言，否则直接返回默认语言。
        /// </summary>
        /// <param name="cultureName">当前语言。</param>
        /// <returns>返回支持的语言。</returns>
        string GetSupportedLanguage(string cultureName);
    }
}
