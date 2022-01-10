namespace Gentings.Storages
{
    /// <summary>
    /// 存储文件。
    /// </summary>
    public interface IStorageFile
    {
        /// <summary>
        /// 大小。
        /// </summary>
        long Length { get; }

        /// <summary>
        /// 包含文件夹和文件名全名。
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// 绝对地址。
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 访问地址。
        /// </summary>
        string Url { get; }

        /// <summary>
        /// 下载地址。
        /// </summary>
        string DownloadUrl { get; }

        /// <summary>
        /// 文件名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 扩展名称。
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// 文件哈希值，一般为Md5值。
        /// </summary>
        string Hashed { get; }

        /// <summary>
        /// 判断是否存在。
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// 已读取方式打开当前存储文件。
        /// </summary>
        /// <returns>返回文件流。</returns>
        Stream OpenRead();

        /// <summary>
        /// 缩放图片。
        /// </summary>
        /// <param name="width">宽度。</param>
        /// <param name="height">高度。</param>
        /// <param name="path">保存路径，未指定将保存在<paramref name="info"/>得文件夹中。</param>
        /// <returns>返回缩略图文件实例。</returns>
        FileInfo Resize(int width, int height, string path = null);
    }
}