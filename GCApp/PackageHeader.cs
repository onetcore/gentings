using System;

namespace GCApp
{
    /// <summary>
    /// 消息包头。
    /// </summary>
    public class PackageHeader
    {
        /// <summary>
        /// 包头大小。
        /// </summary>
        public const int Size = 4 + 4 + 4;

        /// <summary>
        /// 操作码。
        /// </summary>
        public CMPPCommand CommandId { get; }

        /// <summary>
        /// 消息流水号,顺序累加,步长为1,循环使用(一对请求和应答消息的流水号必须相同)。
        /// </summary>
        public uint SequenceId { get; }

        /// <summary>
        /// 消息总长度(含消息头及消息体) 。
        /// </summary>
        public uint TotalLength { get; }

        /// <summary>
        /// 消息包头。
        /// </summary>
        /// <param name="size">消息包长度，不包含头大小 。</param>
        /// <param name="CMPPCommand">操作码。</param>
        /// <param name="sequenceId">消息流水号,顺序累加,步长为1,循环使用(一对请求和应答消息的流水号必须相同)。</param>
        public PackageHeader(uint size, CMPPCommand CMPPCommand, uint sequenceId = 1) //发送前 
        {
            CommandId = CMPPCommand;
            SequenceId = sequenceId;
            TotalLength = size + Size;
        }

        /// <summary>
        /// 从字节中读取消息包头信息。
        /// </summary>
        /// <param name="bytes">当前包实例。</param>
        public PackageHeader(byte[] bytes)
        {
            var buffer = new byte[4];
            Buffer.BlockCopy(bytes, 0, buffer, 0, buffer.Length);
            Array.Reverse(buffer);
            TotalLength = BitConverter.ToUInt32(buffer, 0);

            Buffer.BlockCopy(bytes, 4, buffer, 0, buffer.Length);
            Array.Reverse(buffer);
            CommandId = (CMPPCommand)BitConverter.ToUInt32(buffer, 0);

            Buffer.BlockCopy(bytes, 8, buffer, 0, buffer.Length);
            Array.Reverse(buffer);
            SequenceId = BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        /// 获取当前包头字节实例。
        /// </summary>
        /// <returns>返回当前包头字节实例。</returns>
        public byte[] ToBytes()
        {
            var bytes = new byte[Size];
            var buffer = BitConverter.GetBytes(TotalLength);
            Array.Reverse(buffer);
            Buffer.BlockCopy(buffer, 0, bytes, 0, 4);

            buffer = BitConverter.GetBytes((uint)CommandId);
            Array.Reverse(buffer);
            Buffer.BlockCopy(buffer, 0, bytes, 4, 4);

            buffer = BitConverter.GetBytes(SequenceId);
            Array.Reverse(buffer);
            Buffer.BlockCopy(buffer, 0, bytes, 8, 4);
            return bytes;
        }
    }
}
