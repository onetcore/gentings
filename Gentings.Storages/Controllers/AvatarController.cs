using Gentings.Storages.Avatars;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Storages.Controllers
{
    /// <summary>
    /// 头像控制器。
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AvatarController : Controller
    {
        private readonly IAvatarManager _avatarManager;

        /// <summary>
        /// 初始化类<see cref="StorageController"/>。
        /// </summary>
        /// <param name="avatarManager">存储文件夹接口。</param>
        public AvatarController(IAvatarManager avatarManager)
        {
            _avatarManager = avatarManager;
        }

        /// <summary>
        /// 访问头像文件。
        /// </summary>
        /// <param name="dir">文件夹名称。</param>
        /// <param name="name">文件名称。</param>
        /// <returns>返回文件结果。</returns>
        [Route("s-avatars/{userid:int}.png")]
        [Route("s-avatars/{userid:int}x{size:int}.png")]
        public IActionResult Index(int userid, int size = 0)
        {
            var file = _avatarManager.GetFile(userid, size);
            if (!file.Exists)
                return NotFound();
            return PhysicalFile(file.FullName, file.Extension.GetContentType());
        }
    }
}
