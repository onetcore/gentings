using Gentings.Extensions.Sites.Sections;
using Gentings.Extensions.Sites.Sections.Defaults;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections.Defaults
{
    /// <summary>
    /// 编辑默认节点。
    /// </summary>
    public class EditModel : ModelBase
    {
        /// <summary>
        /// 输入模型。
        /// </summary>
        [BindProperty]
        public DefaultSection? Input { get; set; }

        /// <summary>
        /// 当前节点呈现实例。
        /// </summary>
        public ISectionRender? Render { get; private set; }

        /// <summary>
        /// 当前节点实例。
        /// </summary>
        public Section? Section { get; private set; }

        public IActionResult OnGet(int id)
        {
            Section = SectionManager.Find(id);
            if (Section == null || Section.SectionType != DefaultSectionRender.Default)
                return NotFound();
            Render = SectionManager.GetSectionRender(Section.SectionType);
            Input = Section.As<DefaultSection>();
            return Page();
        }

        public IActionResult OnPost()
        {
            var section = SectionManager.Find(Input!.Id).As<DefaultSection>();
            section.Html = Input.Html;
            section.Style = Input.Style;
            section.Script = Input.Script;
            var result = SectionManager.Save(section);
            if (result)
            {
                return SuccessPage("你已经成功更新了节点！", "../Index", routeValues: new { pid = section.PageId });
            }
            return ErrorPage(result.ToString("节点"));
        }
    }
}
