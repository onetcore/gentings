using System.Collections.Generic;

namespace Gentings.Extensions.OpenServices.ApiDocuments
{
    /// <summary>
    /// 接口管理实例。
    /// </summary>
    public interface IDocumentManagerBase
    {
        /// <summary>
        /// 获取所有API描述。
        /// </summary>
        /// <returns>返回API描述列表。</returns>
        IEnumerable<ApiDescriptor> GetApiDescriptors();

        /// <summary>
        /// 获取程序集名称。
        /// </summary>
        /// <returns>程序集名称列表。</returns>
        IEnumerable<AssemblyInfo> GetAssemblies();

        /// <summary>
        /// 分组获取API描述实例。
        /// </summary>
        /// <returns>返回API描述实例字典列表。</returns>
        IDictionary<string, IEnumerable<ApiDescriptor>> GetGroupApiDescriptors();
    }
}