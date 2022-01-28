using Gentings.Extensions.Sites.Sections.Carousels;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections.Carousel
{
    /// <summary>
    /// ҳ�������Ŀ�б�
    /// </summary>
    public class IndexModel : ModelBase
    {
        private readonly ICarouselManager _carouselManager;
        /// <summary>
        /// ��ʼ����<see cref="IndexModel"/>��
        /// </summary>
        /// <param name="carouselManager">������Ŀ����ӿڡ�</param>
        public IndexModel(ICarouselManager carouselManager)
        {
            _carouselManager = carouselManager;
        }

        /// <summary>
        /// ������ĿId��
        /// </summary>
        public int SectionId { get; private set; }

        /// <summary>
        /// ҳ���б�
        /// </summary>
        public IEnumerable<Extensions.Sites.Sections.Carousels.Carousel> Items { get; private set; }

        public IActionResult OnGet(int id)
        {
            if (id <= 0)
                return NotFound();
            SectionId = id;
            Items = _carouselManager.Fetch(x => x.SectionId == id).OrderBy(x => x.Order);
            return Page();
        }

        public IActionResult OnPostDelete(int[] id)
        {
            if (id == null || id.Length == 0)
                return Error("����ѡ����Ŀ���ٽ���ɾ��������");
            var result = _carouselManager.Delete(id);
            return Json(result, "��Ŀ");
        }
    }
}
