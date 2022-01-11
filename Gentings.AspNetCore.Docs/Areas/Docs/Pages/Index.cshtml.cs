using Gentings.Documents.Markdown;
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
        public string? Path { get; private set; }

        /// <summary>
        /// 文章内容。
        /// </summary>
        public string? Html { get; private set; }

        /// <summary>
        /// 展示Markdown文档实例。
        /// </summary>
        /// <param name="path">当前文档路径。</param>
        /// <returns>返回当前文档试图结果。</returns>
        public async Task<IActionResult> OnGetAsync(string? path)
        {
            path ??= "index";
            if (!path.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                path += ".md";
            if (!TryGetPhysicalPath(path, out var physicalPath))
                return NotFound();
            Path = path;
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
            ViewData["IsDocs"] = true;
            ViewData["Current"] = path.Replace('/', '-');
            return Page();
        }

        private bool TryGetPhysicalPath(string path, out string physicalPath)
        {
            physicalPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Docs", path);
            return System.IO.File.Exists(physicalPath);
        }
    }
}