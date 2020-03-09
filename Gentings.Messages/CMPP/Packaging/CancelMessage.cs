using Gentings.Messages.CMPP.Packaging;
using System;

namespace Gentings.Messages.CMPP.Packaging
{
    /// <summary>
    /// 取消短信包。
    /// </summary>
    public class CancelMessage : IMessage
    {
        public const int Size = 4;

        /// <summary>
        /// 初始化类。
        /// </summary>
        public CancelMessage(byte[] bytes)
        {
            Header = new PackageHeader(bytes);
            var buffer = new byte[Size];
            Buffer.BlockCopy(bytes, PackageHeader.Size, buffer, 0, buffer.Length);
            SuccessId = (uint)BitConverter.ToInt32(buffer);
        }

        /// <summary>
        /// 4 成功标识。0：成功；1：失败。
        /// </summary>
        public uint SuccessId { get; set; }

        /// <summary>
        /// 包头。
        /// </summary>
        public PackageHeader Header { get; }

        /// <summary>
        /// 包头是否合法。
        /// </summary>
        /// <param name="header">发送包头实例。</param>
        /// <returns>判断包头是否合法。</returns>
        public bool IsHeader(PackageHeader header)
        {
            return header.SequenceId == Header.SequenceId && Header.CommandId == CMPPCommand.CMPP_CANCEL_RESP;
        }
    }
}
