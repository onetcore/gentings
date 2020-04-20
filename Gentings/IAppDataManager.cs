using System.Threading.Tasks;

namespace Gentings
{
    /// <summary>
    /// JSON配置数据管理接口。
    /// </summary>
    public interface IAppDataManager : ISingletonService
    {
        /// <summary>
        /// 加载配置。   
        /// </summary>
        /// <typeparam name="TModel">配置类型。</typeparam>
        /// <param name="name">名称，不包含文件扩展名。</param>
        /// <param name="minutes">缓存分钟数。</param>
        /// <returns>返回配置实例。</returns>
        TModel LoadData<TModel>(string name, int minutes = -1);

        /// <summary>
        /// 加载配置。   
        /// </summary>
        /// <typeparam name="TModel">配置类型。</typeparam>
        /// <param name="name">名称，不包含文件扩展名。</param>
        /// <param name="minutes">缓存分钟数。</param>
        /// <returns>返回配置实例。</returns>
        Task<TModel> LoadDataAsync<TModel>(string name, int minutes = -1);

        /// <summary>
        /// 加载数据文件。
        /// </summary>
        /// <param name="name">文件名称。</param>
        /// <returns>返回当前文件内容字符串。</returns>
        string LoadFile(string name);

        /// <summary>
        /// 加载数据文件。
        /// </summary>
        /// <param name="name">文件名称。</param>
        /// <returns>返回当前文件内容字符串。</returns>
        Task<string> LoadFileAsync(string name);
    }
}