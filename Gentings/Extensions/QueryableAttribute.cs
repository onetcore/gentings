namespace Gentings.Extensions
{
    /// <summary>
    /// 分页查询。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class QueryableAttribute : Attribute
    {
        /// <summary>
        /// 初始化类<see cref="QueryableAttribute"/>。
        /// </summary>
        /// <param name="alias">简写，在URL参数中使用名称。</param>
        public QueryableAttribute(string? alias = null)
        {
            Alias = alias?.Trim();
        }

        /// <summary>
        /// 简写。
        /// </summary>
        public string? Alias { get; }
    }
}