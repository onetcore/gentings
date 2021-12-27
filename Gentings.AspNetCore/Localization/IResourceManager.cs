namespace Gentings.AspNetCore.Localization
{
    /// <summary>
    /// UI多语言资源管理接口。
    /// </summary>
    public interface IResourceManager : ISingletonService
    {
        /// <summary>
        /// 获取资源实例。
        /// </summary>
        /// <param name="type">类型。</param>
        /// <param name="key">资源名称。</param>
        /// <returns>返回当前资源实例。</returns>
        string GetResource(Type type, string key);
    }
}
