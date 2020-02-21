using System.Threading.Tasks;

namespace Gentings.Configuration
{
    /// <summary>
    /// JSON配置数据管理接口。
    /// </summary>
    public interface IConfigurationManager : ISingletonService
    {
        /// <summary>
        /// 加载配置。   
        /// </summary>
        /// <typeparam name="TConfiguration">配置类型。</typeparam>
        /// <param name="name">名称，不包含文件扩展名。</param>
        /// <param name="minutes">缓存分钟数。</param>
        /// <returns>返回配置实例。</returns>
        TConfiguration LoadConfiguration<TConfiguration>(string name, int minutes = -1);

        /// <summary>
        /// 加载配置。   
        /// </summary>
        /// <typeparam name="TConfiguration">配置类型。</typeparam>
        /// <param name="name">名称，不包含文件扩展名。</param>
        /// <param name="minutes">缓存分钟数。</param>
        /// <returns>返回配置实例。</returns>
        Task<TConfiguration> LoadConfigurationAsync<TConfiguration>(string name, int minutes = -1);
    }
}