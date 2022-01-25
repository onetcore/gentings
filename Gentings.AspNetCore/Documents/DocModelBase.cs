using Gentings.AspNetCore;
using Gentings.Documents;
using Gentings.Documents.Markdown;
using Gentings.Documents.TableOfContent;
using Gentings.Storages;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.AspNetCore.Documents
{
    /// <summary>
    /// 文档展示页面基类。
    /// </summary>
    public abstract class DocModelBase : ModelBase
    {
        /// <summary>
        /// 文章内容。
        /// </summary>
        public string? Html { get; private set; }

        /// <summary>
        /// 当前目录实例。
        /// </summary>
        public Toc? Toc { get; private set; }

        /// <summary>
        /// 展示Markdown文档实例。
        /// </summary>
        /// <param name="url">当前文档路径。</param>
        /// <param name="culture">当前语言参数。</param>
        /// <returns>返回当前文档试图结果。</returns>
        public async Task<IActionResult> OnGetAsync(string? url, string? culture)
        {
            var path = TocPath.GetMarkdownPath(url);
            if (!TocPath.TryGetPhysicalPath(ref culture, path, out var physicalPath, out var directory))
            {
                if (IsDefaultCulture(culture))
                    return RedirectPermanent($"/docs/{url}");
                return NotFound();
            }
            else if (culture != null && culture != Culture)
                return RedirectPermanent($"/{culture}/docs/{url}");
            ViewData["IsDocs"] = true;
            ViewData["Current"] = path.Replace('/', '-');
            PageContext.AddLibraries(ImportLibrary.Highlight | ImportLibrary.GtDocs);
            await InitAsync(physicalPath);
            await InitTocAsync(directory, culture);
            return await OnAfterGettedAsync();
        }

        /// <summary>
        /// 获取所有文档之后执行的方法，提供子类重写。
        /// </summary>
        /// <returns>返回当前页面实例。</returns>
        protected virtual async Task<IActionResult> OnAfterGettedAsync() => await Task.FromResult(Page());

        private async Task InitTocAsync(DirectoryInfo directory, string? culture)
        {
            if (!TocPath.TryGetTocPhysicalPath(directory, out var file))
                return;
            Toc = await GetRequiredService<IMemoryCache>().GetOrCreateAsync(file, async ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                return await Toc.LoadAsync(file, culture);
            });
        }

        private bool TryGetTocFile(string root, DirectoryInfo directory, out string path)
        {
            path = Path.Combine(directory.FullName, "toc.yml");
            if (System.IO.File.Exists(path))
                return true;
            if (directory.Parent!.FullName.Length >= root.Length)
                return TryGetTocFile(root, directory.Parent!, out path);
            return false;
        }

        private async Task InitAsync(string physicalPath)
        {
            var source = await FileHelper.ReadTextAsync(physicalPath);
            var pipeline = MarkdownConvert.Create(MarkdownExtension.Advanced | MarkdownExtension.Bootstrap | MarkdownExtension.Yaml).Build();
            var document = Markdown.Parse(source, pipeline);
            Html = document.ToHtml(pipeline);
            if (document[0] is YamlFrontMatterBlock yaml)
            {
                foreach (StringLine line in yaml.Lines)
                {
                    if (line.Slice.Length <= 1) continue;
                    var key = line.Slice.Substring(':');
                    if (string.IsNullOrEmpty(key)) continue;
                    ViewData[key.Trim()] = line.Slice.Substring(key.Length + 1);
                }
            }
        }
    }
}