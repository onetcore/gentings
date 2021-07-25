using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Gentings.Blazored.Authentication
{
    /// <summary>
    /// 验证扩展类。
    /// </summary>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// 获取声明值。
        /// </summary>
        /// <param name="state">当前验证状态。</param>
        /// <param name="type">类型。</param>
        /// <returns>返回当前声明值。</returns>
        public static string GetClaimValue(this AuthenticationState state, string type)
            => state.User.Claims.SingleOrDefault(x => x.Type == type)?.Value;

        /// <summary>
        /// 获取当前用户Id。
        /// </summary>
        /// <param name="state">当前验证状态。</param>
        /// <returns>当前用户Id。</returns>
        public static int GetUserId(this AuthenticationState state)
        {
            var value = state.GetClaimValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(value, out var id))
                return id;
            return 0;
        }

        /// <summary>
        /// 获取当前用户名称。
        /// </summary>
        /// <param name="state">当前验证状态。</param>
        /// <returns>当前用户名称。</returns>
        public static string GetUserName(this AuthenticationState state)
        {
            return state.GetClaimValue(ClaimTypes.Name);
        }

        /// <summary>
        /// 获取当前用户头像。
        /// </summary>
        /// <param name="state">当前验证状态。</param>
        /// <returns>当前用户头像。</returns>
        public static string GetAvatar(this AuthenticationState state)
        {
            return state.GetClaimValue(ClaimTypes.Uri);
        }

        /// <summary>
        /// 获取当前用户昵称。
        /// </summary>
        /// <param name="state">当前验证状态。</param>
        /// <returns>当前用户昵称。</returns>
        public static string GetNickName(this AuthenticationState state)
        {
            return state.GetClaimValue(ClaimTypes.Surname);
        }

        /// <summary>
        /// 获取当前用户电子邮件。
        /// </summary>
        /// <param name="state">当前验证状态。</param>
        /// <returns>当前用户电子邮件。</returns>
        public static string GetEmail(this AuthenticationState state)
        {
            return state.GetClaimValue(ClaimTypes.Email);
        }

        /// <summary>
        /// 获取当前用户最高级角色。
        /// </summary>
        /// <param name="state">当前验证状态。</param>
        /// <returns>当前用户最高级角色。</returns>
        public static string GetRoleName(this AuthenticationState state)
        {
            return state.GetClaimValue(ClaimTypes.PrimarySid);
        }
    }
}