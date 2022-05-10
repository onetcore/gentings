namespace Gentings.Extensions
{
    /// <summary>
    /// Id排序实例。
    /// </summary>
    public interface IOrderable
    {
        /// <summary>
        /// 排序，越小越靠前。
        /// </summary>
        int Order { get; set; }
    }
}