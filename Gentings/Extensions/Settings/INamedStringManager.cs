using Gentings.Extensions.Groups;

namespace Gentings.Extensions.Settings
{
    /// <summary>
    /// 字典管理接口。
    /// </summary>
    public interface INamedStringManager : IGroupManager<NamedString>
    {
        /// <summary>
        /// 通过路径获取字典值。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>返回字典值。</returns>
        string? GetString(string path);

        /// <summary>
        /// 通过路径获取字典值。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>返回字典值。</returns>
        Task<string?> GetStringAsync(string path);

        /// <summary>
        /// 通过路径获取字典值。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>返回字典值。</returns>
        string? GetOrAddString(string path);

        /// <summary>
        /// 通过路径获取字典值。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>返回字典值。</returns>
        Task<string?> GetOrAddStringAsync(string path);
    }
}