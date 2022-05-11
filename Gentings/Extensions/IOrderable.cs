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

    /// <summary>
    /// 分页查询。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class QueryableAttribute : Attribute
    {

    }

    /// <summary>
    /// 排序分页查询。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OrderByAttribute : Attribute
    {

    }
}