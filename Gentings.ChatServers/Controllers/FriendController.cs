using System.Threading.Tasks;
using Gentings.Data;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.ChatServers.Controllers
{
    /// <summary>
    /// 好友控制器。
    /// </summary>
    public class FriendController : ControllerBase
    {
        private readonly IDbContext<Friend> _context;

        public FriendController(IDbContext<Friend> context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取好友列表。
        /// </summary>
        /// <returns>返回好友列表。</returns>
        [HttpGet("get-friends")]
        public async Task<IActionResult> GetFriendsAsync()
        {
            var friends = await _context.FetchAsync(x => x.UserId == UserId);
            return OkResult(friends);
        }
    }
}