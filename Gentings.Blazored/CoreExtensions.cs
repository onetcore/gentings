using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gentings.Blazored
{
    /// <summary>
    /// 核心扩展类。
    /// </summary>
    public static class CoreExtensions
    {
        /// <summary>
        /// 将查询字符串附加到URL地址中。
        /// </summary>
        /// <param name="url">URL地址。</param>
        /// <param name="query">查询实例。</param>
        /// <returns>返回附加后的结果。</returns>
        public static string AppendQuery(this string url, IDictionary<string, object> query = null)
        {
            if (query == null || query.Count == 0)
                return url;

            if (url.IndexOf('?') == -1)
                url += '?';
            else
                url += '&';

            url += string.Join("&", query.Select(x =>
            {
                var value = x.Value?.ToString();
                if (string.IsNullOrWhiteSpace(value))
                    return null;
                return $"{x.Key}={value.Trim()}";
            }).Where(x => x != null));
            return url;
        }

        /// <summary>
        /// 将查询字符串附加到URL地址中。
        /// </summary>
        /// <param name="url">URL地址。</param>
        /// <param name="query">查询实例。</param>
        /// <returns>返回附加后的结果。</returns>
        public static string AppendQuery(this string url, object query = null)
        {
            if (query == null) return url;
            return url.AppendQuery(query.GetType()
                .GetRuntimeProperties()
                .Select(x => new KeyValuePair<string, object>(x.Name.ToLower().ToLower(), x.GetValue(query)))
                .ToDictionary(x => x.Key, x => x.Value)
            );
        }
    }
}