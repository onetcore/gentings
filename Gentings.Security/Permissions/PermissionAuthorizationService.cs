using Gentings.Extensions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Gentings.Security.Permissions
{
    /// <summary>
    /// 权限验证服务。
    /// </summary>
    [Suppress(typeof(Security.PermissionAuthorizationService))]
    public class PermissionAuthorizationService : Security.PermissionAuthorizationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPermissionManager _permissionManager;

        /// <summary>
        /// 初始化类<see cref="PermissionAuthorizationService"/>。
        /// </summary>
        /// <param name="httpContextAccessor">HTTP上下文访问实例。</param>
        /// <param name="permissionManager">权限管理接口。</param>
        public PermissionAuthorizationService(IHttpContextAccessor httpContextAccessor, IPermissionManager permissionManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _permissionManager = permissionManager;
        }

        /// <summary>
        /// 判断当前用户是否拥有<paramref name="permissionName"/>权限。
        /// </summary>
        /// <param name="permissionName">权限名称。</param>
        /// <returns>返回判断结果。</returns>
        public override async Task<bool> IsAuthorizedAsync(string permissionName)
        {
            var isAuthorized =
                _httpContextAccessor.HttpContext.Items[typeof(Permission) + ":" + permissionName] as bool?;
            if (isAuthorized != null)
            {
                return isAuthorized.Value;
            }

            isAuthorized = false;
            var id = _httpContextAccessor.HttpContext.User.GetUserId();
            if (id > 0)
            {
                var permission = await _permissionManager.GetUserPermissionValueAsync(id, permissionName);
                isAuthorized = permission == PermissionValue.Allow;
            }
            _httpContextAccessor.HttpContext.Items[typeof(Permission) + ":" + permissionName] = isAuthorized;
            return isAuthorized.Value;
        }

        /// <summary>
        /// 判断当前用户是否拥有<paramref name="permissionName"/>权限。
        /// </summary>
        /// <param name="permissionName">权限名称。</param>
        /// <returns>返回判断结果。</returns>
        public override bool IsAuthorized(string permissionName)
        {
            var isAuthorized =
                _httpContextAccessor.HttpContext.Items[typeof(Permission) + ":" + permissionName] as bool?;
            if (isAuthorized != null)
            {
                return isAuthorized.Value;
            }

            isAuthorized = false;
            var id = _httpContextAccessor.HttpContext.User.GetUserId();
            if (id > 0)
            {
                var permission = _permissionManager.GetUserPermissionValue(id, permissionName);
                isAuthorized = permission == PermissionValue.Allow;
            }
            _httpContextAccessor.HttpContext.Items[typeof(Permission) + ":" + permissionName] = isAuthorized;
            return isAuthorized.Value;
        }
    }
}
