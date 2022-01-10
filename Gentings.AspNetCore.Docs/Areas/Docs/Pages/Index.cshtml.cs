using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Gentings.AspNetCore.Docs.Areas.Docs.Pages
{
    public class IndexModel : ModelBase
    {
        /// <summary>
        /// 当前Markdown文档路径。
        /// </summary>
        public string? Path { get; private set; }

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
            var source = GetMarkdownSourceAsync(physicalPath);

            Path = path;
            return Page();
        }

        private bool TryGetPhysicalPath(string path, out string physicalPath)
        {
            physicalPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Docs", path);
            return System.IO.File.Exists(physicalPath);
        }

        private Task<string> GetMarkdownSourceAsync(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs, Encoding.UTF8);
            return sr.ReadToEndAsync();
        }
    }
}