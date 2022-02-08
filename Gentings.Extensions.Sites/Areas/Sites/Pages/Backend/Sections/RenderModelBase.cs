using Gentings.Extensions.Sites.SectionRenders;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections
{
    /// <summary>
    /// �༭Ĭ�Ͻڵ㡣
    /// </summary>
    public abstract class RenderModelBase<TSection> : ModelBase where TSection : Section, new()
    {
        /// <summary>
        /// ����ģ�͡�
        /// </summary>
        [BindProperty]
        public TSection? Input { get; set; }

        private ISectionRender? _render;
        /// <summary>
        /// ��ǰ�ڵ����ʵ����
        /// </summary>
        public ISectionRender Render => _render ??= SectionManager.GetSectionRender(typeof(TSection).Name.Replace("Section", string.Empty));

        /// <summary>
        /// ��ǰ�ڵ�ʵ����
        /// </summary>
        public Section? Section { get; private set; }

        /// <summary>
        /// ��ȡ�ڵ�ʵ����
        /// </summary>
        /// <param name="id">�ڵ�Id��</param>
        /// <returns>���ر༭�ڵ�ҳ��ʵ����</returns>
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
        /// ��ѯ����ʵ����
        /// </summary>
        /// <returns>���سɹ����ص�ǰҳ��ʵ�������򷵻�NotFound��</returns>
        protected virtual bool OnFound() { return true; }

        /// <summary>
        /// ����ڵ�ʵ����
        /// </summary>
        /// <returns>���ر༭�ڵ���ҳ��ʵ����</returns>
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
        /// ����֮ǰ���½ڵ�ʵ����
        /// </summary>
        /// <param name="section">��ǰ����Ľڵ�ʵ����</param>
        /// <returns>���سɹ����ص�ǰҳ��ʵ�������򷵻ش�����Ϣ��</returns>
        protected virtual bool OnSaving(TSection section) { return true; }
    }
}
