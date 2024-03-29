﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Storages.Controllers
{
    /// <summary>
    /// 存储控制器。
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MediaController : ControllerBase
    {
        private readonly IMediaDirectory _mediaFileProvider;
        /// <summary>
        /// 初始化类<see cref="MediaController"/>。
        /// </summary>
        /// <param name="mediaFileProvider">媒体文件提供者接口。</param>
        public MediaController(IMediaDirectory mediaFileProvider)
        {
            _mediaFileProvider = mediaFileProvider;
        }

        /// <summary>
        /// 访问媒体文件。
        /// </summary>
        /// <param name="name">文件名称。</param>
        /// <returns>返回文件结果。</returns>
        [Route("s-medias/{name}")]
        public async Task<IActionResult> Index(string name)
        {
            name = Path.GetFileNameWithoutExtension(name);
            if (!Guid.TryParse(name, out var id))
            {
                return NotFound();
            }

            var file = await _mediaFileProvider.FindPhysicalFileAsync(id);
            if (file == null || !System.IO.File.Exists(file.PhysicalPath))
            {
                return NotFound();
            }

            return PhysicalFile(file.PhysicalPath, file.ContentType);
        }

        /// <summary>
        /// 访问缩略图片文件。
        /// </summary>
        /// <param name="width">宽度。</param>
        /// <param name="height">高度。</param>
        /// <param name="name">文件名称。</param>
        /// <returns>返回文件结果。</returns>
        [Route("s-medias/{width:int}x{height:int}/{name}")]
        public async Task<IActionResult> Index(int width, int height, string name)
        {
            name = Path.GetFileNameWithoutExtension(name);
            if (!Guid.TryParse(name, out var id))
            {
                return NotFound();
            }

            var file = await _mediaFileProvider.FindThumbAsync(id, width, height);
            if (file == null || !System.IO.File.Exists(file.PhysicalPath))
            {
                return NotFound();
            }

            return PhysicalFile(file.PhysicalPath, "image/png");
        }

        /// <summary>
        /// 访问媒体文件。
        /// </summary>
        /// <param name="name">文件名称。</param>
        /// <returns>返回文件结果。</returns>
        [Route("s-dmedias/{name}")]
        public async Task<IActionResult> Attachment(string name)
        {
            name = Path.GetFileNameWithoutExtension(name);
            if (!Guid.TryParse(name, out var id))
            {
                return NotFound();
            }

            var file = await _mediaFileProvider.FindPhysicalFileAsync(id);
            if (file == null || !System.IO.File.Exists(file.PhysicalPath))
            {
                return NotFound();
            }

            Response.Headers.Add("Content-Disposition", $"attachment;filename={file.FileName}");
            return PhysicalFile(file.PhysicalPath, file.ContentType);
        }
    }
}