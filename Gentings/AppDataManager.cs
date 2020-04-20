using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings
{
    /// <summary>
    /// 配置管理。
    /// </summary>
    public class AppDataManager : IAppDataManager
    {
        private readonly IMemoryCache _cache;
        private const string ConfigDir = "App_Data";

        private string GetPath(string name)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), ConfigDir);
            if (name.IndexOf('.') == -1)
                name += ".json";
            return Path.Combine(path, name);
        }

        private string GetCacheKey(string name) => $"{ConfigDir}:[{name}]";

        /// <summary>
        /// 初始化类<see cref="AppDataManager"/>。
        /// </summary>
        /// <param name="cache">缓存接口。</param>
        public AppDataManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 加载配置。   
        /// </summary>
        /// <typeparam name="TModel">配置类型。</typeparam>
        /// <param name="name">名称，不包含文件扩展名。</param>
        /// <param name="minutes">缓存分钟数。</param>
        /// <returns>返回配置实例。</returns>
        public virtual TModel LoadData<TModel>(string name, int minutes = -1)
        {
            if (minutes <= 0)
            {
                return Cores.FromJsonString<TModel>(LoadFile(name));
            }

            return _cache.GetOrCreate(GetCacheKey(name), ctx =>
            {
                ctx.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));
                return Cores.FromJsonString<TModel>(LoadFile(name));
            });
        }

        /// <summary>
        /// 加载配置。   
        /// </summary>
        /// <typeparam name="TModel">配置类型。</typeparam>
        /// <param name="name">名称，不包含文件扩展名。</param>
        /// <param name="minutes">缓存分钟数。</param>
        /// <returns>返回配置实例。</returns>
        public virtual async Task<TModel> LoadDataAsync<TModel>(string name, int minutes = -1)
        {
            if (minutes <= 0)
            {
                return Cores.FromJsonString<TModel>(await LoadFileAsync(name));
            }

            return await _cache.GetOrCreateAsync(GetCacheKey(name), async ctx =>
            {
                ctx.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));
                return Cores.FromJsonString<TModel>(await LoadFileAsync(name));
            });
        }

        /// <summary>
        /// 加载数据文件。
        /// </summary>
        /// <param name="name">文件名称。</param>
        /// <returns>返回当前文件内容字符串。</returns>
        public virtual string LoadFile(string name)
        {
            var path = GetPath(name);
            if (!File.Exists(path))
            {
                return default;
            }

            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs, Encoding.UTF8);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// 加载数据文件。
        /// </summary>
        /// <param name="name">文件名称。</param>
        /// <returns>返回当前文件内容字符串。</returns>
        public virtual async Task<string> LoadFileAsync(string name)
        {
            var path = GetPath(name);
            if (!File.Exists(path))
            {
                return default;
            }

            await using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs, Encoding.UTF8);
            return await sr.ReadToEndAsync();
        }
    }
}