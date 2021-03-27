using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Gentings.AspNetCore.OpenServices.Controllers
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
        /// <returns>返回Token实例。</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return OkResult("成功获取Token路径！");
        }

        /// <summary>
        /// 获取验证Token。
        /// </summary>
        /// <param name="input">验证模型实例。</param>
        /// <returns>返回Token实例。</returns>
        [HttpPost]
        [ApiResult(typeof(TokenResult))]
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromBody] TokenModel input)
        {
            if (string.IsNullOrEmpty(input.AppId) || !Guid.TryParse(input.AppId, out var appid))
                return BadParameter(input.AppId);

            if (string.IsNullOrEmpty(input.AppSecret))
                return BadParameter(input.AppSecret);

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
            var result = GetRequiredService<IConfiguration>().CreateJwtSecurityToken(claims);
            return OkResult(new TokenResult(result));
        }
    }
}