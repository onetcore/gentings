using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Gentings.Data;
using System.Linq;
using System.Threading.Tasks;
using Gentings.Identity.Data;

namespace Gentings.Identity
{
    /// <summary>
    /// 用户扩展类。
    /// </summary>
    public static class SecurityExtensions
    {
        /// <summary>
        /// 获取用户实例。
        /// </summary>
        /// <param name="httpContextAccessor">当前HTTP上下文。</param>
        /// <returns>返回当前用户实例。</returns>
        public static TUser GetUser<TUser>(this IHttpContextAccessor httpContextAccessor)
            where TUser : UserBase
        {
            return httpContextAccessor.HttpContext?.GetUser<TUser>();
        }

        /// <summary>
        /// 获取用户实例。
        /// </summary>
        /// <param name="httpContext">当前HTTP上下文。</param>
        /// <returns>返回当前用户实例。</returns>
        public static TUser GetUser<TUser>(this HttpContext httpContext)
            where TUser : UserBase
        {
            return httpContext.GetOrCreate(() =>
            {
                var userId = httpContext.User.GetUserId();
                if (userId > 0)
                {
                    return httpContext.RequestServices.GetRequiredService<IDbContext<TUser>>()
                        .AsQueryable().WithNolock()
                        .Where(x => x.Id == userId)
                        .FirstOrDefault();
                }

                return null;
            });
        }

        /// <summary>
        /// 获取用户实例。
        /// </summary>
        /// <param name="httpContextAccessor">当前HTTP上下文。</param>
        /// <returns>返回当前用户实例。</returns>
        public static Task<TUser> GetUserAsync<TUser>(this IHttpContextAccessor httpContextAccessor)
            where TUser : UserBase
        {
            return httpContextAccessor.HttpContext?.GetUserAsync<TUser>();
        }

        /// <summary>
        /// 获取用户实例。
        /// </summary>
        /// <param name="httpContext">当前HTTP上下文。</param>
        /// <returns>返回当前用户实例。</returns>
        public static Task<TUser> GetUserAsync<TUser>(this HttpContext httpContext)
            where TUser : UserBase
        {
            return httpContext.GetOrCreateAsync(async () =>
            {
                var userId = httpContext.User.GetUserId();
                if (userId > 0)
                {
                    return await httpContext.RequestServices.GetRequiredService<IDbContext<TUser>>()
                        .AsQueryable().WithNolock()
                        .Where(x => x.Id == userId)
                        .FirstOrDefaultAsync();
                }

                return null;
            });
        }

        /// <summary>
        /// 获取用户的IP地址。
        /// </summary>
        /// <param name="httpContext">当前HTTP上下文。</param>
        /// <returns>返回当前用户IP地址。</returns>
        public static string GetUserAddress(this HttpContext httpContext)
        {
            var ipAddress = httpContext.Connection?.RemoteIpAddress?.ToString();
            if (ipAddress != null)
            {
                return ipAddress;
            }

            var xff = httpContext.Request.Headers["x-forwarded-for"];
            if (xff.Count > 0)
            {
                ipAddress = xff.FirstOrDefault();
                return ipAddress?.Split(':').FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 获取安全错误。
        /// </summary>
        /// <param name="describer">错误描述实例。</param>
        /// <param name="errorDescriptor">错误枚举实例。</param>
        /// <param name="args">参数。</param>
        /// <returns>返回错误实例。</returns>
        public static IdentityError SecurityError(this IdentityErrorDescriber describer, ErrorDescriptor errorDescriptor, params object[] args)
        {
            if (describer is SecurityErrorDescriptor descriptor)
            {
                return descriptor.Error(errorDescriptor, args);
            }

            return describer.DefaultError();
        }

        /// <summary>
        /// 用户被锁定。
        /// </summary>
        /// <param name="describer">错误描述实例。</param>
        /// <returns>返回错误实例。</returns>
        public static IdentityError UserLockedOut(this IdentityErrorDescriber describer)
        {
            return describer.SecurityError(ErrorDescriptor.UserLockedOut);
        }

        /// <summary>
        /// 用户不存在。
        /// </summary>
        /// <param name="describer">错误描述实例。</param>
        /// <returns>返回错误实例。</returns>
        public static IdentityError UserNotFound(this IdentityErrorDescriber describer)
        {
            return describer.SecurityError(ErrorDescriptor.UserNotFound);
        }

        /// <summary>
        /// 角色唯一键已经存在。
        /// </summary>
        /// <param name="describer">错误描述实例。</param>
        /// <param name="normalizedRoleName">角色唯一键。</param>
        /// <returns>返回错误实例。</returns>
        public static IdentityError DuplicateNormalizedRoleName(this IdentityErrorDescriber describer, string normalizedRoleName)
        {
            return describer.SecurityError(ErrorDescriptor.DuplicateNormalizedRoleName, normalizedRoleName);
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        /// <param name="result">错误实例。</param>
        /// <param name="seperator">分隔符。</param>
        /// <returns>返回错误描述信息。</returns>
        public static string ToErrorString(this IdentityResult result, string seperator = "<br/>")
        {
            return string.Join(seperator, result.Errors.Select(x => x.Description));
        }

        /// <summary>
        /// 获取或添加当前请求上下文实例。
        /// </summary>
        /// <typeparam name="TCache">当前缓存类型。</typeparam>
        /// <param name="context">HTTP上下文。</param>
        /// <param name="key">缓存键。</param>
        /// <param name="func">新添加的对象。</param>
        /// <returns>返回当前缓存对象。</returns>
        internal static TCache GetOrCreate<TCache>(this HttpContext context, object key, Func<TCache> func)
        {
            if (context.Items.TryGetValue(key, out var value) && value is TCache cache)
            {
                return cache;
            }

            cache = func();
            context.Items[key] = cache;
            return cache;
        }

        /// <summary>
        /// 获取或添加当前请求上下文实例。
        /// </summary>
        /// <typeparam name="TCache">当前缓存类型。</typeparam>
        /// <param name="context">HTTP上下文。</param>
        /// <param name="func">新添加的对象。</param>
        /// <returns>返回当前缓存对象。</returns>
        internal static TCache GetOrCreate<TCache>(this HttpContext context, Func<TCache> func)
        {
            return context.GetOrCreate(typeof(TCache), func);
        }

        /// <summary>
        /// 获取或添加当前请求上下文实例。
        /// </summary>
        /// <typeparam name="TCache">当前缓存类型。</typeparam>
        /// <param name="context">HTTP上下文。</param>
        /// <param name="key">缓存键。</param>
        /// <param name="func">新添加的对象。</param>
        /// <returns>返回当前缓存对象。</returns>
        internal static async Task<TCache> GetOrCreateAsync<TCache>(this HttpContext context, object key, Func<Task<TCache>> func)
        {
            if (context.Items.TryGetValue(key, out var value) && value is TCache cache)
            {
                return cache;
            }

            cache = await func();
            context.Items[key] = cache;
            return cache;
        }

        /// <summary>
        /// 获取或添加当前请求上下文实例。
        /// </summary>
        /// <typeparam name="TCache">当前缓存类型。</typeparam>
        /// <param name="context">HTTP上下文。</param>
        /// <param name="func">新添加的对象。</param>
        /// <returns>返回当前缓存对象。</returns>
        internal static Task<TCache> GetOrCreateAsync<TCache>(this HttpContext context, Func<Task<TCache>> func)
        {
            return context.GetOrCreateAsync(typeof(TCache), func);
        }
    }
}