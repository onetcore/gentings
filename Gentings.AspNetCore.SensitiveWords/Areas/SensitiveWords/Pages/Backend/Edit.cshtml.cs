using Gentings.Extensions.SensitiveWords;
using Gentings.Security;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.SensitiveWords.Areas.SensitiveWords.Pages.Backend
{
    /// <summary>
    /// 编辑敏感词汇。
    /// </summary>
    [PermissionAuthorize(Permissions.EditSensitive)]
    public class EditModel : ModelBase
    {
        private readonly ISensitiveWordManager _sensitiveWordManager;
        public EditModel(ISensitiveWordManager sensitiveWordManager)
        {
            _sensitiveWordManager = sensitiveWordManager;
        }

        [BindProperty]
        public SensitiveWord Input { get; set; }

        public IActionResult OnGet(int id)
        {
            if (id > 0)
            {
                Input = _sensitiveWordManager.Find(id);
                if (Input == null)
                    return NotFound();
            }
            else
                Input = new SensitiveWord();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Input.Word))
            {
                ModelState.AddModelError("Input.Word", "敏感词不能为空！");
                return Error();
            }

            var result = await _sensitiveWordManager.SaveAsync(Input);
            return Json(result, Input.Word);
        }
    }
}