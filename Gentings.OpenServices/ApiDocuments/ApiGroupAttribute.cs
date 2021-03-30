using Microsoft.AspNetCore.Mvc;

namespace Gentings.OpenServices.ApiDocuments
{
    /// <summary>
    /// API服务特性。
    /// </summary>
    public class ApiServiceAttribute : ApiExplorerSettingsAttribute
    {
        /// <summary>
        /// 初始化类<see cref="ApiServiceAttribute"/>。
        /// </summary>
        /// <param name="groupName">分组名称。</param>
        /// <param name="ignoreApi">是否忽略当前API。</param>
        public ApiServiceAttribute(string groupName, bool ignoreApi = false)
        {
            GroupName = groupName;
            IgnoreApi = ignoreApi;
        }
    }
}