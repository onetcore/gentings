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

        /// <summary>
        /// 保存用户实例。
        /// </summary>
        /// <param name="name">用户名称。</param>
        /// <param name="id">用户id。</param>
        /// <returns>返回保存结果。</returns>
        [HttpPost("save")]
        public IActionResult Save(string name, int id)
        {
            return Ok();
        }

        /// <summary>
        /// 保存用户实例。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <returns>返回保存结果。</returns>
        [HttpPost("save-user")]
        public IActionResult Save(User user)
        {
            return Ok();
        }
    }
}
