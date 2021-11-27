using Microsoft.Extensions.Caching.Memory;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;

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
        /// <returns>返回枚举值的小写字符串。</returns>
        public static string ToLowerString(this Enum value) => value.ToString().ToLower();

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
    }
}
