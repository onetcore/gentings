using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ControllerBase = Gentings.AspNetCore.ControllerBase;

namespace GS.Controllers
{
    /// <summary>
    /// 賬戶控制器。
    /// </summary>
    [Authorize]
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// 退出登錄。
        /// </summary>
        /// <returns></returns>
        [Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
