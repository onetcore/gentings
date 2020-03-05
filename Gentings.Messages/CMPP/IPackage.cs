using Gentings.Messages.CMPP.Packages;

namespace Gentings.Messages.CMPP
{
    /// <summary>
    /// 包接口。
    /// </summary>
    public interface IPackage
    {
        /// <summary>
        /// 将包转换为字节数组。
        /// </summary>
        /// <returns>字节数组。</returns>
        byte[] ToBytes();

        /// <summary>
        /// 包头。
        /// </summary>
        PackageHeader Header { get; }
    }
}
