using Gentings.Extensions.Sites.Sections;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections
{
    /// <summary>
    /// 编辑节点。
    /// </summary>
    public class EditModel : ModelBase
    {
        private readonly ISectionManager _sectionManager;

        public EditModel(ISectionManager sectionManager)
        {
            _sectionManager = sectionManager;
        }

        /// <summary>
        /// 输入模型。
        /// </summary>
        [BindProperty]
        public Section Input { get; set; }

        /// <summary>
        /// 当前节点类型。
        /// </summary>
        public ISection Section { get; private set; }

        public IActionResult OnGet(int id)
        {
            Input = _sectionManager.Find(id);
            if (Input == null)
                return NotFound();
            Section = _sectionManager.GetSection(Input.SectionType);
            return Page();
        }

        public IActionResult OnPost()
        {
            Section = _sectionManager.GetSection(Input.SectionType);
            Input.UpdatedDate = DateTimeOffset.Now;
            var result = _sectionManager.Save(Input);
            if (result)
                return SuccessPage("你已经成功更新了节点！", "Index", routeValues: new { pid = Input.PageId });
            return ErrorPage(result.ToString("节点"));
        }

        public IActionResult OnPostSection(string name)
        {
            var section = _sectionManager.GetSection(name);
            return Json(new { section.Html, section.Style, section.Script });
        }
    }
}
