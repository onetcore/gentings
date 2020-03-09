using GCApp.Packaging;

namespace GCApp
{
    /// <summary>
    /// 返回消息接口。
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 包头。
        /// </summary>
        PackageHeader Header { get; }

        /// <summary>
        /// 包头是否合法。
        /// </summary>
        /// <param name="header">发送包头实例。</param>
        /// <returns>判断包头是否合法。</returns>
        bool IsHeader(PackageHeader header);
    }
}
