using System.Threading.Tasks;
using Gentings.Data.Initializers;

namespace Gentings.Identity.Permissions
{
    /// <summary>
    /// 权限初始化类。
    /// </summary>
    public abstract class PermissionInitializer : IInitializer
    {
        private readonly IPermissionManager _permissionManager;
        /// <summary>
        /// 初始化类<see cref="PermissionInitializer"/>。
        /// </summary>
        /// <param name="permissionManager">权限管理类。</param>
        protected PermissionInitializer(IPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        /// <summary>
        /// 优先级，越大越靠前。
        /// </summary>
        public int Priority => -1;

        /// <summary>
        /// 判断是否禁用。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        public Task<bool> IsDisabledAsync() => Task.FromResult(false);

        /// <summary>
        /// 安装时候预先执行的接口。
        /// </summary>
        /// <returns>返回执行结果。</returns>
        public Task<bool> ExecuteAsync()
        {
            return _permissionManager.RefreshOwnersAsync();
        }
    }
}