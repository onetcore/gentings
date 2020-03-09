namespace GCApp
{
    /// <summary>
    /// 计费用户类型。
    /// </summary>
    public enum FeeUserType : byte
    {
        /// <summary>
        ///   0:对目的终端MSISDN计费; 
        /// </summary>
        DestTerminal = 0,
        /// <summary>
        ///   1:对源终端MSISDN计费; 
        /// </summary>
        SrcTerminal = 1,
        /// <summary>
        ///   2:对SP计费; 
        /// </summary>
        SP = 2,
        /// <summary>
        ///   3:表示本字段无效,对谁计费参见FeeTerminalId字段。
        /// </summary>
        Invalid = 3,
    }
}
