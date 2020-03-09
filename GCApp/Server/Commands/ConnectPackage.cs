using System;
using System.Runtime.InteropServices;

namespace GCApp.Server.Commands
{
    /// <summary>
    /// 连接实体。
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ConnectPackage
    {
        /// <summary>
        /// 服务提供商账号。
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string Source_Addr;

        /// <summary>
        /// 验证MD5加密串。
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] AuthenticatorSource;

        /// <summary>
        /// 版本。
        /// </summary>
        public byte Version;

        /// <summary>
        /// 时间撮。
        /// </summary>
        public uint Timestamp;
    }

    [Serializable]
    public struct ConnectPackage1
    {
        /// <summary>
        /// 服务提供商账号。
        /// </summary>
        public string Source_Addr;

        /// <summary>
        /// 验证MD5加密串。
        /// </summary>
        public byte[] AuthenticatorSource;

        /// <summary>
        /// 版本。
        /// </summary>
        public byte Version;

        /// <summary>
        /// 时间撮。
        /// </summary>
        public uint Timestamp;
    }
}
