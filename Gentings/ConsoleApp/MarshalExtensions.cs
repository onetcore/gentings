using System.Runtime.InteropServices;

namespace Gentings.ConsoleApp
{
    /// <summary>
    /// Marshal扩展类。
    /// </summary>
    public static class MarshalExtensions
    {
        /// <summary>
        /// 结构体转化成byte[]。
        /// </summary>
        /// <param name="structure">当前结构实例。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] ToMarshalBytes(this object structure)
        {
            var size = Marshal.SizeOf(structure);
            var buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, buffer, false);
                var bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);

                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        /// byte[]转化成结构体
        /// </summary>
        /// <typeparam name="T">当前结构类型。</typeparam>
        /// <param name="bytes">当前字节实例。</param>
        /// <returns>返回当前结构体。</returns>
        public static T ToMarshalStruct<T>(this byte[] bytes)
            where T : struct
        {
            var size = Marshal.SizeOf<T>();
            var buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return Marshal.PtrToStructure<T>(buffer);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}