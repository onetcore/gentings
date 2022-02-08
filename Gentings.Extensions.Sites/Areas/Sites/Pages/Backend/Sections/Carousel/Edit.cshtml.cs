using Gentings.Extensions.Sites.SectionRenders.Carousels;
using Gentings.Storages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections.Carousel
{
    /// <summary>
    /// �༭������Ŀ��
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
        /// ����ģ�͡�
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
                ModelState.AddModelError("Input.Title", "���ⲻ��Ϊ�գ�");
                return Error();
            }
            if (Input.Target == OpenTarget.Frame && string.IsNullOrEmpty(Input.FrameName))
                Input.Target = OpenTarget.Self;
            Input.UpdatedDate = DateTimeOffset.Now;
            var result = _carouselManager.Save(Input);
            return Json(result, "��Ŀ");
        }

        public async Task<IActionResult> OnPostUpload(IFormFile file, int sid)
        {
            if (file == null || file.Length == 0)
                return Error("����Ϊ���ļ���");

            var result = await _storageDirectory.SaveAsync(file, "sections", $"{sid}-{Cores.UnixNow.ToBase36()}.$");
            return Json(result);
        }
    }
}
