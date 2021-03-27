using System;
using System.Buffers;
using System.Text;

namespace Gentings.Sockets
{
    /// <summary>
    /// 字节读取器。
    /// </summary>
    public class ByteReader
    {
        private readonly ReadOnlySequence<byte> _sequence;

        /// <summary>
        /// 初始化类<see cref="ByteReader"/>。
        /// </summary>
        /// <param name="sequence">只读字节内存片段。</param>
        public ByteReader(ReadOnlySequence<byte> sequence)
        {
            _sequence = sequence;
            Start = sequence.Start;
            End = sequence.End;
            IsEmpty = sequence.IsEmpty;
            Length = sequence.Length;
        }

        /// <summary>
        /// 是否是空值。
        /// </summary>
        public bool IsEmpty { get; }

        /// <summary>
        /// 缓存大小。
        /// </summary>
        public long Length { get; }

        /// <summary>
        /// 初始化类<see cref="ByteReader"/>，并设置<paramref name="reader"/>序列位置前进<paramref name="length"/>字节。
        /// </summary>
        /// <param name="reader">字节读取实例。</param>
        /// <param name="length">从<paramref name="reader"/>中读取字节数量。</param>
        public ByteReader(ByteReader reader, int length)
            : this(reader, (long) length)
        {
        }

        /// <summary>
        /// 初始化类<see cref="ByteReader"/>，并设置<paramref name="reader"/>序列位置前进<paramref name="length"/>字节。
        /// </summary>
        /// <param name="reader">字节读取实例。</param>
        /// <param name="length">从<paramref name="reader"/>中读取字节数量。</param>
        public ByteReader(ByteReader reader, long length)
        {
            _sequence = reader._sequence.Slice(reader.Start, length);
            Start = _sequence.Start;
            End = _sequence.End;
            IsEmpty = _sequence.IsEmpty;
            Length = _sequence.Length;
            reader.Start = reader._sequence.GetPosition(length, reader.Start);
        }

        /// <summary>
        /// 读取一个整形实例。
        /// </summary>
        /// <returns>返回读取值。</returns>
        public short ReadInt16() => ReadBuffer(2, span => span.ReadInt16());

        /// <summary>
        /// 读取一个无符号整形实例。
        /// </summary>
        /// <returns>返回读取值。</returns>
        public ushort ReadUInt16() => ReadBuffer(2, span => span.ReadUInt16());

        /// <summary>
        /// 读取一个整形实例。
        /// </summary>
        /// <returns>返回读取值。</returns>
        public int ReadInt32()
        {
            return ReadBuffer(4, span => span.ReadInt32());
        }

        /// <summary>
        /// 读取一个无符号整形实例。
        /// </summary>
        /// <returns>返回读取值。</returns>
        public uint ReadUInt32() => ReadBuffer(4, span => span.ReadUInt32());

        /// <summary>
        /// 读取一个长整形实例。
        /// </summary>
        /// <returns>返回读取值。</returns>
        public long ReadInt64() => ReadBuffer(8, span => span.ReadInt64());

        /// <summary>
        /// 读取一个无符号长整形实例。
        /// </summary>
        /// <returns>返回读取值。</returns>
        public ulong ReadUInt64() => ReadBuffer(8, span => span.ReadUInt64());

        private const byte EndChar = 0;
        /// <summary>
        /// 读取字符串。
        /// </summary>
        /// <param name="size">字节大小。</param>
        /// <param name="encoding">编码。</param>
        /// <returns>返回当前字符串实例。</returns>
        public string ReadString(int size, Encoding encoding = default)
            => ReadBuffer(size, span =>
            {
                var index = span.IndexOf(EndChar);
                if (index != -1)
                    span = span.Slice(0, index);
                return (encoding ?? Encoding.Default).GetString(span);
            });

        /// <summary>
        /// 读取字符串。
        /// </summary>
        /// <param name="size">字节大小。</param>
        /// <returns>返回当前字符串实例。</returns>
        public string ReadASCII(int size) => ReadString(size, Encoding.ASCII);

        /// <summary>
        /// 读取一个字节。
        /// </summary>
        /// <returns>返回读取结果。</returns>
        public byte ReadByte() => ReadBuffer(1, span => span[0]);

        /// <summary>
        /// 读取字节。
        /// </summary>
        /// <param name="size">当前字节数。</param>
        /// <returns>返回读取结果。</returns>
        public byte[] ReadBytes(int size) => ReadBuffer(size, span => span.ToArray());

        /// <summary>
        /// 读取当前只读片段。
        /// </summary>
        /// <param name="size">当前字节数。</param>
        /// <param name="func">转换函数。</param>
        /// <returns>返回读取结果。</returns>
        public TResult ReadBuffer<TResult>(int size, ReadOnlySpanFunc<byte, TResult> func)
        {
            var buffer = _sequence.Slice(Start, size);
            Start = _sequence.GetPosition(size, Start);
            if (buffer.First.Length == size)
                return func(buffer.First.Span);

            Span<byte> bytes = stackalloc byte[size];
            buffer.CopyTo(bytes);
            return func(bytes);
        }

        /// <summary>
        /// 起始位置。
        /// </summary>
        public SequencePosition Start { get; private set; }

        /// <summary>
        /// 结束位置。
        /// </summary>
        public SequencePosition End { get; }

        /// <summary>
        /// 判断是否结尾。
        /// </summary>
        public bool IsEnd => IsEmpty || Start.Equals(End);

        /// <summary>
        /// 显示16进制字符串。
        /// </summary>
        /// <returns>返回16进制字符串。</returns>
        public override string ToString()
        {
            return _sequence.ToArray().ToHexString();
        }
    }

    /// <summary>
    /// 只读片段代理。
    /// </summary>
    /// <typeparam name="T">片段类型。</typeparam>
    /// <typeparam name="TResult">返回值。</typeparam>
    /// <param name="span">判断实例。</param>
    /// <returns>返回执行后的结果。</returns>
    public delegate TResult ReadOnlySpanFunc<T, out TResult>(ReadOnlySpan<T> span);
}