using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Gentings.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Identity
{
    /// <summary>
    /// 用户管理实现类。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <typeparam name="TUserClaim">用户声明类型。</typeparam>
    /// <typeparam name="TUserLogin">用户登录类型。</typeparam>
    /// <typeparam name="TUserToken">用户标识类型。</typeparam>
    public abstract class UserManager<TUser, TUserClaim, TUserLogin, TUserToken>
        : UserManager<TUser>, IUserManager<TUser>
        where TUser : UserBase, new()
        where TUserClaim : UserClaimBase, new()
        where TUserLogin : UserLoginBase, new()
        where TUserToken : UserTokenBase, new()
    {
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 获取当前接口。
        /// </summary>
        /// <typeparam name="TService">当前服务类型。</typeparam>
        /// <returns>返回当前服务实例。</returns>
        protected TService GetService<TService>() => _serviceProvider.GetService<TService>();

        /// <summary>
        /// 获取当前接口。
        /// </summary>
        /// <typeparam name="TService">当前服务类型。</typeparam>
        /// <returns>返回当前服务实例。</returns>
        protected TService GetRequiredService<TService>() => _serviceProvider.GetRequiredService<TService>();

        private IMemoryCache _cache;
        /// <summary>
        /// 缓存实例。
        /// </summary>
        protected IMemoryCache Cache => _cache ??= GetRequiredService<IMemoryCache>();

        private SignInManager<TUser> _signInManager;
        /// <summary>
        /// 登录管理实例。
        /// </summary>
        public SignInManager<TUser> SignInManager => _signInManager ??= GetRequiredService<SignInManager<TUser>>();

        private readonly IUserStoreBase<TUser, TUserClaim, TUserLogin, TUserToken> _store;
        /// <summary>
        /// 数据库操作接口。
        /// </summary>
        protected IUserDbContext<TUser, TUserClaim, TUserLogin, TUserToken> DbContext { get; }

        private HttpContext _httpContext;
        /// <summary>
        /// 当前HTTP上下文。
        /// </summary>
        protected HttpContext HttpContext =>
            _httpContext ??= _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;

        /// <summary>
        /// 分页加载用户。
        /// </summary>
        /// <typeparam name="TQuery">查询类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <returns>返回查询分页实例。</returns>
        public virtual Task<IPageEnumerable<TUser>> LoadAsync<TQuery>(TQuery query) where TQuery : UserQuery<TUser>
        {
            return _store.UserContext.LoadAsync(query);
        }

        /// <summary>
        /// 新建用户实例（不会对密码进行加密）。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <returns>返回添加用户结果。</returns>
        public override Task<IdentityResult> CreateAsync(TUser user)
        {
            if (user.CreatedIP == null)
                user.CreatedIP = HttpContext.GetUserAddress();
            user.PasswordHash = HashPassword(user);
            return base.CreateAsync(user);
        }

        /// <summary>
        /// 新建用户实例。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <param name="password">未加密时的密码。</param>
        /// <returns>返回添加用户结果。</returns>
        public override Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            if (user.CreatedIP == null)
                user.CreatedIP = HttpContext.GetUserAddress();
            user.PasswordHash = password;
            user.PasswordHash = HashPassword(user);
            return base.CreateAsync(user);
        }

        /// <summary>
        /// 获取当前用户。
        /// </summary>
        /// <returns>返回当前用户实例。</returns>
        public TUser GetUser() => HttpContext.GetUser<TUser>();

        /// <summary>
        /// 获取当前用户。
        /// </summary>
        /// <returns>返回当前用户实例。</returns>
        public Task<TUser> GetUserAsync() => HttpContext.GetUserAsync<TUser>();

        /// <summary>
        /// 判断当前用户是否已经登录。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        public bool IsSignedIn()
        {
            return SignInManager.IsSignedIn(HttpContext.User);
        }

        /// <summary>
        /// 密码登录。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="password">密码。</param>
        /// <param name="isRemembered">是否记住登录状态。</param>
        /// <returns>返回登录结果。</returns>
        public Task<SignInResult> PasswordSignInAsync(TUser user, string password, bool isRemembered) => SignInManager.PasswordSignInAsync(user, password, isRemembered, true);

        /// <summary>
        /// 登出。
        /// </summary>
        public virtual Task SignOutAsync()
        {
            return SignInManager.SignOutAsync();
        }

        /// <summary>
        /// 修改密码。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <param name="password">原始密码。</param>
        /// <param name="newPassword">新密码。</param>
        /// <returns>返回修改结果。</returns>
        public override async Task<IdentityResult> ChangePasswordAsync(TUser user, string password, string newPassword)
        {
            password = PasswordSalt(user.NormalizedUserName, password);
            newPassword = PasswordSalt(user.NormalizedUserName, newPassword);
            return await base.ChangePasswordAsync(user, password, newPassword);
        }

        /// <summary>
        /// 重置密码。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <param name="newPassword">新密码。</param>
        /// <returns>返回修改结果。</returns>
        public virtual Task<IdentityResult> ResetPasswordAsync(TUser user, string newPassword)
        {
            return ResetPasswordAsync(user, null, newPassword);
        }

        /// <summary>
        /// 重置密码。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <param name="token">修改密码标识。</param>
        /// <param name="newPassword">新密码。</param>
        /// <returns>返回修改结果。</returns>
        public override async Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword)
        {
            if (user.PasswordHash == null || user.NormalizedUserName == null)
                user = await FindByIdAsync(user.Id);
            if (token == null)
                token = await GeneratePasswordResetTokenAsync(user);
            newPassword = PasswordSalt(user.NormalizedUserName, newPassword);
            return await base.ResetPasswordAsync(user, token, newPassword);
        }

        /// <summary>
        /// 通过用户ID更新用户列。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="fields">用户列。</param>
        /// <returns>返回更新结果。</returns>
        public virtual bool Update(int userId, object fields)
        {
            return _store.Update(userId, fields);
        }

        /// <summary>
        /// 通过用户ID更新用户列。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="fields">用户列。</param>
        /// <returns>返回更新结果。</returns>
        public virtual Task<bool> UpdateAsync(int userId, object fields)
        {
            return _store.UpdateAsync(userId, fields);
        }

        /// <summary>
        /// 分页加载用户。
        /// </summary>
        /// <typeparam name="TQuery">查询类型。</typeparam>
        /// <typeparam name="TUserModel">用户模型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <returns>返回查询分页实例。</returns>
        public virtual IPageEnumerable<TUserModel> Load<TQuery, TUserModel>(TQuery query) where TQuery : UserQuery<TUser>
        {
            return _store.UserContext.Load<TQuery, TUserModel>(query);
        }

        /// <summary>
        /// 分页加载用户。
        /// </summary>
        /// <typeparam name="TQuery">查询类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <returns>返回查询分页实例。</returns>
        public virtual IPageEnumerable<TUser> Load<TQuery>(TQuery query) where TQuery : UserQuery<TUser>
        {
            return _store.UserContext.Load(query);
        }

        /// <summary>
        /// 分页加载用户。
        /// </summary>
        /// <typeparam name="TQuery">查询类型。</typeparam>
        /// <typeparam name="TUserModel">用户模型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <returns>返回查询分页实例。</returns>
        public virtual Task<IPageEnumerable<TUserModel>> LoadAsync<TQuery, TUserModel>(TQuery query) where TQuery : UserQuery<TUser>
        {
            return _store.UserContext.LoadAsync<TQuery, TUserModel>(query);
        }

        /// <summary>
        /// 判断当前用户名称是否存在。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <returns>返回判断结果。</returns>
        public virtual IdentityResult IsDuplicated(TUser user)
        {
            return _store.IsDuplicated(user);
        }

        /// <summary>
        /// 判断当前用户名称是否存在。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <returns>返回判断结果。</returns>
        public virtual Task<IdentityResult> IsDuplicatedAsync(TUser user)
        {
            return _store.IsDuplicatedAsync(user, CancellationToken);
        }

        /// <summary>
        /// 判断当前用户名称是否存在。
        /// </summary>
        /// <param name="userId">用户实例。</param>
        /// <param name="userName">用户名称。</param>
        /// <returns>返回判断结果。</returns>
        public virtual IdentityResult IsDuplicated(int userId, string userName)
        {
            if (DbContext.UserContext.Any(x => x.Id != userId && (x.UserName == userName || x.NormalizedUserName == userName)))
                return IdentityResult.Failed(ErrorDescriber.DuplicateUserName(userName));
            return IdentityResult.Success;
        }

        /// <summary>
        /// 判断当前用户名称是否存在。
        /// </summary>
        /// <param name="userId">用户实例。</param>
        /// <param name="userName">用户名称。</param>
        /// <returns>返回判断结果。</returns>
        public virtual async Task<IdentityResult> IsDuplicatedAsync(int userId, string userName)
        {
            if (await DbContext.UserContext.AnyAsync(x => x.Id != userId && (x.UserName == userName || x.NormalizedUserName == userName)))
                return IdentityResult.Failed(ErrorDescriber.DuplicateUserName(userName));
            return IdentityResult.Success;
        }

        /// <summary>
        /// 锁定或者解锁用户。
        /// </summary>
        /// <param name="userIds">用户Id。</param>
        /// <param name="lockoutEnd">锁定截至日期。</param>
        /// <returns>返回执行结果。</returns>
        public virtual bool Lockout(int[] userIds, DateTimeOffset? lockoutEnd = null)
        {
            return DbContext.UserContext.Update(x => x.Id.Included(userIds), new { LockoutEnd = lockoutEnd });
        }

        /// <summary>
        /// 锁定或者解锁用户。
        /// </summary>
        /// <param name="userIds">用户Id。</param>
        /// <param name="lockoutEnd">锁定截至日期。</param>
        /// <returns>返回执行结果。</returns>
        public virtual Task<bool> LockoutAsync(int[] userIds, DateTimeOffset? lockoutEnd = null)
        {
            return DbContext.UserContext.UpdateAsync(x => x.Id.Included(userIds), new { LockoutEnd = lockoutEnd });
        }

        /// <summary>
        /// 删除用户。
        /// </summary>
        /// <param name="ids">用户Id。</param>
        /// <returns>返回删除结果。</returns>
        public virtual IdentityResult Delete(int[] ids)
        {
            if (DbContext.UserContext.Delete(x => x.Id.Included(ids)))
                return IdentityResult.Success;
            return IdentityResult.Failed(ErrorDescriber.DefaultError());
        }

        /// <summary>
        /// 删除用户。
        /// </summary>
        /// <param name="id">用户Id。</param>
        /// <returns>返回删除结果。</returns>
        public virtual IdentityResult Delete(int id)
        {
            if (DbContext.UserContext.Delete(id))
                return IdentityResult.Success;
            return IdentityResult.Failed(ErrorDescriber.DefaultError());
        }

        /// <summary>
        /// 删除用户。
        /// </summary>
        /// <param name="ids">用户Id。</param>
        /// <returns>返回删除结果。</returns>
        public virtual async Task<IdentityResult> DeleteAsync(int[] ids)
        {
            if (await DbContext.UserContext.DeleteAsync(x => x.Id.Included(ids)))
                return IdentityResult.Success;
            return IdentityResult.Failed(ErrorDescriber.DefaultError());
        }

        /// <summary>
        /// 删除用户。
        /// </summary>
        /// <param name="id">用户Id。</param>
        /// <returns>返回删除结果。</returns>
        public virtual async Task<IdentityResult> DeleteAsync(int id)
        {
            if (await DbContext.UserContext.DeleteAsync(id))
                return IdentityResult.Success;
            return IdentityResult.Failed(ErrorDescriber.DefaultError());
        }

        /// <summary>
        /// 通过用户Id查询用户实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回用户实例。</returns>
        public virtual Task<TUser> FindByIdAsync(int userId)
        {
            return _store.FindUserAsync(userId);
        }

        /// <summary>
        /// 通过用户Id查询用户实例。
        /// </summary>
        /// <param name="userName">用户名称。</param>
        /// <returns>返回用户实例。</returns>
        public override Task<TUser> FindByNameAsync(string userName)
        {
            return _store.FindByNameAsync(userName);
        }

        /// <summary>
        /// 通过电话号码获取用户实例。
        /// </summary>
        /// <param name="phoneNumber">电话号码。</param>
        /// <returns>返回用户实例。</returns>
        public virtual Task<TUser> FindByPhoneNumberAsync(string phoneNumber)
        {
            return _store.UserContext.FindAsync(x => x.PhoneNumber == phoneNumber);
        }

        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <returns>返回加密后得字符串。</returns>
        public virtual string HashPassword(TUser user)
        {
            var userName = user.NormalizedUserName ?? NormalizeName(user.UserName);
            var password = PasswordSalt(userName, user.PasswordHash);
            return PasswordHasher.HashPassword(user, password);
        }

        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="password">原始密码。</param>
        /// <returns>返回加密后得字符串。</returns>
        public virtual string HashPassword(string password)
        {
            return PasswordHasher.HashPassword(null, password);
        }

        /// <summary>
        /// 验证密码。
        /// </summary>
        /// <param name="hashedPassword">原始已经加密得密码。</param>
        /// <param name="providedPassword">要验证得原始密码。</param>
        /// <returns>验证结果。</returns>
        public virtual bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return PasswordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword) !=
                   PasswordVerificationResult.Failed;
        }

        /// <summary>
        /// 拼接密码。
        /// </summary>
        /// <param name="userName">当前用户名。</param>
        /// <param name="password">密码。</param>
        /// <returns>返回拼接后得字符串。</returns>
        public virtual string PasswordSalt(string userName, string password)
        {
            return $"{NormalizeName(userName)}2O.l8{password}";
        }

        /// <summary>
        /// 验证密码。
        /// </summary>
        /// <param name="user">当前用户。</param>
        /// <param name="password">当前密码。</param>
        /// <returns>返回判断结果。</returns>
        public override Task<bool> CheckPasswordAsync(TUser user, string password)
        {
            password = PasswordSalt(user.NormalizedUserName, password);
            return base.CheckPasswordAsync(user, password);
        }

        private const string TwoFactorTokenName = "TwoFactor";

        /// <summary>
        /// 二次登录验证判定。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <param name="verificationCode">验证码。</param>
        /// <returns>返回判定结果。</returns>
        public virtual Task<bool> VerifyTwoFactorTokenAsync(TUser user, string verificationCode)
        {
            return VerifyTwoFactorTokenAsync(user, Options.Tokens.AuthenticatorTokenProvider, verificationCode);
        }

        /// <summary>
        /// 加载所有用户。
        /// </summary>
        /// <param name="expression">用户条件表达式。</param>
        /// <returns>返回用户列表。</returns>
        public virtual IEnumerable<TUser> LoadUsers(Expression<Predicate<TUser>> expression = null)
        {
            return _store.UserContext.Fetch(expression);
        }

        /// <summary>
        /// 加载所有用户。
        /// </summary>
        /// <param name="expression">用户条件表达式。</param>
        /// <returns>返回用户列表。</returns>
        public virtual Task<IEnumerable<TUser>> LoadUsersAsync(Expression<Predicate<TUser>> expression = null)
        {
            return _store.UserContext.FetchAsync(expression);
        }

        /// <summary>
        /// 设置登录状态。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <returns>返回任务。</returns>
        public virtual async Task SetLoginStatusAsync(TUser user)
        {
            user.LastLoginDate = DateTimeOffset.Now;
            user.LoginIP = HttpContext.GetUserAddress();
            await UpdateAsync(user.Id, new { user.LastLoginDate, user.LoginIP });
        }

        /// <summary>
        /// 初始化类<see cref="UserManager{TUser, TUserClaim, TUserLogin, TUserToken}"/>。
        /// </summary>
        /// <param name="store">用户存储接口。</param>
        /// <param name="optionsAccessor"><see cref="T:Microsoft.AspNetCore.Identity.IdentityOptions" />实例对象。</param>
        /// <param name="passwordHasher">密码加密器接口。</param>
        /// <param name="userValidators">用户验证接口。</param>
        /// <param name="passwordValidators">密码验证接口。</param>
        /// <param name="keyNormalizer">唯一键格式化字符串。</param>
        /// <param name="errors">错误实例。</param>
        /// <param name="serviceProvider">服务提供者接口。</param>
        protected UserManager(IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider serviceProvider)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, serviceProvider, serviceProvider.GetRequiredService<ILogger<UserManager<TUser>>>())
        {
            _serviceProvider = serviceProvider;
            _store = store as IUserStoreBase<TUser, TUserClaim, TUserLogin, TUserToken>;
            DbContext = store as IUserDbContext<TUser, TUserClaim, TUserLogin, TUserToken>;
        }
    }

    /// <summary>
    /// 用户管理实现类。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <typeparam name="TRole">角色类型。</typeparam>
    /// <typeparam name="TUserClaim">用户声明类型。</typeparam>
    /// <typeparam name="TUserRole">用户角色类型。</typeparam>
    /// <typeparam name="TUserLogin">用户登录类型。</typeparam>
    /// <typeparam name="TUserToken">用户标识类型。</typeparam>
    /// <typeparam name="TRoleClaim">角色声明类型。</typeparam>
    public abstract class UserManager<TUser, TRole, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>
        : UserManager<TUser, TUserClaim, TUserLogin, TUserToken>, IUserManager<TUser, TRole>
        where TUser : UserBase, new()
        where TRole : RoleBase, new()
        where TUserClaim : UserClaimBase, new()
        where TUserRole : UserRoleBase, new()
        where TUserLogin : UserLoginBase, new()
        where TUserToken : UserTokenBase, new()
        where TRoleClaim : RoleClaimBase, new()
    {
        private readonly IUserStoreBase<TUser, TRole, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim> _store;
        /// <summary>
        /// 数据库操作接口。
        /// </summary>
        protected new IUserRoleDbContext<TUser, TRole, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim> DbContext { get; }

        /// <summary>
        /// 初始化类<see cref="UserManager{TUser, TRole, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim}"/>。
        /// </summary>
        /// <param name="store">用户存储接口。</param>
        /// <param name="optionsAccessor"><see cref="T:Microsoft.AspNetCore.Identity.IdentityOptions" />实例对象。</param>
        /// <param name="passwordHasher">密码加密器接口。</param>
        /// <param name="userValidators">用户验证接口。</param>
        /// <param name="passwordValidators">密码验证接口。</param>
        /// <param name="keyNormalizer">唯一键格式化字符串。</param>
        /// <param name="errors">错误实例。</param>
        /// <param name="serviceProvider">服务提供者接口。</param>
        protected UserManager(IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider serviceProvider)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, serviceProvider)
        {
            _store = store as IUserStoreBase<TUser, TRole, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>;
            DbContext = store as IUserRoleDbContext<TUser, TRole, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>;
        }

        /// <summary>
        /// 获取角色Id。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回角色Id集合。</returns>
        public virtual int[] GetRoleIds(int userId)
        {
            var roles = GetRoles(userId);
            return roles.Select(x => x.Id).ToArray();
        }

        /// <summary>
        /// 获取角色Id。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回角色Id集合。</returns>
        public virtual async Task<int[]> GetRoleIdsAsync(int userId)
        {
            var roles = await GetRolesAsync(userId);
            return roles.Select(x => x.Id).ToArray();
        }

        /// <summary>
        /// 获取角色列表。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回角色Id集合。</returns>
        public virtual IEnumerable<TRole> GetRoles(int userId)
        {
            return _store.GetRoles(userId);
        }

        /// <summary>
        /// 获取角色列表。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回角色Id集合。</returns>
        public virtual Task<IEnumerable<TRole>> GetRolesAsync(int userId)
        {
            return _store.GetRolesAsync(userId);
        }

        /// <summary>
        /// 获取最高级角色实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回用户实例对象。</returns>
        public virtual TRole GetMaxRole(int userId)
        {
            return GetRoles(userId).OrderByDescending(x => x.RoleLevel).FirstOrDefault();
        }

        /// <summary>
        /// 获取最高级角色实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回用户实例对象。</returns>
        public virtual async Task<TRole> GetMaxRoleAsync(int userId)
        {
            var roles = await GetRolesAsync(userId);
            return roles.OrderByDescending(x => x.RoleLevel).FirstOrDefault();
        }

        /// <summary>
        /// 将用户添加到角色中。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="roleIds">角色Id列表。</param>
        /// <returns>返回添加结果。</returns>
        public virtual bool AddUserToRoles(int userId, int[] roleIds)
        {
            return _store.AddUserToRoles(userId, roleIds);
        }

        /// <summary>
        /// 将用户添加到角色中。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="roleIds">角色Id列表。</param>
        /// <returns>返回添加结果。</returns>
        public virtual Task<bool> AddUserToRolesAsync(int userId, int[] roleIds)
        {
            return _store.AddUserToRolesAsync(userId, roleIds);
        }

        /// <summary>
        /// 设置用户角色。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="roleIds">角色Id列表。</param>
        /// <returns>返回添加结果。</returns>
        public virtual bool SetUserToRoles(int userId, int[] roleIds)
        {
            return _store.SetUserToRoles(userId, roleIds);
        }

        /// <summary>
        /// 设置用户角色。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="roleIds">角色Id列表。</param>
        /// <returns>返回设置结果。</returns>
        public virtual Task<bool> SetUserToRolesAsync(int userId, int[] roleIds)
        {
            return _store.SetUserToRolesAsync(userId, roleIds);
        }
    }
}