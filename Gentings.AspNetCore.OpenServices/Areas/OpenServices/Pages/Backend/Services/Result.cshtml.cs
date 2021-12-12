using Gentings.Extensions.OpenServices;
using Gentings.Extensions.OpenServices.ApiDocuments;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.OpenServices.Areas.OpenServices.Pages.Backend.Services
{
    /// <summary>
    /// 返回详情。
    /// </summary>
    public class ResultModel : ModelBase
    {
        private readonly IServiceDocumentManager _documentManager;
        /// <summary>
        /// 初始化类<see cref="ResultModel"/>。
        /// </summary>
        /// <param name="documentManager">文档管理接口。</param>
        public ResultModel(IServiceDocumentManager documentManager)
        {
            _documentManager = documentManager;
        }

        /// <summary>
        /// 返回类型列表。
        /// </summary>
        public ApiDescriptor ApiDescriptor { get; private set; }

        /// <summary>
        /// 获取Token页面。
        /// </summary>
        public IActionResult OnGet(string method, string route)
        {
            ApiDescriptor = _documentManager.GetApiDescriptors()
                .SingleOrDefault(x => x.HttpMethod == method && x.RouteTemplate == route);
            if (ApiDescriptor == null)
                return NotFound();
            return Page();
        }
    }
}
