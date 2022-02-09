using Gentings.Documents.XmlDocuments;
using Gentings.Extensions.OpenServices;

namespace Gentings.AspNetCore.OpenServices.Areas.OpenServices.Pages.Backend.Services
{
    /// <summary>
    /// 开放服务模型。
    /// </summary>
    public class IndexModel : ModelBase
    {
        /// <summary>
        /// 服务管理接口。
        /// </summary>
        public IOpenServiceManager ServiceManager { get; }
        private readonly IServiceDocumentManager _serviceManager;

        /// <summary>
        /// 初始化类<see cref="IndexModel"/>。
        /// </summary>
        /// <param name="serviceDocumentManager">服务文档管理接口。</param>
        /// <param name="serviceManager">开放服务管理接口。</param>
        public IndexModel(IServiceDocumentManager serviceDocumentManager, IOpenServiceManager serviceManager)
        {
            ServiceManager = serviceManager;
            _serviceManager = serviceDocumentManager;
        }
        /// <summary>
        /// 文档列表。
        /// </summary>
        public IDictionary<string, IEnumerable<ApiDescriptor>>? Document { get; private set; }
        /// <summary>
        /// 获取文档列表。
        /// </summary>
        public void OnGet()
        {
            Document = _serviceManager.GetGroupApiDescriptors();
        }
    }
}
