using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections
{
    /// <summary>
    /// ��ӽڵ㡣
    /// </summary>
    public class CreateModel : ModelBase
    {
        private readonly ISectionManager _sectionManager;

        public CreateModel(ISectionManager sectionManager)
        {
            _sectionManager = sectionManager;
        }

        /// <summary>
        /// ����ģ�͡�
        /// </summary>
        [BindProperty]
        public Section Input { get; set; }

        public IActionResult OnGet(int pid)
        {
            if (pid <= 0)
                return NotFound();
            Input = new Section { PageId = pid };
            return Page();
        }

        public IActionResult OnPost()
        {
            var isValid = true;
            if (string.IsNullOrEmpty(Input.Name))
            {
                ModelState.AddModelError("Input.Name", "Ψһ���Ʋ���Ϊ�գ�");
                isValid = false;
            }

            Input.Name = Input.Name!.ToLower();
            if (string.IsNullOrEmpty(Input.DisplayName))
            {
                Input.DisplayName = Input.Name;
            }

            if (isValid)
            {
                var section = _sectionManager.GetSection(Input.SectionType);
                Input.Script = section.Script;
                Input.Html = section.Html;
                Input.Style = section.Style;
                var result = _sectionManager.Save(Input);
                return Json(result, "�ڵ�");
            }

            return Page();
        }
    }
}
