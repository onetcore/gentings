using System;

namespace Gentings.Messages.CMPP
{
    /// <summary>
    /// 群发短信返回的消息。
    /// </summary>
    public class SubmitMessage
    {
        /// <summary>
        /// 包大小。
        /// </summary>
        public const int Size = 8 + 4;

        /// <summary>
        /// 消息ID。
        /// </summary>
        public uint MsgId { get; }

        /// <summary>
        /// 返回结果。
        /// </summary>
        public uint Result { get; }

        /// <summary>
        /// 包头。
        /// </summary>
        public PackageHeader Header { get; }

        /// <summary>
        /// 解析提交包实例。
        /// </summary>
        /// <param name="bytes">提交返回包字节数组。</param>
        public SubmitMessage(byte[] bytes)
        {
            int i = 0;
            byte[] buffer = new byte[PackageHeader.Size];
            Buffer.BlockCopy(bytes, 0, buffer, 0, buffer.Length);
            Header = new PackageHeader(buffer);

            //Msg_Id 
            i += PackageHeader.Size;
            buffer = new byte[8];
            Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
            Array.Reverse(buffer);
            MsgId = BitConverter.ToUInt32(buffer, 0);

            //Result 
            i += 8;
            buffer = new byte[4];
            Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
            Array.Reverse(buffer);
            Result = BitConverter.ToUInt32(buffer, 0);
        }
    }

}
