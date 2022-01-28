using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Pages
{
    /// <summary>
    /// 编辑页面。
    /// </summary>
    public class EditModel : ModelBase
    {
        private readonly IPageManager _pageManager;

        public EditModel(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        /// <summary>
        /// 输入模型。
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
                ModelState.AddModelError("Input.Key", "唯一名称不能为空！");
                isValid = false;
            }
            if (string.IsNullOrEmpty(Input.Title))
            {
                ModelState.AddModelError("Input.Title", "标题不能为空！");
                isValid = false;
            }

            if (isValid)
            {
                if (Input.Id > 0)
                    Input.UpdatedDate = DateTimeOffset.Now;
                var result = _pageManager.Save(Input);
                if (result)
                    return SuccessPage("你已经成功更新了页面！", "./Index");
                return ErrorPage(result.ToString("页面"));
            }

            return Page();
        }
    }
}
