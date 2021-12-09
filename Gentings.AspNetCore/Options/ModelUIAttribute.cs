namespace Gentings.AspNetCore.Options
{
    /// <summary>
    /// 用户泛型模型特性类。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public abstract class ModelUIAttribute : Attribute
    {
        /// <summary>
        /// 初始化类<see cref="ModelUIAttribute"/>。
        /// </summary>
        /// <param name="implementationTemplate">实现当前特性类型的模型泛型类，泛型只能有一个用户类型。</param>
        public ModelUIAttribute(Type implementationTemplate)
        {
            Template = implementationTemplate;
        }

        /// <summary>
        /// 当前泛型模型类型。
        /// </summary>
        public Type Template { get; }
    }
}
