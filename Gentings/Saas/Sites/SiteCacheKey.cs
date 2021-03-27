using System;

namespace Gentings.Saas.Sites
{
    /// <summary>
    /// 网站缓存键类型。
    /// </summary>
    public class SiteCacheKey : Tuple<Type, int>
    {
        /// <summary>初始化类<see cref="SiteCacheKey"/>。</summary>
        /// <param name="type">类型实例。</param>
        /// <param name="siteId">值。</param>
        public SiteCacheKey(Type type, int siteId) : base(type, siteId)
        {
        }
    }
}