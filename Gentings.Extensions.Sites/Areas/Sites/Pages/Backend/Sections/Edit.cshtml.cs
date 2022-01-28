using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections
{
    /// <summary>
    /// ���ýڵ㡣
    /// </summary>
    public class EditModel : ModelBase
    {
        /// <summary>
        /// ����ģ�͡�
        /// </summary>
        [BindProperty]
        public Section? Input { get; set; }

        public IActionResult OnGet(int id, int pid)
        {
            if (id > 0)
            {
                Input = SectionManager.Find(id);
                if (Input == null)
                    return NotFound();
            }
            else if (pid <= 0)
                return NotFound();
            else
                Input = new Section { PageId = pid };
            return Page();
        }

        public IActionResult OnPost()
        {
            var isValid = true;
            if (string.IsNullOrEmpty(Input!.Name))
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
                var entity = Input.Id == 0 ? Input : SectionManager.Find(Input.Id);
                if (Input.Id > 0)
                {
                    entity.Disabled = Input.Disabled;
                    entity.DisplayName = Input.DisplayName;
                    entity.Name = Input.Name;
                }
                var result = SectionManager.Save(Input);
                return Json(result, "�ڵ�");
            }

            return Page();
        }
    }
}
