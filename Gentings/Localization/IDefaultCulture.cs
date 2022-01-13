namespace Gentings.Localization
{
    /// <summary>
    /// 默认语言接口。
    /// </summary>
    public interface IDefaultCulture : ISingletonService
    {
        /// <summary>
        /// 默认语言名称。
        /// </summary>
        string CultureName { get; }

        /// <summary>
        /// 判断是不是默认语言。
        /// </summary>
        /// <param name="cultureName">当前语言。</param>
        /// <returns>返回判断结果。</returns>
        bool IsDefault(string cultureName);
    }
}
