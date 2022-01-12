using Gentings.Documents;
using Gentings.Documents.Markdown;
using Gentings.Documents.TableOfContent;
using Gentings.Storages;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Docs.Areas.Docs.Pages
{
    public class IndexModel : ModelBase
    {
        /// <summary>
        /// 当前Markdown文档路径。
        /// </summary>
        public string? PhysicalPath { get; private set; }

        /// <summary>
        /// 文章内容。
        /// </summary>
        public string? Html { get; private set; }

        /// <summary>
        /// 当前目录实例。
        /// </summary>
        public Toc Toc { get; private set; }

        /// <summary>
        /// 展示Markdown文档实例。
        /// </summary>
        /// <param name="url">当前文档路径。</param>
        /// <returns>返回当前文档试图结果。</returns>
        public async Task<IActionResult> OnGetAsync(string? url)
        {
            if (!TryGetPhysicalPath(ref url))
                return NotFound();
            await InitAsync();
            await InitTocAsync();
            ViewData["IsDocs"] = true;
            ViewData["Current"] = url.Replace('/', '-');
            return Page();
        }

        private async Task InitTocAsync()
        {
            var root = Path.Combine(Directory.GetCurrentDirectory(), "Docs");
            var directory = new DirectoryInfo(Path.GetDirectoryName(PhysicalPath!)!);
            if (!TryGetTocFile(root, directory, out var file))
                return;
            Toc = await Toc.LoadAsync(file);
            Toc.DirectoryName = file.Substring(root.Length).Replace('\\', '/');
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

        private async Task InitAsync()
        {
            var source = await FileHelper.ReadTextAsync(PhysicalPath);
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

        private bool TryGetPhysicalPath(ref string path)
        {
            var root = Path.Combine(Directory.GetCurrentDirectory(), "Docs");
            if (path != null)
            {
                if (path.EndsWith('/') || path.EndsWith('\\'))
                { path += "index.md"; }
                else
                {//目录
                    var def = path + "/index.md";
                    if (System.IO.File.Exists(Path.Combine(root, def)))
                        path = def;
                }
            }
            else
            {
                path = "index.md";
            }
            if (!path.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                path += ".md";
            PhysicalPath = Path.Join(root, Culture, path);
            if (System.IO.File.Exists(PhysicalPath))
                return true;
            var index = Culture.IndexOf('-');
            var culture = Culture.Substring(0, index);
            PhysicalPath = Path.Join(root, culture, path);
            if (System.IO.File.Exists(PhysicalPath))
                return true;
            PhysicalPath = Path.Join(root, path);
            return System.IO.File.Exists(PhysicalPath);
        }
    }
}