using Gentings.Extensions.OpenServices;
using Gentings.Extensions.OpenServices.ApiDocuments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Gentings.AspNetCore.OpenServices.Areas.OpenServices.Pages.Backend.Services
{
    /// <summary>
    /// 调试窗口。
    /// </summary>
    public class TestModel : ModelBase
    {
        private readonly IServiceDocumentManager _serviceManager;
        /// <summary>
        /// 初始化类<see cref="TestModel"/>。
        /// </summary>
        /// <param name="serviceManager">服务文档管理接口。</param>
        public TestModel(IServiceDocumentManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// 获取API实例。
        /// </summary>
        /// <param name="id">API路由。</param>
        /// <param name="method">方法。</param>
        public IActionResult OnGet(string id, string method = "GET")
        {
            Api = _serviceManager.GetApiDescriptors()
                .SingleOrDefault(x => x.RouteTemplate.Equals(id, StringComparison.OrdinalIgnoreCase) && x.HttpMethod == method);
            if (Api == null)
                return NotFound();
            return Page();
        }

        /// <summary>
        /// Token模型。
        /// </summary>
        public class TokenModel
        {
            /// <summary>
            /// Token标识。
            /// </summary>
            [Required(ErrorMessage = "Token不能为空！")]
            public string Token { get; set; }
        }

        /// <summary>
        /// 设置Token实例。
        /// </summary>
        /// <param name="token">Token标识。</param>
        /// <returns>返回设置结果。</returns>
        public IActionResult OnPostToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                HttpContext.Response.Cookies.Delete(ApiDescriptor.JwtToken);
                HttpContext.Response.Cookies.Append(ApiDescriptor.JwtToken, token, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(1)
                });
                return Success("你已经成功设置了Token！");
            }
            return Error("Token标识不能为空！");
        }

        /// <summary>
        /// 当前API实例。
        /// </summary>
        public ApiDescriptor Api { get; private set; }
    }
}
