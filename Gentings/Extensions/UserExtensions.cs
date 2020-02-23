using System.Linq;
using System.Security.Claims;

namespace Gentings.Extensions
{
    /// <summary>
    /// 用户扩展类型。
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// 获取当前登录用户的Id。
        /// </summary>
        /// <param name="claims">当前用户接口实例。</param>
        /// <returns>返回用户Id，如果未登录则返回0。</returns>
        public static int GetUserId(this ClaimsPrincipal claims)
        {
            int.TryParse(claims.GetFirstValue(ClaimTypes.NameIdentifier), out var userId);
            return userId;
        }

        /// <summary>
        /// 获取当前用户的用户名称。
        /// </summary>
        /// <param name="claims">当前用户接口实例。</param>
        /// <returns>返回用户名称，如果未登录则返回“Anonymous”。</returns>
        public static string GetUserName(this ClaimsPrincipal claims)
        {
            return claims.GetFirstValue(ClaimTypes.Name);
        }

        /// <summary>
        /// 获取当前用户的角色名称。
        /// </summary>
        /// <param name="claims">当前用户接口实例。</param>
        /// <returns>返回角色名称，如果未登录则返回“Anonymous”。</returns>
        public static string GetRoleName(this ClaimsPrincipal claims)
        {
            return claims.GetFirstValue(ClaimTypes.Role);
        }

        /// <summary>
        /// 获取当前用户的用户名称。
        /// </summary>
        /// <param name="claims">当前用户接口实例。</param>
        /// <returns>返回角色名称，如果未登录则返回“Anonymous”。</returns>
        public static string[] GetRoleNames(this ClaimsPrincipal claims)
        {
            return claims.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();
        }

        private static string GetFirstValue(this ClaimsPrincipal claims, string claimType)
        {
            return claims.FindFirst(claimType)?.Value;
        }
    }
}
