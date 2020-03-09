namespace Gentings.Messages.CMPP.Packaging
{
    /// <summary>
    /// 连接状态。
    /// </summary>
    public enum ConnectStatus : uint
    {
        /// <summary>
        /// 正确。
        /// </summary>
        Ok,
        /// <summary>
        /// 消息结构错。
        /// </summary>
        InvalidPackage,
        /// <summary>
        /// 非法源地址。
        /// </summary>
        InvlaidAuthenticator,
        /// <summary>
        /// 认证错。
        /// </summary>
        Unauthorized,
        /// <summary>
        /// 版本太高。
        /// </summary>
        VersionError,
        /// <summary>
        /// 其他错误。
        /// </summary>
        Others,
    }
}
