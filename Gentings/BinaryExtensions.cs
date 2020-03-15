using System;
using System.Buffers.Binary;

namespace Gentings
{
    /// <summary>
    /// Span操作扩展类。
    /// </summary>
    public static class BinaryExtensions
    {
        /// <summary>
        /// 读取数值。
        /// </summary>
        /// <param name="buffer">当前只读字节实例。</param>
        /// <returns>返回数值。</returns>
        public static byte ReadByte(this ReadOnlySpan<byte> buffer)
        {
            return buffer[0];
        }

        /// <summary>
        /// 读取数值。
        /// </summary>
        /// <param name="buffer">当前只读字节实例。</param>
        /// <returns>返回数值。</returns>
        public static short ReadInt16(this ReadOnlySpan<byte> buffer)
        {
            if (BitConverter.IsLittleEndian)
                return BinaryPrimitives.ReadInt16BigEndian(buffer);
            return BinaryPrimitives.ReadInt16LittleEndian(buffer);
        }

        /// <summary>
        /// 读取无符号数值。
        /// </summary>
        /// <param name="buffer">当前只读字节实例。</param>
        /// <returns>返回无符号数值。</returns>
        public static ushort ReadUInt16(this ReadOnlySpan<byte> buffer)
        {
            if (BitConverter.IsLittleEndian)
                return BinaryPrimitives.ReadUInt16BigEndian(buffer);
            return BinaryPrimitives.ReadUInt16LittleEndian(buffer);
        }

        /// <summary>
        /// 读取数值。
        /// </summary>
        /// <param name="buffer">当前只读字节实例。</param>
        /// <returns>返回数值。</returns>
        public static int ReadInt32(this ReadOnlySpan<byte> buffer)
        {
            if (BitConverter.IsLittleEndian)
                return BinaryPrimitives.ReadInt32BigEndian(buffer);
            return BinaryPrimitives.ReadInt32LittleEndian(buffer);
        }

        /// <summary>
        /// 读取无符号数值。
        /// </summary>
        /// <param name="buffer">当前只读字节实例。</param>
        /// <returns>返回无符号数值。</returns>
        public static uint ReadUInt32(this ReadOnlySpan<byte> buffer)
        {
            if (BitConverter.IsLittleEndian)
                return BinaryPrimitives.ReadUInt32BigEndian(buffer);
            return BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        }

        /// <summary>
        /// 读取数值。
        /// </summary>
        /// <param name="buffer">当前只读字节实例。</param>
        /// <returns>返回数值。</returns>
        public static long ReadInt64(this ReadOnlySpan<byte> buffer)
        {
            if (BitConverter.IsLittleEndian)
                return BinaryPrimitives.ReadInt64BigEndian(buffer);
            return BinaryPrimitives.ReadInt64LittleEndian(buffer);
        }

        /// <summary>
        /// 读取无符号数值。
        /// </summary>
        /// <param name="buffer">当前只读字节实例。</param>
        /// <returns>返回无符号数值。</returns>
        public static ulong ReadUInt64(this ReadOnlySpan<byte> buffer)
        {
            if (BitConverter.IsLittleEndian)
                return BinaryPrimitives.ReadUInt64BigEndian(buffer);
            return BinaryPrimitives.ReadUInt64LittleEndian(buffer);
        }

        /// <summary>
        /// 写入数值。
        /// </summary>
        /// <param name="buffer">当前只读字节实例。</param>
        /// <param name="value">写入值。</param>
        public static void WriteInt16(ref this Span<byte> buffer, short value)
        {
            if (BitConverter.IsLittleEndian)
                BinaryPrimitives.WriteInt16LittleEndian(buffer, value);
            BinaryPrimitives.WriteInt16BigEndian(buffer, value);
        }

        /// <summary>
        /// 写入无符号数值。
        /// </summary>
        /// <param name="buffer">当前只读字节实例。</param>
        /// <param name="value">写入值。</param>
        public static void WriteUInt16(this Span<byte> buffer, ushort value)
        {
            if (BitConverter.IsLittleEndian)
                BinaryPrimitives.WriteUInt16LittleEndian(buffer, value);
            BinaryPrimitives.WriteUInt16BigEndian(buffer, value);
        }

        /// <summary>
        /// 写入数值。
        /// </summary>
        /// <param name="buffer">当前只读字节实例。</param>
        /// <param name="value">写入值。</param>
        public static void WriteInt32(this Span<byte> buffer, int value)
        {
            if (BitConverter.IsLittleEndian)
                BinaryPrimitives.WriteInt32LittleEndian(buffer, value);
            BinaryPrimitives.WriteInt32BigEndian(buffer, value);
        }

        /// <summary>
        /// 写入无符号数值。
        /// </summary>
        /// <param name="buffer">当前只读字节实例。</param>
        /// <param name="value">写入值。</param>
        public static void WriteUInt32(this Span<byte> buffer, uint value)
        {
            if (BitConverter.IsLittleEndian)
                BinaryPrimitives.WriteUInt32LittleEndian(buffer, value);
            BinaryPrimitives.WriteUInt32BigEndian(buffer, value);
        }

        /// <summary>
        /// 写入数值。
        /// </summary>
        /// <param name="buffer">当前只读字节实例。</param>
        /// <param name="value">写入值。</param>
        public static void WriteInt64(this Span<byte> buffer, long value)
        {
            if (BitConverter.IsLittleEndian)
                BinaryPrimitives.WriteInt64LittleEndian(buffer, value);
            BinaryPrimitives.WriteInt64BigEndian(buffer, value);
        }

        /// <summary>
        /// 写入无符号数值。
        /// </summary>
        /// <param name="buffer">当前只读字节实例。</param>
        /// <param name="value">写入值。</param>
        public static void WriteUInt64(this Span<byte> buffer, ulong value)
        {
            if (BitConverter.IsLittleEndian)
                BinaryPrimitives.WriteUInt64LittleEndian(buffer, value);
            BinaryPrimitives.WriteUInt64BigEndian(buffer, value);
        }
    }
}