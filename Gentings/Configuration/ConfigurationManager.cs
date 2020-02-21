using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Configuration
{
    /// <summary>
    /// 配置管理。
    /// </summary>
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IMemoryCache _cache;
        private const string ConfigDir = "config";

        private string GetPath(string name)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), ConfigDir);
            return Path.Combine(path, $"{name}.json");
        }

        private string GetCacheKey(string name) => $"{ConfigDir}:[{name}]";

        /// <summary>
        /// 初始化类<see cref="ConfigurationManager"/>。
        /// </summary>
        /// <param name="cache">缓存接口。</param>
        public ConfigurationManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 加载配置。   
        /// </summary>
        /// <typeparam name="TConfiguration">配置类型。</typeparam>
        /// <param name="name">名称，不包含文件扩展名。</param>
        /// <param name="minutes">缓存分钟数。</param>
        /// <returns>返回配置实例。</returns>
        public virtual TConfiguration LoadConfiguration<TConfiguration>(string name, int minutes = -1)
        {
            if (minutes <= 0) return LoadConfigurationFile<TConfiguration>(name);
            return _cache.GetOrCreate(GetCacheKey(name), ctx =>
            {
                ctx.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));
                return LoadConfigurationFile<TConfiguration>(name);
            });
        }

        private TConfiguration LoadConfigurationFile<TConfiguration>(string name)
        {
            var path = GetPath(name);
            if (!File.Exists(path))
                return default;
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs, Encoding.UTF8);
            return Cores.FromJsonString<TConfiguration>(sr.ReadToEnd());
        }

        /// <summary>
        /// 加载配置。   
        /// </summary>
        /// <typeparam name="TConfiguration">配置类型。</typeparam>
        /// <param name="name">名称，不包含文件扩展名。</param>
        /// <param name="minutes">缓存分钟数。</param>
        /// <returns>返回配置实例。</returns>
        public virtual async Task<TConfiguration> LoadConfigurationAsync<TConfiguration>(string name, int minutes = -1)
        {
            if (minutes <= 0) return await LoadConfigurationFileAsync<TConfiguration>(name);
            return await _cache.GetOrCreateAsync(GetCacheKey(name), async ctx =>
            {
                ctx.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));
                return await LoadConfigurationFileAsync<TConfiguration>(name);
            });
        }

        private async Task<TConfiguration> LoadConfigurationFileAsync<TConfiguration>(string name)
        {
            var path = GetPath(name);
            if (!File.Exists(path))
                return default;
            await using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs, Encoding.UTF8);
            return Cores.FromJsonString<TConfiguration>(await sr.ReadToEndAsync());
        }
    }
}