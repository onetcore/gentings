using Gentings.Documents.XmlDocuments;
using Gentings.Extensions.OpenServices;
using Gentings.Security;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.OpenServices.Areas.OpenServices.Pages.Backend
{
    /// <summary>
    /// 开放服务模型。
    /// </summary>
    [PermissionAuthorize(Permissions.Setting)]
    public class ApisModel : ModelBase
    {
        /// <summary>
        /// 服务管理接口。
        /// </summary>
        public IOpenServiceManager ServiceManager { get; }
        private readonly IServiceDocumentManager _serviceManager;
        private readonly IApplicationManager _applicationManager;

        /// <summary>
        /// 初始化类<see cref="IndexModel"/>。
        /// </summary>
        /// <param name="serviceDocumentManager">服务文档管理接口。</param>
        /// <param name="serviceManager">开放服务管理接口。</param>
        /// <param name="applicationManager">应用程序管理接口。</param>
        public ApisModel(IServiceDocumentManager serviceDocumentManager, IOpenServiceManager serviceManager, IApplicationManager applicationManager)
        {
            ServiceManager = serviceManager;
            _serviceManager = serviceDocumentManager;
            _applicationManager = applicationManager;
        }

        /// <summary>
        /// 文档列表。
        /// </summary>
        public IDictionary<string, IEnumerable<ApiDescriptor>> Document { get; private set; }

        /// <summary>
        /// 当前应用程序。
        /// </summary>
        public Application Application { get; private set; }

        /// <summary>
        /// 获取文档列表。
        /// </summary>
        /// <param name="appid">应用程序Id。</param>
        public async Task<IActionResult> OnGet(Guid appid)
        {
            Application = await _applicationManager.FindAsync(appid);
            if (Application == null)
                return NotFound();
            Services = await _applicationManager.LoadApplicationServicesAsync(appid);
            Document = _serviceManager.GetGroupApiDescriptors();
            return Page();
        }

        /// <summary>
        /// 关联API。
        /// </summary>
        /// <param name="appid">应用程序Id。</param>
        /// <param name="id">服务ID列表。</param>
        /// <returns>返回关联结果。</returns>
        public async Task<IActionResult> OnPostAddAsync(Guid appid, int[] id)
        {
            var result = await _applicationManager.AddApplicationServicesAsync(appid, id);
            if (result)
                return Success("你已经成功关联所选择的API到当前应用程序中！");
            return Error("关联失败，请重试！");
        }

        /// <summary>
        /// 当前应用程序包含的服务Id。
        /// </summary>
        public List<int> Services { get; private set; }
    }
}
