using Gentings.Security.Avatars;
using Gentings.Storages;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Security.Controllers
{
    /// <summary>
    /// 头像控制器。
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AvatarController : ControllerBase
    {
        private readonly IAvatarManager _avatarManager;

        /// <summary>
        /// 初始化类<see cref="AvatarController"/>。
        /// </summary>
        /// <param name="avatarManager">存储文件夹接口。</param>
        public AvatarController(IAvatarManager avatarManager)
        {
            _avatarManager = avatarManager;
        }

        /// <summary>
        /// 访问头像文件。
        /// </summary>
        /// <param name="userid">用户Id。</param>
        /// <param name="size">大小。</param>
        /// <returns>返回文件结果。</returns>
        [Route("s-avatars/{userid:int}.png", Order = int.MaxValue)]
        [Route("s-avatars/{userid:int}x{size:int}.png", Order = int.MaxValue)]
        public IActionResult Index(int userid, int size = 0)
        {
            var file = _avatarManager.GetFile(userid, size);
            if (!file.Exists)
            {
                return NotFound();
            }

            return PhysicalFile(file.FullName, file.Extension.GetContentType());
        }
    }
}
