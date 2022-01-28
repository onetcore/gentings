using Gentings.Extensions.Sites.Menus;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Menus
{
    public class EditMenuModel : ModelBase
    {
        private readonly IPageMenuManager _menuManager;

        public EditMenuModel(IPageMenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        [BindProperty]
        public PageMenu Input { get; set; }

        public IActionResult OnGet(int id, int cid)
        {
            if (id > 0)
                Input = _menuManager.Find(id);
            else if (cid > 0)
                Input = new PageMenu { CategoryId = cid };
            if (Input == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Input.Name))
            {
                ModelState.AddModelError("Input.Name", "名称不能为空！");
                return Error();
            }

            Input.Name = Input.Name.Trim();
            if (Input.Target == OpenTarget.Frame && string.IsNullOrEmpty(Input.FrameName))
                Input.Target = OpenTarget.Self;
            var result = await _menuManager.SaveAsync(Input);
            return Json(result, "菜单");
        }
    }
}