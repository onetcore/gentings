using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gentings.ChatServers.Controllers
{
    /// <summary>
    /// 聊天控制器。
    /// </summary>
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
            var user = await _userManager.FindAsync(UserId);
            return OkResult(user);
        }
    }
}
