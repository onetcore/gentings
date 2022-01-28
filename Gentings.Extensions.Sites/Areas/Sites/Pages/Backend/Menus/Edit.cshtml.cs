using System.Text.RegularExpressions;
using Gentings.Extensions.Sites.Menus;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Menus
{
    public class EditModel : ModelBase
    {
        private readonly IMenuCategoryManager _categoryManager;

        public EditModel(IMenuCategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        [BindProperty]
        public MenuCategory Input { get; set; }

        public void OnGet(int id)
        {
            Input = _categoryManager.Find(id);
        }

        private static readonly Regex _regex = new Regex("[a-z\\-]+", RegexOptions.IgnoreCase);
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Input.Name))
            {
                ModelState.AddModelError("Input.Name", "名称不能为空！");
                return Error();
            }

            Input.Name = Input.Name.Trim();
            if (!_regex.IsMatch(Input.Name))
            {
                ModelState.AddModelError("Input.Name", "名称只能为英文字母和“-”字符！");
                return Error();
            }
            Input.DisplayName ??= Input.Name;
            var result = await _categoryManager.SaveAsync(Input);
            return Json(result, "分类");
        }
    }
}