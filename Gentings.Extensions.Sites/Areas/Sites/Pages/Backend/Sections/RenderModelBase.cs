using Gentings.Extensions.Sites.SectionRenders;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections
{
    /// <summary>
    /// 编辑默认节点。
    /// </summary>
    public abstract class RenderModelBase<TSection> : ModelBase where TSection : Section, new()
    {
        /// <summary>
        /// 输入模型。
        /// </summary>
        [BindProperty]
        public TSection? Input { get; set; }

        private ISectionRender? _render;
        /// <summary>
        /// 当前节点呈现实例。
        /// </summary>
        public ISectionRender Render => _render ??= SectionManager.GetSectionRender(typeof(TSection).Name.Replace("Section", string.Empty));

        /// <summary>
        /// 当前节点实例。
        /// </summary>
        public Section? Section { get; private set; }

        /// <summary>
        /// 获取节点实例。
        /// </summary>
        /// <param name="id">节点Id。</param>
        /// <returns>返回编辑节点页面实例。</returns>
        public IActionResult OnGet(int id)
        {
            Section = SectionManager.Find(id);
            if (Section == null || Section.RenderName != Render.Name)
                return NotFound();
            Input = Section.As<TSection>();
            if (OnFound())
                return Page();
            return NotFound();
        }

        /// <summary>
        /// 查询其他实例。
        /// </summary>
        /// <returns>返回成功返回当前页面实例，否则返回NotFound。</returns>
        protected virtual bool OnFound() { return true; }

        /// <summary>
        /// 保存节点实例。
        /// </summary>
        /// <returns>返回编辑节点结果页面实例。</returns>
        public IActionResult OnPost()
        {
            var section = SectionManager.Find(Input!.Id).As<TSection>();
            section.Style = Input.Style;
            section.Script = Input.Script;
            section.IsFluid = Input.IsFluid;
            section.DisplayName = Input.DisplayName;
            section.Disabled = Input.Disabled;
            section.DisplayMode = Input.DisplayMode;
            if (OnSaving(section))
            {
                section.UpdatedDate = DateTimeOffset.Now;
                var result = SectionManager.Save(section);
                return Json(result, section.DisplayName);
            }
            return Error();
        }

        /// <summary>
        /// 保存之前更新节点实例。
        /// </summary>
        /// <param name="section">当前保存的节点实例。</param>
        /// <returns>返回成功返回当前页面实例，否则返回错误信息。</returns>
        protected virtual bool OnSaving(TSection section) { return true; }
    }
}
