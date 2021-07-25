using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gentings.Blazored.Authentication
{
    /// <summary>
    /// 验证服务。
    /// </summary>
    public interface IAuthorizedService
    {
        /// <summary>
        /// 用户登录。
        /// </summary>
        /// <param name="model">登录模型实例。</param>
        /// <returns>返回登录结果。</returns>
        Task<ServiceDataResult<SignInResult>> Login(SignInModel model);

        /// <summary>
        /// 退出登录。
        /// </summary>
        Task Logout();
    }

    /// <summary>
    /// 验证服务实现类。
    /// </summary>
    public class AuthorizedService : ServiceBase, IAuthorizedService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        /// <summary>
        /// 初始化类<see cref="AuthorizedService"/>。
        /// </summary>
        /// <param name="authenticationStateProvider">验证状态提供者实例。</param>
        /// <param name="serviceProvider">服务提供者接口。</param>
        public AuthorizedService(
            Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider authenticationStateProvider,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _authenticationStateProvider = authenticationStateProvider as AuthenticationStateProvider;
        }

        public async Task<SignUpResult> Register(SignUpModel model)
        {
            var result = await PostAsync<SignUpResult>("api/authorize/signup", model);

            return result.Data;
        }

        /// <summary>
        /// 用户登录。
        /// </summary>
        /// <param name="model">登录模型实例。</param>
        /// <returns>返回登录结果。</returns>
        public async Task<ServiceDataResult<SignInResult>> Login(SignInModel model)
        {
            var result = await PostAsync<SignInResult>("api/authorize/signin", model);
            if (result.Status)
                await JSRuntime.SetLocalStorageAsync(AuthToken, result.Data.Token);
            _authenticationStateProvider.MarkUserAsAuthenticated(model.UserName);
            return result;
        }

        /// <summary>
        /// 退出登录。
        /// </summary>
        public async Task Logout()
        {
            await JSRuntime.SetLocalStorageAsync(AuthToken);
            _authenticationStateProvider.MarkUserAsLoggedOut();
            Client.DefaultRequestHeaders.Authorization = null;
        }
    }
}