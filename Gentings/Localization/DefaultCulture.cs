namespace Gentings.Localization
{
    /// <summary>
    /// 默认语言接口。
    /// </summary>
    public class DefaultCulture : IDefaultCulture
    {
        /// <summary>
        /// 默认语言名称。
        /// </summary>
        public string CultureName => "zh-CN";

        /// <summary>
        /// 判断是不是默认语言。
        /// </summary>
        /// <param name="cultureName">当前语言。</param>
        /// <returns>返回判断结果。</returns>
        public bool IsDefault(string cultureName)
        {
            if (cultureName == null) return false;
            cultureName = cultureName.ToLowerInvariant();
            var def = CultureName.ToLowerInvariant();
            if (cultureName == def)
                return true;
            var index = cultureName.IndexOf('-');
            if (index == -1) return false;
            cultureName = cultureName.Substring(0, index);
            return cultureName == def;
        }
    }
}
