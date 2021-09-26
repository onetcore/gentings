using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Gentings.AspNetCore;
using Gentings.Extensions.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.OpenServices.Controllers
{
    /// <summary>
    /// Token服务。
    /// </summary>
    public class TokenController : ControllerBase
    {
        private readonly IApplicationManager _applicationManager;
        /// <summary>
        /// 初始化类<see cref="TokenController"/>。
        /// </summary>
        /// <param name="applicationManager">应用管理实例。</param>
        public TokenController(IApplicationManager applicationManager)
        {
            _applicationManager = applicationManager;
        }

        /// <summary>
        /// 获取验证Token。
        /// </summary>
        /// <param name="input">验证模型实例。</param>
        /// <returns>返回Token实例。</returns>
        [HttpPost]
        [AllowAnonymous]
        [ApiResult(typeof(TokenResult))]
        public async Task<IActionResult> Index([FromBody] TokenModel input)
        {
            var valid = true;
            if (string.IsNullOrEmpty(input.AppId) || !Guid.TryParse(input.AppId, out var appid))
            {
                valid = false;
                appid = Guid.Empty;
                ModelState.AddModelError(nameof(input.AppId), Resources.TokenModel_AppIdNull);
            }

            if (string.IsNullOrEmpty(input.AppSecret))
            {
                valid = false;
                ModelState.AddModelError(nameof(input.AppSecret), Resources.TokenModel_AppSecretNull);
            }

            if (valid)
            {
                var application = await _applicationManager.FindUserApplicationAsync(appid);
                if (application == null)
                    return BadResult(ErrorCode.ApplicationNotFound);

                if (!application.AppSecret.Equals(input.AppSecret, StringComparison.OrdinalIgnoreCase))
                    return BadResult(ErrorCode.AppSecretInvalid);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, application.UserId.ToString()),
                    new Claim(ClaimTypes.Name, application["UserName"]),
                    new Claim(ClaimTypes.PrimarySid, application.Id.ToString("N"))
                };
                var result = CreateJwtSecurityToken(claims);
                return OkResult(new TokenResult(result));
            }

            return BadResult();
        }
    }
}