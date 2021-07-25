using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Gentings.Blazored.Authentication
{
    /// <summary>
    /// 验证提供者。
    /// </summary>
    public class AuthenticationStateProvider : Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider
    {
        private readonly JSRuntime _runtime;
        /// <summary>
        /// 初始化类<see cref="AuthenticationStateProvider"/>。
        /// </summary>
        /// <param name="runtime">脚本运行时。</param>
        public AuthenticationStateProvider(JSRuntime runtime)
        {
            _runtime = runtime;
        }

        /// <summary>
        /// 获取验证状态。
        /// </summary>
        /// <returns>返回当前验证状态。</returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _runtime.GetLocalStorageAsync(ServiceBase.AuthToken);
            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt")));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var storages = GetJwtValues(jwt);
            if (storages.TryGetValue(ClaimTypes.Role, out object data))
            {
                var roles = data.ToString()?.Trim();
                if (roles!.StartsWith("["))
                {
                    var array = JsonSerializer.Deserialize<string[]>(roles);
                    foreach (var role in array)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                storages.Remove(ClaimTypes.Role);
            }

            claims.AddRange(storages.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }

        private Dictionary<string, object> GetJwtValues(string base64)
        {
            base64 = base64.Split('.')[1];
            base64 = base64.Replace('_', '/').Replace('-', '+');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return JsonSerializer.Deserialize<Dictionary<string, object>>(Convert.FromBase64String(base64));
        }

        /// <summary>
        /// 写入当前用户实例。
        /// </summary>
        /// <param name="userName">用户名称。</param>
        public void MarkUserAsAuthenticated(string userName)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName) }, "apiauth"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        /// <summary>
        /// 确认用户已经退出。
        /// </summary>
        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}