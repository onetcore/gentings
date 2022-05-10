using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel;
using System.Reflection;

namespace Gentings
{
    /// <summary>
    /// 核心扩展类型。
    /// </summary>
    public static class CoreExtensions
    {
        /// <summary>
        /// 获取小写字符串。
        /// </summary>
        /// <param name="value">枚举值。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>返回枚举值的小写字符串。</returns>
        public static string? ToLowerString(this object? value, string? defaultValue = "null")
        {
            return value?.ToString()?.ToLower() ?? defaultValue;
        }

        /// <summary>
        /// 获取枚举描述信息。
        /// </summary>
        /// <param name="value">枚举值。</param>
        /// <returns>返回枚举的描述信息。</returns>
        public static string ToDescriptionString(this Enum value)
        {
            var name = value.ToString();
            var info = value.GetType().GetField(name);
            if (info == null)
                return name;
            return info.GetCustomAttribute<DescriptionAttribute>()?.Description ?? name;
        }

        /// <summary>
        /// 默认缓存时长。
        /// </summary>
        public static readonly TimeSpan DefaultCacheExpiration = TimeSpan.FromMinutes(3);

        /// <summary>
        /// 设置默认缓存时间，3分钟。
        /// </summary>
        /// <param name="cache">缓存实体接口。</param>
        /// <returns>返回缓存实体接口。</returns>
        public static ICacheEntry SetDefaultAbsoluteExpiration(this ICacheEntry cache)
        {
            return cache.SetAbsoluteExpiration(DefaultCacheExpiration);
        }

        /// <summary>
        /// 判断是否为当前语言。
        /// </summary>
        /// <param name="culture">用于判断的语言。</param>
        /// <param name="current">当前语言。</param>
        /// <returns>返回判断结果。</returns>
        public static bool IsCulture(this string culture, string current)
        {
            if (culture == null || current == null) return false;
            current = current.ToLowerInvariant();
            culture = culture.ToLowerInvariant();
            if (culture == current) return true;
            var index = culture.IndexOf('-');
            if (index == -1) return false;
            culture = culture[..index];
            return culture == current;
        }
    }
}
