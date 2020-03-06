namespace Gentings.Messages.CMPP.Packaging
{
    /// <summary>
    /// 提交短信返回的结果。
    /// </summary>
    public enum SubmitResult : uint
    {
        /// <summary>
        /// 0：正确；
        /// </summary>
        Ok,
        /// <summary>
        /// 1：消息结构错；
        /// </summary>
        InvalidPackage,
        /// <summary>
        /// 2：命令字错；
        /// </summary>
        InvalidCommandId,
        /// <summary>
        /// 3：消息序号重复；
        /// </summary>
        SequenceIdDuplicated,
        /// <summary>
        /// 4：消息长度错；
        /// </summary>
        PackageLengthError,
        /// <summary>
        /// 5：资费代码错；
        /// </summary>
        FeeCodeError,
        /// <summary>
        /// 6：超过最大信息长;
        /// </summary>
        OutofMaxLength,
        /// <summary>
        /// 7：业务代码错；
        /// </summary>
        ServiceIdError,
        /// <summary>
        /// 8：流量控制错；
        /// </summary>
        FlowError,
        /// <summary>
        /// 9：本网关不负责服务此计费号码；
        /// </summary>
        SpError,
        /// <summary>
        /// 10：Src_Id错误；
        /// </summary>
        SrcIdError,
        /// <summary>
        /// 11：Msg_src错误；
        /// </summary>
        MsgSrcError,
        /// <summary>
        /// 12：Fee_terminal_Id错误；
        /// </summary>
        FeeTerminalIdError,
        /// <summary>
        /// 13：Dest_terminal_Id错误；
        /// </summary>
        DestTerminalIdError,
    }
}
