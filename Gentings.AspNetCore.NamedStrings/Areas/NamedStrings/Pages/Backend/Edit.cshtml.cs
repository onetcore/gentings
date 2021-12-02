using Gentings.Extensions.Settings;
using Gentings.Security;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.NamedStrings.Areas.NamedStrings.Pages.Backend
{
    /// <summary>
    /// 编辑字典。
    /// </summary>
    [PermissionAuthorize(Permissions.EditNamedStrings)]
    public class EditModel : ModelBase
    {
        private readonly INamedStringManager _stringManager;
        public EditModel(INamedStringManager stringManager)
        {
            _stringManager = stringManager;
        }

        [BindProperty]
        public NamedString Input { get; set; }

        public IActionResult OnGet(int id, int pid)
        {
            if (id > 0)
            {
                Input = _stringManager.Find(id);
                if (Input == null)
                    return NotFound();
            }
            else
                Input = new NamedString { ParentId = pid };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Input.Name))
            {
                ModelState.AddModelError("Input.Name", "名称不能为空！");
                return Error();
            }

            var result = await _stringManager.SaveAsync(Input);
            return Json(result, Input.Value);
        }
    }
}