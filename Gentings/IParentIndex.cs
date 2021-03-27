namespace Gentings
{
    /// <summary>
    /// 索引接口。
    /// </summary>
    public interface IParentIndex
    {
        /// <summary>
        /// 父级Id。
        /// </summary>
        int ParentId { get; set; }

        /// <summary>
        /// 子分组Id。
        /// </summary>
        int Id { get; set; }
    }
}