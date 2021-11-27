using Gentings.Extensions;
using Gentings.Storages;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Storages.Areas.Storages.Pages.Backend
{
    /// <summary>
    /// 媒体文件管理。
    /// </summary>
    public class IndexModel : ModelBase
    {
        private readonly IMediaDirectory _mediaDirectory;
        /// <summary>
        /// 初始化类<see cref="IndexModel"/>。
        /// </summary>
        /// <param name="mediaDirectory">媒体文件夹实例对象。</param>
        public IndexModel(IMediaDirectory mediaDirectory)
        {
            _mediaDirectory = mediaDirectory;
        }

        /// <summary>
        /// 媒体查询实例对象。
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public MediaQuery Query { get; set; }

        /// <summary>
        /// 文件实例列表。
        /// </summary>
        public IPageEnumerable<MediaFile> Items { get; private set; }

        /// <summary>
        /// 查询媒体文件列表。
        /// </summary>
        /// <returns>返回媒体文件页面视图。</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            Items = await _mediaDirectory.LoadAsync(Query);
            return Page();
        }

        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="id">文件Id列表。</param>
        /// <returns>返回删除结果。</returns>
        public async Task<IActionResult> OnPostDeleteAsync(Guid[] id)
        {
            if (id == null || id.Length == 0)
                return Error("请选择文件后再进行删除操作！");
            foreach (var fileId in id)
            {
                await _mediaDirectory.DeleteAsync(fileId);
            }
            return Success("你已经成功删除文件！");
        }
    }
}