using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Storages
{
    /// <summary>
    /// 存储控制器。
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class StorageController : ControllerBase
    {
        private readonly IStorageDirectory _storageDirectory;

        /// <summary>
        /// 初始化类<see cref="StorageController"/>。
        /// </summary>
        /// <param name="storageDirectory">存储文件夹接口。</param>
        public StorageController(IStorageDirectory storageDirectory)
        {
            _storageDirectory = storageDirectory;
        }

        /// <summary>
        /// 访问存储文件。
        /// </summary>
        /// <param name="dir">文件夹名称。</param>
        /// <param name="name">文件名称。</param>
        /// <returns>返回文件结果。</returns>
        [Route("s-files/{dir:alpha}/{name}")]
        public IActionResult Index(string dir, string name)
        {
            name = Path.Combine(dir, name);
            var file = _storageDirectory.GetFile(name);
            if (file == null || !file.Exists)
            {
                return NotFound();
            }

            return PhysicalFile(file.FullName, file.Extension.GetContentType());
        }

        /// <summary>
        /// 访问媒体文件。
        /// </summary>
        /// <param name="dir">文件夹名称。</param>
        /// <param name="name">文件名称。</param>
        /// <returns>返回文件结果。</returns>
        [Route("d-files/{dir:alpha}/{name}")]
        public IActionResult Attachment(string dir, string name)
        {
            name = Path.Combine(dir, name);
            var file = _storageDirectory.GetFile(name);
            if (file == null || !file.Exists)
            {
                return NotFound();
            }

            Response.Headers.Add("Content-Disposition", $"attachment;filename={file.Name}");
            return PhysicalFile(file.FullName, file.Extension.GetContentType());
        }
    }
}