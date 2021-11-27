namespace Gentings.Storages
{
    /// <summary>
    /// 上传/下载结果。
    /// </summary>
    public class MediaResult
    {
        /// <summary>
        /// 初始化类<see cref="MediaResult"/>。
        /// </summary>
        /// <param name="url">文件访问的URL地址。</param>
        /// <param name="message">错误消息。</param>
        internal MediaResult(string url, string message = null)
        {
            Url = url;
            Message = message;
        }

        /// <summary>
        /// 文件访问的URL地址。
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// 错误消息。
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 隐式转换为布尔类型。
        /// </summary>
        /// <param name="result">媒体结果实例。</param>
        public static implicit operator bool(MediaResult result)
        {
            if (result == null) return false;
            return result.Message == null;
        }

        /// <summary>
        /// 隐式将字符串转换为上传/下载结果实例。
        /// </summary>
        /// <param name="message">错误消息。</param>
        public static implicit operator MediaResult(string message) => new MediaResult(null, message);
    }
}