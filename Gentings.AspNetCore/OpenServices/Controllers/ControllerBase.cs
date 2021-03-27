using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.OpenServices.Controllers
{
    /// <summary>
    /// 开发服务基类。
    /// </summary>
    [OpenService]
    [Route("open/[controller]")]
    public abstract class ControllerBase : AspNetCore.ControllerBase
    {
        private Guid? _appid;
        /// <summary>
        /// 当前请求的AppId。
        /// </summary>
        protected Guid AppId
        {
            get
            {
                if (_appid == null)
                {
                    var appid = HttpContext.GetUserFirstValue(ClaimTypes.PrimarySid);
                    if (string.IsNullOrWhiteSpace(appid) || !Guid.TryParse(appid, out var result))
                        result = Guid.Empty;
                    _appid = result;
                }

                return _appid.Value;
            }
        }

        private Application _application;
        /// <summary>
        /// 当前请求的应用实例。
        /// </summary>
        protected Application Application
        {
            get
            {
                if (_application == null)
                {
                    if (AppId == Guid.Empty)
                        return null;
                    _application = GetRequiredService<IApplicationManager>().Find(AppId);
                }

                return _application;
            }
        }
    }
}