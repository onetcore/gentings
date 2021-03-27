using System.Linq;
using Gentings.AspNetCore.ApiDocuments;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.AspNetCore.OpenServices
{
    /// <summary>
    /// 开放服务文档管理实现类。
    /// </summary>
    public class ServiceDocumentManager : DocumentManagerBase, IServiceDocumentManager
    {
        /// <summary>
        /// 初始化类<see cref="ServiceDocumentManager"/>。
        /// </summary>
        /// <param name="cache">缓存接口。</param>
        /// <param name="provider">Action描述实例提供者。</param>
        public ServiceDocumentManager(IMemoryCache cache, IApiDescriptionGroupCollectionProvider provider) : base(cache, provider)
        {
        }

        /// <summary>
        /// 判断是否符合当前文档实例。
        /// </summary>
        /// <param name="descriptor">控制器操作实例。</param>
        /// <returns>返回判断结果。</returns>
        protected override bool IsValidated(ApiDescription descriptor)
        {
            if (base.IsValidated(descriptor))
                return descriptor.ActionDescriptor.EndpointMetadata.Any(x => x is OpenServiceAttribute);
            return false;
        }
    }
}