namespace Gentings.Messages.CMPP
{
    /// <summary>
    /// CMPP服务提供者。
    /// </summary>
    public interface ICMPPServiceProvider : ISingletonServices
    {
        /// <summary>
        /// 提供者名称，唯一标识。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 服务提供商的IP地址。
        /// </summary>
        string Host { get; }

        /// <summary>
        /// 端口。
        /// </summary>
        int Port { get; }

        /// <summary>
        /// 服务提供商Id。
        /// </summary>
        string Spid { get; }

        /// <summary>
        /// 密钥。
        /// </summary>
        string Password { get; }

        /// <summary>
        /// 是否禁用。
        /// </summary>
        bool Disabled { get; }
    }
}
