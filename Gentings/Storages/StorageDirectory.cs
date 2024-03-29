﻿using Gentings.AspNetCore;
using Gentings.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Gentings.Storages
{
    /// <summary>
    /// 存储文件夹提供者实现类。
    /// </summary>
    public class StorageDirectory : IStorageDirectory
    {
        private readonly string _root;
        private readonly string _temp;
        /// <summary>
        /// 初始化类<see cref="StorageDirectory"/>。
        /// </summary>
        /// <param name="configuration">配置接口。</param>
        public StorageDirectory(IConfiguration configuration)
        {
            var path = configuration["StorageDir"]?.Trim() ?? "../storages";
            _root = path.MapPath();
            if (!Directory.Exists(_root))
            {
                Directory.CreateDirectory(_root);
            }

            _temp = Path.Combine(_root, "temp");
            if (!Directory.Exists(_temp))
            {
                Directory.CreateDirectory(_temp);
            }
        }

        /// <summary>
        /// 获取当前路径的物理路径。
        /// </summary>
        /// <param name="path">当前相对路径。</param>
        /// <returns>返回当前路径的物理路径。</returns>
        public string GetPhysicalPath(string? path = null)
        {
            if (path == null)
            {
                return _root;
            }

            path = path.Trim(' ', '~', '/', '\\');
            return Path.Combine(_root, path);
        }

        /// <summary>
        /// 获取临时目录得物理路径。
        /// </summary>
        /// <param name="fileName">文件名称。</param>
        /// <returns>返回当前临时文件物理路径。</returns>
        public string GetTempPath(string? fileName = null)
        {
            if (fileName == null)
            {
                return _temp;
            }

            return Path.Combine(_temp, fileName);
        }

        /// <summary>
        /// 获取当前路径文件的操作提供者接口实例。
        /// </summary>
        /// <param name="path">文件相对路径。</param>
        /// <returns>文件的操作提供者接口实例。</returns>
        public IStorageFile GetFile(string path)
        {
            return new StorageFile(GetPhysicalPath(path), path);
        }

        /// <summary>
        /// 将表单文件实例保存到临时文件夹中。
        /// </summary>
        /// <param name="file">表单文件实例。</param>
        /// <returns>返回文件实例。</returns>
        public virtual async Task<FileInfo> SaveToTempAsync(IFormFile file)
        {
            var tempFile = GetTempPath(Guid.NewGuid().ToString());
            await using (var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fs);
            }
            return new FileInfo(tempFile);
        }

        /// <summary>
        /// 将表单文件实例保存到临时文件夹中。
        /// </summary>
        /// <param name="file">表单文件实例。</param>
        /// <returns>返回文件实例。</returns>
        public virtual async Task<FileInfo> SaveToTempAsync(Stream file)
        {
            var tempFile = GetTempPath(Guid.NewGuid().ToString());
            await file.SaveToAsync(tempFile);
            return new FileInfo(tempFile);
        }

        /// <summary>
        /// 将Uri文件实例保存到临时文件夹中。
        /// </summary>
        /// <param name="uri">文件Uri实例。</param>
        /// <returns>返回文件实例。</returns>
        public virtual Task<FileInfo> SaveToTempAsync(Uri uri)
        {
            return HttpService.ExecuteAsync(async client =>
            {
                client.Timeout = TimeSpan.FromHours(1);
                client.DefaultRequestHeaders.Referrer =
                    new Uri($"{uri.Scheme}://{uri.DnsSafeHost}{(uri.IsDefaultPort ? null : ":" + uri.Port)}/");
                await using var stream = await client.GetStreamAsync(uri);
                return await SaveToTempAsync(stream);
            });
        }

        /// <summary>
        /// 将字符串保存到临时文件夹中。
        /// </summary>
        /// <param name="text">要保存的字符串。</param>
        /// <param name="fileName">文件名。</param>
        /// <returns>返回文件实例。</returns>
        public virtual async Task<FileInfo> SaveToTempAsync(string text, string? fileName = null)
        {
            fileName ??= Guid.NewGuid().ToString();
            fileName = GetTempPath(fileName);
            await FileHelper.SaveTextAsync(fileName, text);
            return new FileInfo(fileName);
        }

        /// <summary>
        /// 将表单文件实例保存到特定的文件夹中。
        /// </summary>
        /// <param name="file">表单文件实例。</param>
        /// <param name="directoryName">文件夹名称。</param>
        /// <param name="fileName">文件名称，如果为空，则直接使用表单实例的文件名。</param>
        /// <returns>返回文件提供者操作接口实例。</returns>
        public async Task<IStorageFile> SaveAsync(IFormFile file, string directoryName, string? fileName = null)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception(Resources.StorageDirectory_FormFileInvalid);
            }

            if (fileName == null)
                fileName = file.FileName;
            else if (fileName.EndsWith(".$"))
                fileName = fileName[0..^2] + Path.GetExtension(file.FileName);

            var path = GetPhysicalPath(directoryName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(path, fileName);
            await using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fs);
            }
            return new StorageFile(path, Path.Combine(directoryName, fileName));
        }

        /// <summary>
        /// 将字符串保存到特定的文件夹中。
        /// </summary>
        /// <param name="text">要保存的字符串。</param>
        /// <param name="directoryName">文件夹名称。</param>
        /// <param name="fileName">文件名称，如果为空，则直接使用表单实例的文件名。</param>
        /// <returns>返回文件提供者操作接口实例。</returns>
        public virtual async Task<IStorageFile> SaveAsync(string text, string directoryName, string fileName)
        {
            var path = GetPhysicalPath(directoryName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(path, fileName);
            await FileHelper.SaveTextAsync(path, text);
            return new StorageFile(path, Path.Combine(directoryName, fileName));
        }

        /// <summary>
        /// 清理空文件夹。
        /// </summary>
        public void ClearEmptyDirectories()
        {
            foreach (var info in new DirectoryInfo(_root).GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                if (info.Name.Equals("temp", StringComparison.OrdinalIgnoreCase))
                {
                    ClearTempFiles(info);
                    continue;
                }
                var directories = info.GetDirectories("*", SearchOption.AllDirectories);
                foreach (var directory in directories)
                {
                    if (!directory.Exists)
                    {
                        continue;
                    }

                    var files = directory.GetFiles("*", SearchOption.AllDirectories);
                    if (files.Length == 0)
                    {
                        try { directory.Delete(true); }
                        catch { }
                    }
                }
            }
        }

        private void ClearTempFiles(DirectoryInfo info)
        {
            var files = info.GetFiles();
            var expired = DateTime.Now.AddDays(-1);
            foreach (var file in files)
            {
                if (file.LastAccessTime < expired)
                {
                    try { file.Delete(); }
                    catch { }
                }
            }
        }

        /// <summary>
        /// 刪除文件。
        /// </summary>
        /// <param name="path">文件路径。</param>
        public void DeleteFile(string path)
        {
            path = GetPhysicalPath(path);
            if (File.Exists(path)) File.Delete(path);
        }

        /// <summary>
        /// 刪除文件夹。
        /// </summary>
        /// <param name="path">文件夹路径。</param>
        /// <param name="recursive">是否刪除文件夹以及子文件夹的所有文件。</param>
        public void DeleteDirectory(string path, bool recursive = false)
        {
            path = GetPhysicalPath(path);
            if (Directory.Exists(path)) Directory.Delete(path, recursive);
        }

        private class StorageFile : IStorageFile
        {
            private readonly FileInfo _info;
            public StorageFile(string path, string absolutePath)
            {
                _info = new FileInfo(path);
                Path = absolutePath?.Replace('\\', '/');
            }

            /// <summary>
            /// 绝对地址。
            /// </summary>
            public string? Path { get; }

            /// <summary>
            /// 访问地址。
            /// </summary>
            public string Url => $"/s-files/{Path}";

            /// <summary>
            /// 下载地址。
            /// </summary>
            public string DownloadUrl => $"/d-files/{Path}";

            /// <summary>
            /// 大小。
            /// </summary>
            public long Length => _info.Length;

            /// <summary>
            /// 包含文件夹和文件名全名。
            /// </summary>
            public string FullName => _info.FullName;

            /// <summary>
            /// 文件名称。
            /// </summary>
            public string Name => _info.Name;

            /// <summary>
            /// 扩展名称。
            /// </summary>
            public string Extension => _info.Extension;

            private string? _hashed;
            /// <summary>
            /// 文件哈希值，一般为Md5值。
            /// </summary>
            public string? Hashed
            {
                get
                {
                    if (_hashed == null && _info.Exists)
                    {
                        _hashed = _info.ComputeHash();
                    }
                    return _hashed;
                }
            }

            /// <summary>
            /// 判断是否存在。
            /// </summary>
            public bool Exists => _info.Exists;

            /// <summary>
            /// 已读取方式打开当前存储文件。
            /// </summary>
            /// <returns>返回文件流。</returns>
            public Stream OpenRead()
            {
                return _info.OpenRead();
            }

            /// <summary>
            /// 缩放图片。
            /// </summary>
            /// <param name="width">宽度。</param>
            /// <param name="height">高度。</param>
            /// <param name="path">保存路径，未指定将保存在当前文件夹中。</param>
            /// <returns>返回缩略图文件实例。</returns>
            public FileInfo Resize(int width, int height, string? path = null)
            {
                return _info.Resize(width, height, path);
            }
        }
    }
}