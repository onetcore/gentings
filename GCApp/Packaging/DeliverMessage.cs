using GCApp;
using System;

namespace GCApp.Packaging
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
        /// <param name="result">
        /// 返回结果。
        ///  0：正确；
        ///  1：消息结构错；
        ///  2：命令字错；
        ///  3：消息序号重复；
        ///  4：消息长度错；
        ///  5：资费代码错；
        ///  6：超过最大信息长；
        ///  7：业务代码错；
        ///  8: 流量控制错；
        ///  9~ ：其他错误。
        /// </param>
        public DeliverMessage(ulong msgid, uint result)
        {
            _msgid = msgid;
            _result = result;
            _header = new PackageHeader(Size, CMPPCommand.CMPP_DELIVER_RESP, 0);
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
