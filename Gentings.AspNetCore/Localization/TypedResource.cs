using System.Collections.Concurrent;

namespace Gentings.AspNetCore.Localization
{
    /// <summary>
    /// 类型资源。
    /// </summary>
    public class TypedResource
    {
        private readonly ConcurrentDictionary<Type, Resource> _resources = new();

        internal TypedResource(Type type, string path)
        {
            var resource = new Resource(path);
            _resources.AddOrUpdate(type, resource, (k, v) => resource);
        }

        /// <summary>
        /// 获取当前类型的资源实例。
        /// </summary>
        /// <param name="type">类型。</param>
        /// <param name="key">资源唯一键。</param>
        /// <returns>返回当前资源值。</returns>
        public string? GetResource(Type type, string key)
        {
            if (_resources.TryGetValue(type, out var resource))
                return resource?[key];
            return null;
        }
    }
}
