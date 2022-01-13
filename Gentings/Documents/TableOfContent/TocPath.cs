namespace Gentings.Documents.TableOfContent
{
    /// <summary>
    /// Toc路径辅助类。
    /// </summary>
    public static class TocPath
    {
        static TocPath()
        {
            RootDirectory = Path.Join(Directory.GetCurrentDirectory(), "App_Data/Docs");
        }

        private const string LanguagePrefixed = "_";

        /// <summary>
        /// 文档URL开始目录。
        /// </summary>
        public const string RootUrl = "/docs/";

        /// <summary>
        /// 文档所在的根目录。
        /// </summary>
        public static readonly string RootDirectory;

        /// <summary>
        /// 获取语言文档根目录。
        /// </summary>
        /// <param name="culture">语言实例。</param>
        /// <param name="path">返回语言文档根目录。</param>
        /// <returns>判断是否存在语言文件夹。</returns>
        public static bool TryGetCultureRoot(ref string culture, out string path)
        {
            if (culture == null)
            {
                path = RootDirectory;
                return true;
            }
            path = Path.Join(RootDirectory, LanguagePrefixed + culture);
            if (Directory.Exists(path)) return true;
            var index = culture.IndexOf('-');
            if (index != -1)
            {
                var shortName = culture.Substring(0, index);
                path = Path.Join(RootDirectory, LanguagePrefixed + shortName);
                if (Directory.Exists(path))
                {
                    culture = shortName;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取Markdown物理路径。
        /// </summary>
        /// <param name="culture">语言实例。</param>
        /// <param name="path">当前Url地址，不包含“/docs/”。</param>
        /// <param name="physicalPath">Markdown物理路径。</param>
        /// <param name="directory">Toc文件在语言包中的文件夹物理路径。</param>
        /// <returns>返回判断结果。</returns>
        public static bool TryGetPhysicalPath(ref string culture, string path, out string physicalPath, out DirectoryInfo directory)
        {
            if (TryGetCultureRoot(ref culture, out var culturePath))
            {
                physicalPath = Path.Join(culturePath, path);
                directory = new DirectoryInfo(Path.GetDirectoryName(physicalPath));//即使Markdown不存在，toc目录还是返回语言包的的目录
                return File.Exists(physicalPath);
            }

            physicalPath = null;
            directory = null;
            return false;
        }

        /// <summary>
        /// 获取URL路径对应的Markdown文档路径。
        /// </summary>
        /// <param name="path">当前链接路径地址，不包含“/docs/”。</param>
        /// <returns>返回对应的Markdown文件路径。</returns>
        public static string GetMarkdownPath(string path)
        {
            if (path != null)
            {
                if (path.EndsWith('/') || path.EndsWith('\\'))
                { path += "index.md"; }
                else
                {//目录
                    var def = path + "/index.md";
                    if (File.Exists(Path.Join(RootDirectory, def)))
                        path = def;
                }
            }
            else
            {
                path = "index.md";
            }
            if (!path.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                path += ".md";
            return path;
        }

        /// <summary>
        /// 尝试获取Toc文件物理路径。
        /// </summary>
        /// <param name="directory">和Markdown文件同级别目录。</param>
        /// <param name="physicalPath">返回的Toc物理路径。</param>
        /// <returns>返回获取结果。</returns>
        public static bool TryGetTocPhysicalPath(DirectoryInfo directory, out string physicalPath)
        {
            physicalPath = Path.Join(directory.FullName, "toc.yml");
            if (File.Exists(physicalPath))
                return true;
            if (directory.Parent!.FullName.Length >= RootDirectory.Length)
                return TryGetTocPhysicalPath(directory.Parent!, out physicalPath);
            return false;
        }

        /// <summary>
        /// 获取当前路径的URL地址。
        /// </summary>
        /// <param name="culture">语言。</param>
        /// <param name="path">路径。</param>
        /// <returns>返回URL地址。</returns>
        public static string GetUrlRoot(string culture, string path)
        {
            path = Path.GetDirectoryName(path).Replace('\\', '/') + '/';
            path = path.Substring(RootDirectory.Length);
            if (culture != null)
            {
                path = path.Replace($"/{LanguagePrefixed}{culture}/", $"/{culture}/");
                culture = $"/{culture}/";
                var index = path.IndexOf(culture, StringComparison.OrdinalIgnoreCase);
                if (index != -1)
                    path = path.Substring(index + culture.Length).ToLowerInvariant();
            }
            path = Path.Join(culture, "/docs/", path);
            return path;
        }
    }
}
