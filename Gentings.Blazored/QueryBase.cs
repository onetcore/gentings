namespace Gentings.Blazored
{
    /// <summary>
    /// 查询基类。
    /// </summary>
    public abstract class QueryBase
    {
        /// <summary>
        /// 页码。
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// 每页显示记录数。
        /// </summary>
        public int PageSize { get; set; }
    }
}