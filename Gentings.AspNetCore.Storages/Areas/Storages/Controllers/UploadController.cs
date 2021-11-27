using Gentings.Storages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Storages.Areas.Storages.Controllers
{
    /// <summary>
    /// 上传文件。
    /// </summary>
    [Authorize]
    [DisableRequestSizeLimit]
    [Route("/storages/{extension}/{action}")]
    public class UploadController : ControllerBase
    {
        private readonly IMediaDirectory _mediaDirectory;
        /// <summary>
        /// 初始化类<see cref="UploadController"/>。
        /// </summary>
        /// <param name="mediaDirectory">媒体文件夹接口实例。</param>
        public UploadController(IMediaDirectory mediaDirectory)
        {
            _mediaDirectory = mediaDirectory;
        }

        /// <summary>
        /// 上传文件。
        /// </summary>
        /// <param name="file">文件表单实例。</param>
        /// <param name="extension">扩展名称。</param>
        /// <returns>返回上传结果。</returns>
        public async Task<IActionResult> Upload(IFormFile file, string extension)
        {
            if (file?.Length <= 0)
                return Error("请选择非空文件后，再上传！");
            var result = await _mediaDirectory.UploadAsync(file, extension);
            if (result)//不传回数据，刷新页面
                return Success("你已经成功上传了文件！");
            return Error("文件上传失败，请重试！");
        }
    }
}
