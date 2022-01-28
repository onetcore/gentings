using Gentings.Extensions.Sites.Sections.Carousels;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections.Carousel
{
    /// <summary>
    /// �༭������Ŀ��
    /// </summary>
    public class EditModel : ModelBase
    {
        private readonly ICarouselManager _carouselManager;

        public EditModel(ICarouselManager carouselManager)
        {
            _carouselManager = carouselManager;
        }

        /// <summary>
        /// ����ģ�͡�
        /// </summary>
        [BindProperty]
        public Extensions.Sites.Sections.Carousels.Carousel Input { get; set; }

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
            var result = _carouselManager.Save(Input);
            return Json(result, "��Ŀ");
        }
    }
}
