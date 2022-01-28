using Gentings.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Templates
{
    /// <summary>
    /// 模板页面模型基类。
    /// </summary>
    public abstract class TemplateModelBase : ModelBase
    {
        /// <summary>
        /// 获取模板页面。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>返回页面视图模型。</returns>
        public async Task<IActionResult> OnGetAsync(string? path = null)
        {
            if (path == null)
                path = "/";
            else
                path = path.TrimEnd('/');
            var page = await GetRequiredService<IPageManager>().FindAsync(path);
            if (page == null || page.Disabled)
                return NotFound();
            var template = GetRequiredService<ITemplateManager>().GetTemplate(page.TemplateName);
            var sections = await GetRequiredService<ISectionManager>().FetchAsync(x => x.PageId == page.Id);
            Context = new PageModelContext(page, sections, template, Settings);
            AddData("Model", Context);
            AddData("Title", page.Title);
            AddData("Keyword", page.Keyword);
            AddData("Description", page.Description);
            AddData("ClassName", page.ClassName);
            return Page();
        }

        /// <summary>
        /// 当前模型上下文。
        /// </summary>
        public PageModelContext Context { get; private set; }

        /// <summary>
        /// 添加视图数据。
        /// </summary>
        /// <param name="key">唯一键。</param>
        /// <param name="value">当前对应的值。</param>
        protected void AddData(string key, string? value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            ViewData[key] = value;
        }

        /// <summary>
        /// 添加视图数据。
        /// </summary>
        /// <param name="key">唯一键。</param>
        /// <param name="value">当前对应的值。</param>
        protected void AddData(string key, object? value)
        {
            if (value == null)
                return;
            ViewData[key] = value;
        }

        /// <summary>
        /// 网站配置。
        /// </summary>
        protected abstract SiteSettings Settings { get; }
    }
}
