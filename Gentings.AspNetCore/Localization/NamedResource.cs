using System.Collections.Concurrent;

namespace Gentings.AspNetCore.Localization
{
    /// <summary>
    /// 文件资源。
    /// </summary>
    public class NamedResource
    {
        private readonly ConcurrentDictionary<string, Resource> _resources = new(StringComparer.OrdinalIgnoreCase);

        internal NamedResource(string resourceName, string path)
        {
            var resource = new Resource(path);
            _resources.AddOrUpdate(resourceName, resource, (k, v) => resource);
        }

        /// <summary>
        /// 获取当前类型的资源实例。
        /// </summary>
        /// <param name="resourceName">资源文件名称。</param>
        /// <param name="key">资源唯一键。</param>
        /// <returns>返回当前资源值。</returns>
        public string? GetResource(string resourceName, string key)
        {
            if (_resources.TryGetValue(resourceName, out var resource))
                return resource?[key];
            return null;
        }
    }
}
