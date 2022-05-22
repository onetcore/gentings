namespace Gentings.Extensions
{
    /// <summary>
    /// 排序分页查询。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MovableAttribute : Attribute
    {
        /// <summary>
        /// 初始化类<see cref="MovableAttribute"/>。
        /// </summary>
        /// <param name="expression">是否为条件表达式。</param>
        public MovableAttribute(bool expression = false)
        {
            Expression = expression;
        }

        /// <summary>
        /// 是否为条件表达式。
        /// </summary>
        public bool Expression { get; }
    }
}