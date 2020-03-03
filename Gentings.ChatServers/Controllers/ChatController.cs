using Gentings.AspNetCore;
using Gentings.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ControllerBase = Gentings.AspNetCore.ControllerBase;

namespace Gentings.ChatServers.Controllers
{
    /// <summary>
    /// 聊天控制器。
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public ChatController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// 获取当前用户的实例对象。
        /// </summary>
        /// <returns>返回当前用户实例对象。</returns>
        [HttpGet("user")]
        public async Task<IActionResult> Init()
        {
            var user = await _userManager.FindAsync(User.GetUserId());
            return OkResult(user);
        }
    }
}
