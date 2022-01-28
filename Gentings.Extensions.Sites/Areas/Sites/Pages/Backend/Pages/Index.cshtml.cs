using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Pages
{
    /// <summary>
    /// ҳ���б�
    /// </summary>
    public class IndexModel : ModelBase
    {
        private readonly IPageManager _pageManager;

        public IndexModel(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        /// <summary>
        /// ��ѯʵ����
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public PageQuery Query { get; set; }

        /// <summary>
        /// ҳ���б�
        /// </summary>
        public IPageEnumerable<Page> Items { get; private set; }

        public void OnGet()
        {
            Items = _pageManager.Load(Query);
        }

        public IActionResult OnPostDelete(int[] id)
        {
            if (id == null || id.Length == 0)
                return Error("����ѡ��ҳ����ٽ���ɾ��������");
            var result = _pageManager.Delete(id);
            return Json(result, "ҳ��");
        }
    }
}
