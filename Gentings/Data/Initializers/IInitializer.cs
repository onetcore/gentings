namespace Gentings.Data.Initializers
{
    /// <summary>
    /// 安装时候执行的接口。
    /// </summary>
    public interface IInitializer : IServices
    {
        /// <summary>
        /// 优先级，越大越靠前。
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 判断是否禁用。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        Task<bool> IsDisabledAsync();

        /// <summary>
        /// 安装时候预先执行的接口。
        /// </summary>
        /// <returns>返回执行结果。</returns>
        Task<bool> ExecuteAsync();
    }
}