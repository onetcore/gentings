using System.Threading.Tasks;

namespace Gentings.Security
{
    /// <summary>
    /// 权限验证服务。
    /// </summary>
    public interface IPermissionAuthorizationService : ISingletonService
    {
        /// <summary>
        /// 判断当前用户是否拥有<paramref name="permissionName"/>权限。
        /// </summary>
        /// <param name="permissionName">权限名称。</param>
        /// <returns>返回判断结果。</returns>
        Task<bool> IsAuthorizedAsync(string permissionName);

        /// <summary>
        /// 判断当前用户是否拥有<paramref name="permissionName"/>权限。
        /// </summary>
        /// <param name="permissionName">权限名称。</param>
        /// <returns>返回判断结果。</returns>
        bool IsAuthorized(string permissionName);

        /// <summary>
        /// 判断当前用户是否拥有管理员权限。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        Task<bool> IsAdministratorAsync();

        /// <summary>
        /// 判断当前用户是否拥有管理员权限。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        bool IsAdministrator();
    }
}