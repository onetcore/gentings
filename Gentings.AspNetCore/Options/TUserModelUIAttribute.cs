namespace Gentings.AspNetCore.Options
{
    /// <summary>
    /// 用户模型UI特性。
    /// </summary>
    public sealed class TUserModelUIAttribute : ModelUIAttribute
    {
        /// <summary>
        /// 初始化类<see cref="TUserModelUIAttribute"/>。
        /// </summary>
        /// <param name="implementationTemplate">实现当前特性类型的模型泛型类，泛型只能有一个用户类型。</param>
        public TUserModelUIAttribute(Type implementationTemplate) : base(implementationTemplate)
        {
        }
    }
}
