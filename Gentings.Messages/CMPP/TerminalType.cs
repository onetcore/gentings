namespace Gentings.Messages.CMPP
{
    /// <summary>
    /// 计费终端类型。
    /// </summary>
    public enum TerminalType : byte
    {
        /// <summary>
        /// 0:真实号码。
        /// </summary>
        RealNumber,
        /// <summary>
        /// 1:伪码。
        /// </summary>
        PseudoCode,
    }
}
