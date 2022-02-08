using Gentings.Extensions.Sites.SectionRenders.Carousels;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections.Carousel
{
    /// <summary>
    /// ҳ�������Ŀ�б�
    /// </summary>
    public class IndexModel : RenderModelBase<CarouselSection>
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
        /// ҳ���б�
        /// </summary>
        public IEnumerable<SectionRenders.Carousels.Carousel>? Items { get; private set; }

        /// <summary>
        /// ��ѯ����ʵ����
        /// </summary>
        /// <returns>���سɹ����ص�ǰҳ��ʵ�������򷵻�NotFound��</returns>
        protected override bool OnFound()
        {
            Items = _carouselManager.Fetch(x => x.SectionId == Section!.Id).OrderBy(x => x.Order).ToList();
            return true;
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
