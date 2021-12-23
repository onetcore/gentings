using System.Threading.Tasks;

namespace Gentings.Security
{
    /// <summary>
    /// 权限验证服务。
    /// </summary>
    public class PermissionAuthorizationService : IPermissionAuthorizationService
    {
        /// <summary>
        /// 判断当前用户是否拥有<paramref name="permissionName"/>权限。
        /// </summary>
        /// <param name="permissionName">权限名称。</param>
        /// <returns>返回判断结果。</returns>
        public virtual Task<bool> IsAuthorizedAsync(string permissionName)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 判断当前用户是否拥有<paramref name="permissionName"/>权限。
        /// </summary>
        /// <param name="permissionName">权限名称。</param>
        /// <returns>返回判断结果。</returns>
        public virtual bool IsAuthorized(string permissionName)
        {
            return true;
        }

        /// <summary>
        /// 判断当前用户是否拥有管理员权限。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        public virtual Task<bool> IsAdministratorAsync() => IsAuthorizedAsync(CorePermissions.Administrator);

        /// <summary>
        /// 判断当前用户是否拥有管理员权限。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        public virtual bool IsAdministrator() => IsAuthorized(CorePermissions.Administrator);
    }
}