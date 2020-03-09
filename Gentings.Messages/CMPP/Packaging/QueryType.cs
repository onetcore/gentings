namespace Gentings.Messages.CMPP.Packaging
{
    /// <summary>
    /// 查询类别。
    /// </summary>
    public enum QueryType : uint
    {
        /// <summary>
        /// 0：总数查询；
        /// </summary>
        TotalSize,
        /// <summary>
        /// 1：按业务类型查询。
        /// </summary>
        ServiceType,
    }
}
