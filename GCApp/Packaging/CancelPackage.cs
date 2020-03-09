using GCApp;

namespace GCApp.Packaging
{
    /// <summary>
    /// 取消短信包。
    /// </summary>
    public class CancelPackage : IPackage
    {
        /// <summary>
        /// 初始化类<see cref="CancelPackage"/>。
        /// </summary>
        public CancelPackage()
        {
            Header = new PackageHeader(8, CMPPCommand.CMPP_CANCEL, 1);
        }

        /// <summary>
        /// 8 信息标识（SP想要删除的信息标识）。
        /// </summary>
        public uint MsgId { get; set; }

        /// <summary>
        /// 包头。
        /// </summary>
        public PackageHeader Header { get; }

        /// <summary>
        /// 将包转换为字节数组。
        /// </summary>
        /// <returns>字节数组。</returns>
        public byte[] ToBytes() => Header.Pack(writer => writer.Write((ulong)MsgId));
    }
}
