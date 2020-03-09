using System;
using System.IO;
using System.Text;

namespace GCApp.Packaging
{
    /// <summary>
    /// 消息包扩展类。
    /// </summary>
    public static class PackageExtensions
    {
        private static readonly Encoding _encoding = Encoding.GetEncoding("bg2312");
        /// <summary>
        /// 将GB2312的字符串写入当前写入器中。
        /// </summary>
        /// <param name="writer">写入器实例。</param>
        /// <param name="value">当前字符串。</param>
        /// <param name="length">长度。</param>
        public static void WriteString(this BinaryWriter writer, string value, int length)
        {
            var bytes = new byte[length];
            var buffer = _encoding.GetBytes(value);
            Buffer.BlockCopy(buffer, 0, bytes, 0, buffer.Length);
            writer.Write(bytes);
        }

        /// <summary>
        /// 写入一个字节。
        /// </summary>
        /// <param name="writer">写入器实例。</param>
        /// <param name="value">当前数值。</param>
        public static void WriteByte(this BinaryWriter writer, uint value)
        {
            writer.Write((byte)value);
        }

        /// <summary>
        /// 写入一个字节。
        /// </summary>
        /// <param name="writer">写入器实例。</param>
        /// <param name="value">当前枚举。</param>
        public static void WriteByte(this BinaryWriter writer, Enum value)
        {
            writer.Write((byte)(object)value);
        }

        /// <summary>
        /// 将字节数组转换为字符串。
        /// </summary>
        /// <param name="buffer">当前字节实例对象。</param>
        /// <returns>返回当前字符串。</returns>
        public static string ReadString(this byte[] buffer) => _encoding.GetString(buffer);

        /// <summary>
        /// 打包实例。
        /// </summary>
        /// <param name="action">打包操作方法。</param>
        /// <param name="header">标题头。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Pack(this PackageHeader header, Action<BinaryWriter> action)
        {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(header.ToBytes());
                action(writer);
                return ms.ToArray();
            }
        }
    }
}
