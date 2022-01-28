using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Pages
{
    /// <summary>
    /// �༭ҳ�档
    /// </summary>
    public class EditModel : ModelBase
    {
        private readonly IPageManager _pageManager;

        public EditModel(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        /// <summary>
        /// ����ģ�͡�
        /// </summary>
        [BindProperty]
        public Page Input { get; set; }

        public IActionResult OnGet(int id)
        {
            Input = _pageManager.Find(id);
            if (id > 0 && Input == null)
                return NotFound();
            return Page();
        }

        public IActionResult OnPost()
        {
            var isValid = true;
            if (string.IsNullOrEmpty(Input.Key))
            {
                ModelState.AddModelError("Input.Key", "Ψһ���Ʋ���Ϊ�գ�");
                isValid = false;
            }
            if (string.IsNullOrEmpty(Input.Title))
            {
                ModelState.AddModelError("Input.Title", "���ⲻ��Ϊ�գ�");
                isValid = false;
            }

            if (isValid)
            {
                if (Input.Id > 0)
                    Input.UpdatedDate = DateTimeOffset.Now;
                var result = _pageManager.Save(Input);
                if (result)
                    return SuccessPage("���Ѿ��ɹ�������ҳ�棡", "./Index");
                return ErrorPage(result.ToString("ҳ��"));
            }

            return Page();
        }
    }
}
