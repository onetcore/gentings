using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

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
        /// <returns>返回角色名称。</returns>
        public static string GetRoleName(this ClaimsPrincipal claims)
        {
            return claims.GetFirstValue(ClaimTypes.Role);
        }

        /// <summary>
        /// 获取当前用户的角色名称列表。
        /// </summary>
        /// <param name="claims">当前用户接口实例。</param>
        /// <returns>返回角色名称。</returns>
        public static string[] GetRoleNames(this ClaimsPrincipal claims)
        {
            return claims.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();
        }

        private static string GetFirstValue(this ClaimsPrincipal claims, string claimType)
        {
            return claims.FindFirst(claimType)?.Value;
        }

        /// <summary>
        /// 获取用户声明实例。
        /// </summary>
        /// <param name="context">当前HTTP上下文。</param>
        /// <param name="claimType">声明类型。</param>
        /// <returns>返回用户声明实例。</returns>
        public static string GetUserFirstValue(this HttpContext context, string claimType)
        {
            return context.User.FindFirst(claimType)?.Value;
        }

        /// <summary>
        /// 通过用户实例创建声明标志。
        /// </summary>
        /// <param name="user">当前用户实例。</param>
        /// <param name="authenticationScheme">验证方式。</param>
        /// <param name="action">实例化声明列表。</param>
        /// <returns>返回用户声明标志。</returns>
        public static ClaimsIdentity Create(this IUser user, string authenticationScheme, Action<List<Claim>> action = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            if (action != null) action(claims);
            return new ClaimsIdentity(claims, authenticationScheme);
        }
    }
}