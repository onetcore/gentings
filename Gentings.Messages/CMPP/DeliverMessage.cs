using System;

namespace Gentings.Messages.CMPP
{
    /// <summary>
    /// 传送返回消息。
    /// </summary>
    public class DeliverMessage
    {
        private readonly PackageHeader _header;
        private readonly ulong _msgid;
        private readonly uint _result;
        public const int Size = 8 + 4;
        /// <summary>
        /// 实例化类<see cref="DeliverMessage"/>。
        /// </summary>
        /// <param name="msgid">消息Id。</param>
        /// <param name="result">返回结果。</param>
        public DeliverMessage(ulong msgid, uint result)
        {
            _msgid = msgid;
            _result = result;
            _header = new PackageHeader(Size, OpCode.CMPP_DELIVER_RESP, 0);
        }

        /// <summary>
        /// 将包转换为字节数组。
        /// </summary>
        /// <returns>字节数组。</returns>
        public byte[] ToBytes()
        {
            int i = 0;
            byte[] bytes = new byte[PackageHeader.Size + Size];
            //header 
            var buffer = _header.ToBytes();
            Buffer.BlockCopy(buffer, 0, bytes, 0, buffer.Length);
            i += PackageHeader.Size;

            //msg_id 8 
            buffer = BitConverter.GetBytes(_msgid);
            Array.Reverse(buffer);
            buffer.CopyTo(bytes, i);

            //result 4 
            i += 8;
            buffer = BitConverter.GetBytes(_result);
            Array.Reverse(buffer);
            buffer.CopyTo(bytes, i);
            return bytes;
        }
    }
}
