using System.Threading.Tasks;
using Gentings.Security.Properties;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Security.Controllers
{
    /// <summary>
    /// 用户控制器。
    /// </summary>
    [Authorize]
    public class AccountController : AspNetCore.ControllerBase
    {
        /// <summary>
        /// 退出登录。
        /// </summary>
        [Route("logout")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            await HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
            await LogAsync(Resources.Logout_Success);
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            return LocalRedirect("/");
        }

        /// <summary>
        /// 退出登录。
        /// </summary>
        [Route("api/signout")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            await HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
            await LogAsync(Resources.Logout_Success);
            return OkResult();
        }
    }
}