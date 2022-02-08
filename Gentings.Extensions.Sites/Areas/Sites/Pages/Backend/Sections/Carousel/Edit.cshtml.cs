using Gentings.Extensions.Sites.SectionRenders.Carousels;
using Gentings.Storages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections.Carousel
{
    /// <summary>
    /// 编辑滚动项目。
    /// </summary>
    public class EditModel : ModelBase
    {
        private readonly ICarouselManager _carouselManager;
        private readonly IStorageDirectory _storageDirectory;

        public EditModel(ICarouselManager carouselManager, IStorageDirectory storageDirectory)
        {
            _carouselManager = carouselManager;
            _storageDirectory = storageDirectory;
        }

        /// <summary>
        /// 输入模型。
        /// </summary>
        [BindProperty]
        public SectionRenders.Carousels.Carousel? Input { get; set; }

        public IActionResult OnGet(int id, int sid)
        {
            if (id > 0)
            {
                Input = _carouselManager.Find(id);
                if (Input == null)
                    return NotFound();
            }
            else if (sid > 0)
                Input = new() { SectionId = sid };
            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Input!.Title))
            {
                ModelState.AddModelError("Input.Title", "标题不能为空！");
                return Error();
            }
            if (Input.Target == OpenTarget.Frame && string.IsNullOrEmpty(Input.FrameName))
                Input.Target = OpenTarget.Self;
            Input.UpdatedDate = DateTimeOffset.Now;
            var result = _carouselManager.Save(Input);
            return Json(result, "项目");
        }

        public async Task<IActionResult> OnPostUpload(IFormFile file, int sid)
        {
            if (file == null || file.Length == 0)
                return Error("不能为空文件！");

            var result = await _storageDirectory.SaveAsync(file, "sections", $"{sid}-{Cores.UnixNow.ToBase36()}.$");
            return Json(result);
        }
    }
}
