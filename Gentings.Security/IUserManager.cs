﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gentings.Extensions;
using Microsoft.AspNetCore.Http;

namespace Gentings.Security
{
    /// <summary>
    /// 用户管理接口。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    public interface IUserManager<TUser>
        where TUser : UserBase, new()
    {
        /// <summary>
        /// 登录管理实例。
        /// </summary>
        SignInManager<TUser> SignInManager { get; }

        /// <summary>
        /// 通过用户Id查询用户实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回用户实例。</returns>
        Task<TUser> FindByIdAsync(int userId);

        /// <summary>
        /// 通过用户Id查询用户实例。
        /// </summary>
        /// <param name="userName">用户名称。</param>
        /// <returns>返回用户实例。</returns>
        Task<TUser> FindByNameAsync(string userName);

        /// <summary>
        /// 通过电话号码获取用户实例。
        /// </summary>
        /// <param name="phoneNumber">电话号码。</param>
        /// <returns>返回用户实例。</returns>
        Task<TUser> FindByPhoneNumberAsync(string phoneNumber);

        /// <summary>
        /// 通过电子邮件获取用户实例。
        /// </summary>
        /// <param name="email">电子邮件。</param>
        /// <returns>返回用户实例。</returns>
        Task<TUser> FindByEmailAsync(string email);

        /// <summary>
        /// 正常实例化键。
        /// </summary>
        /// <param name="userName">用户名称。</param>
        /// <returns>返回正常化后的字符串。</returns>
        string NormalizeName(string userName);

        /// <summary>
        /// 正常实例化键。
        /// </summary>
        /// <param name="email">电子邮件。</param>
        /// <returns>返回正常化后的字符串。</returns>
        string NormalizeEmail(string email);

        /// <summary>
        /// 获取当前用户。
        /// </summary>
        /// <returns>返回当前用户实例。</returns>
        TUser GetUser();

        /// <summary>
        /// 获取当前用户。
        /// </summary>
        /// <returns>返回当前用户实例。</returns>
        Task<TUser> GetUserAsync();

        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <returns>返回加密后得字符串。</returns>
        string HashPassword(TUser user);

        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="password">原始密码。</param>
        /// <returns>返回加密后得字符串。</returns>
        string HashPassword(string password);

        /// <summary>
        /// 验证密码。
        /// </summary>
        /// <param name="hashedPassword">原始已经加密得密码。</param>
        /// <param name="providedPassword">要验证得原始密码。</param>
        /// <returns>验证结果。</returns>
        bool VerifyHashedPassword(string hashedPassword, string providedPassword);

        /// <summary>
        /// 拼接密码。
        /// </summary>
        /// <param name="userName">当前用户名。</param>
        /// <param name="password">密码。</param>
        /// <returns>返回拼接后得字符串。</returns>
        string PasswordSalt(string userName, string password);

        /// <summary>
        /// 密码登录。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="password">密码。</param>
        /// <param name="isRemembered">是否记住登录状态。</param>
        /// <returns>返回登录结果。</returns>
        Task<SignInResult> PasswordSignInAsync(TUser user, string password, bool isRemembered);

        /// <summary>
        /// 修改密码。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <param name="password">原始密码。</param>
        /// <param name="newPassword">新密码。</param>
        /// <returns>返回修改结果。</returns>
        Task<IdentityResult> ChangePasswordAsync(TUser user, string password, string newPassword);

        /// <summary>
        /// 添加密码。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <param name="password">密码。</param>
        /// <returns>返回添加结果。</returns>
        Task<IdentityResult> AddPasswordAsync(TUser user, string password);

        /// <summary>
        /// 重置密码。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <param name="newPassword">新密码。</param>
        /// <returns>返回修改结果。</returns>
        Task<IdentityResult> ResetPasswordAsync(TUser user, string newPassword);

        /// <summary>
        /// 重置密码。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <param name="token">修改密码标识。</param>
        /// <param name="newPassword">新密码。</param>
        /// <returns>返回修改结果。</returns>
        Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword);

        /// <summary>
        /// 生成一个密码重置的标识。
        /// </summary>
        /// <param name="user">用户是对象。</param>
        /// <returns>返回密码重置的标识。</returns>
        Task<string> GeneratePasswordResetTokenAsync(TUser user);

        /// <summary>
        /// 通过用户ID更新用户列。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="fields">用户列。</param>
        /// <returns>返回更新结果。</returns>
        bool Update(int userId, object fields);

        /// <summary>
        /// 通过用户ID更新用户列。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="fields">用户列。</param>
        /// <returns>返回更新结果。</returns>
        Task<bool> UpdateAsync(int userId, object fields);

        /// <summary>
        /// 更新用户列。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <returns>返回更新结果。</returns>
        Task<IdentityResult> UpdateAsync(TUser user);

        /// <summary>
        /// 分页加载用户。
        /// </summary>
        /// <typeparam name="TQuery">查询类型。</typeparam>
        /// <typeparam name="TUserModel">用户模型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <returns>返回查询分页实例。</returns>
        IPageEnumerable<TUserModel> Load<TQuery, TUserModel>(TQuery query) where TQuery : UserQuery<TUser>;

        /// <summary>
        /// 分页加载用户。
        /// </summary>
        /// <typeparam name="TQuery">查询类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <returns>返回查询分页实例。</returns>
        IPageEnumerable<TUser> Load<TQuery>(TQuery query) where TQuery : UserQuery<TUser>;

        /// <summary>
        /// 分页加载用户。
        /// </summary>
        /// <typeparam name="TQuery">查询类型。</typeparam>
        /// <typeparam name="TUserModel">用户模型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <returns>返回查询分页实例。</returns>
        Task<IPageEnumerable<TUserModel>> LoadAsync<TQuery, TUserModel>(TQuery query) where TQuery : UserQuery<TUser>;

        /// <summary>
        /// 分页加载用户。
        /// </summary>
        /// <typeparam name="TQuery">查询类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <returns>返回查询分页实例。</returns>
        Task<IPageEnumerable<TUser>> LoadAsync<TQuery>(TQuery query) where TQuery : UserQuery<TUser>;

        /// <summary>
        /// 新建用户实例（不会对密码进行加密）。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <returns>返回添加用户结果。</returns>
        Task<IdentityResult> CreateAsync(TUser user);

        /// <summary>
        /// 新建用户实例。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <param name="password">未加密时的密码。</param>
        /// <returns>返回添加用户结果。</returns>
        Task<IdentityResult> CreateAsync(TUser user, string password);

        /// <summary>
        /// 新建用户实例。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <returns>返回添加用户结果。</returns>
        Task<IdentityResult> DeleteAsync(TUser user);

        /// <summary>
        /// 判断当前用户名称是否存在。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <returns>返回判断结果。</returns>
        IdentityResult IsDuplicated(TUser user);

        /// <summary>
        /// 判断当前用户名称是否存在。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <returns>返回判断结果。</returns>
        Task<IdentityResult> IsDuplicatedAsync(TUser user);

        /// <summary>
        /// 判断当前用户名称是否存在。
        /// </summary>
        /// <param name="userId">用户实例。</param>
        /// <param name="userName">用户名称。</param>
        /// <returns>返回判断结果。</returns>
        IdentityResult IsDuplicated(int userId, string userName);

        /// <summary>
        /// 判断当前用户名称是否存在。
        /// </summary>
        /// <param name="userId">用户实例。</param>
        /// <param name="userName">用户名称。</param>
        /// <returns>返回判断结果。</returns>
        Task<IdentityResult> IsDuplicatedAsync(int userId, string userName);

        /// <summary>
        /// 锁定或者解锁用户。
        /// </summary>
        /// <param name="userIds">用户Id。</param>
        /// <param name="lockoutEnd">锁定截至日期。</param>
        /// <returns>返回执行结果。</returns>
        bool Lockout(int[] userIds, DateTimeOffset? lockoutEnd = null);

        /// <summary>
        /// 锁定或者解锁用户。
        /// </summary>
        /// <param name="userIds">用户Id。</param>
        /// <param name="lockoutEnd">锁定截至日期。</param>
        /// <returns>返回执行结果。</returns>
        Task<bool> LockoutAsync(int[] userIds, DateTimeOffset? lockoutEnd = null);

        /// <summary>
        /// 删除用户。
        /// </summary>
        /// <param name="ids">用户Id。</param>
        /// <returns>返回删除结果。</returns>
        IdentityResult Delete(int[] ids);

        /// <summary>
        /// 删除用户。
        /// </summary>
        /// <param name="id">用户Id。</param>
        /// <returns>返回删除结果。</returns>
        IdentityResult Delete(int id);

        /// <summary>
        /// 删除用户。
        /// </summary>
        /// <param name="ids">用户Id。</param>
        /// <returns>返回删除结果。</returns>
        Task<IdentityResult> DeleteAsync(int[] ids);

        /// <summary>
        /// 删除用户。
        /// </summary>
        /// <param name="id">用户Id。</param>
        /// <returns>返回删除结果。</returns>
        Task<IdentityResult> DeleteAsync(int id);

        /// <summary>
        /// 添加用户社会化登录信息。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="login">用户登录信息。</param>
        /// <returns>返回添加结果。</returns>
        Task<IdentityResult> AddLoginAsync(TUser user, UserLoginInfo login);

        /// <summary>
        /// 生成电子邮件确认码。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <returns>返回当前确认码。</returns>
        Task<string> GenerateEmailConfirmationTokenAsync(TUser user);

        /// <summary>
        /// 验证密码。
        /// </summary>
        /// <param name="user">当前用户。</param>
        /// <param name="password">当前密码。</param>
        /// <returns>返回判断结果。</returns>
        Task<bool> CheckPasswordAsync(TUser user, string password);

        /// <summary>
        /// 获取登录信息。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <returns>返回登录信息列表。</returns>
        Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user);

        /// <summary>
        /// 生成二次验证密钥。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <param name="tokenProvider">标识提供者。</param>
        /// <returns>返回生成的密钥。</returns>
        Task<string> GenerateTwoFactorTokenAsync(TUser user, string tokenProvider);

        /// <summary>
        /// 获取验证密钥。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <returns>返回密钥。</returns>
        Task<string> GetAuthenticatorKeyAsync(TUser user);

        /// <summary>
        /// 二次登录验证判定。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <param name="verificationCode">验证码。</param>
        /// <returns>返回判定结果。</returns>
        Task<bool> VerifyTwoFactorTokenAsync(TUser user, string verificationCode);

        /// <summary>
        /// 计算二次登录验码数量。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <returns>返回计算结果。</returns>
        Task<int> CountRecoveryCodesAsync(TUser user);

        /// <summary>
        /// 生成二次登录验证码。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="count">生成数量。</param>
        /// <returns>返回生成的验证码。</returns>
        Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(TUser user, int count);

        /// <summary>
        /// 重置用的验证密钥。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <returns>返回重置结果。</returns>
        Task<IdentityResult> ResetAuthenticatorKeyAsync(TUser user);

        /// <summary>
        /// 移除登录信息。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="loginProvider">登录提供者。</param>
        /// <param name="providerKey">提供者密钥。</param>
        /// <returns>返回移除结果。</returns>
        Task<IdentityResult> RemoveLoginAsync(TUser user, string loginProvider, string providerKey);

        /// <summary>
        /// 确认邮件地址。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="token">确认邮件标识。</param>
        /// <returns>返回确认结果。</returns>
        Task<IdentityResult> ConfirmEmailAsync(TUser user, string token);

        /// <summary>
        /// 生成修改电子邮件确认标识。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="newEmail">新电子邮件。</param>
        /// <returns>返回生成的标识。</returns>
        Task<string> GenerateChangeEmailTokenAsync(TUser user, string newEmail);

        /// <summary>
        /// 修改电子邮件。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="newEmail">新电子邮件。</param>
        /// <param name="token">确认标识。</param>
        /// <returns>返回修改结果。</returns>
        Task<IdentityResult> ChangeEmailAsync(TUser user, string newEmail, string token);

        /// <summary>
        /// 修改电话号码。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="phoneNumber">电话号码。</param>
        /// <param name="token">确认标识。</param>
        /// <returns>返回修改结果。</returns>
        Task<IdentityResult> ChangePhoneNumberAsync(TUser user, string phoneNumber, string token);

        /// <summary>
        /// 生成修改电话号码确认标识。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="phoneNumber">新电话号码。</param>
        /// <returns>返回生成的标识。</returns>
        Task<string> GenerateChangePhoneNumberTokenAsync(TUser user, string phoneNumber);

        /// <summary>
        /// 验证修改电话号码标识。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="token">确认标识。</param>
        /// <param name="phoneNumber">电话号码。</param>
        /// <returns>返回验证结果。</returns>
        Task<bool> VerifyChangePhoneNumberTokenAsync(TUser user, string token, string phoneNumber);

        /// <summary>
        /// 获取验证标识。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="loginProvider">登录提供者。</param>
        /// <param name="tokenName">标识名称。</param>
        /// <returns>返回验证标识。</returns>
        Task<string> GetAuthenticationTokenAsync(TUser user, string loginProvider, string tokenName);

        /// <summary>
        /// 设置验证标识。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="loginProvider">登录提供者。</param>
        /// <param name="tokenName">标识名称。</param>
        /// <param name="tokenValue">标识值。</param>
        /// <returns>返回操作结果。</returns>
        Task<IdentityResult> SetAuthenticationTokenAsync(TUser user, string loginProvider, string tokenName,
            string tokenValue);

        /// <summary>
        /// 移除验证标识。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="loginProvider">登录提供者。</param>
        /// <param name="tokenName">标识名称。</param>
        /// <returns>返回操作结果。</returns>
        Task<IdentityResult> RemoveAuthenticationTokenAsync(TUser user, string loginProvider, string tokenName);

        /// <summary>
        /// 加载所有用户。
        /// </summary>
        /// <param name="expression">用户条件表达式。</param>
        /// <returns>返回用户列表。</returns>
        IEnumerable<TUser> LoadUsers(Expression<Predicate<TUser>> expression = null);

        /// <summary>
        /// 加载所有用户。
        /// </summary>
        /// <param name="expression">用户条件表达式。</param>
        /// <returns>返回用户列表。</returns>
        Task<IEnumerable<TUser>> LoadUsersAsync(Expression<Predicate<TUser>> expression = null);

        /// <summary>
        /// 设置登录状态。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <returns>返回任务。</returns>
        Task SetLoginStatusAsync(TUser user);

        /// <summary>
        /// 获取缓存用户实例。
        /// </summary>
        /// <returns>返回缓存用户实例对象。</returns>
        CachedUser GetCachedUser();

        /// <summary>
        /// 获取缓存用户实例。
        /// </summary>
        /// <param name="id">用户Id。</param>
        /// <returns>返回缓存用户实例对象。</returns>
        CachedUser GetCachedUser(int id);

        /// <summary>
        /// 获取缓存用户实例。
        /// </summary>
        /// <returns>返回缓存用户实例对象。</returns>
        Task<CachedUser> GetCachedUserAsync();

        /// <summary>
        /// 获取缓存用户实例。
        /// </summary>
        /// <param name="id">用户Id。</param>
        /// <returns>返回缓存用户实例对象。</returns>
        Task<CachedUser> GetCachedUserAsync(int id);

        /// <summary>
        /// 获取缓存用户实例列表。
        /// </summary>
        /// <param name="ids">用户Id。</param>
        /// <returns>返回缓存用户实例对象列表。</returns>
        IEnumerable<CachedUser> GetCachedUsers(int[] ids);

        /// <summary>
        /// 获取缓存用户实例列表。
        /// </summary>
        /// <param name="ids">用户Id。</param>
        /// <returns>返回缓存用户实例对象列表。</returns>
        Task<IEnumerable<CachedUser>> GetCachedUsersAsync(int[] ids);

        /// <summary>
        /// 获取当前用户的所有子账户列表。
        /// </summary>
        /// <param name="userId">当前用户Id。</param>
        /// <param name="topOnly">是否只是第一层级账户。</param>
        /// <returns>返回当前用户的所有子账户列表。</returns>
        IEnumerable<IndexedUser> LoadChildren(int userId, bool topOnly = true);

        /// <summary>
        /// 获取当前用户的所有子账户列表。
        /// </summary>
        /// <param name="userId">当前用户Id。</param>
        /// <param name="topOnly">是否只是第一层级账户。</param>
        /// <returns>返回当前用户的所有子账户列表。</returns>
        Task<IEnumerable<IndexedUser>> LoadChildrenAsync(int userId, bool topOnly = true);

        /// <summary>
        /// 判断电子邮件是否已经确认过。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <returns>返回判断结果。</returns>
        Task<bool> IsEmailConfirmedAsync(TUser user);

        /// <summary>
        /// 上传头像。
        /// </summary>
        /// <param name="id">用户Id。</param>
        /// <param name="avatarFile">头像文件实例。</param>
        /// <returns>返回上传结果。</returns>
        Task<string> UploadAvatarAsync(int id, IFormFile avatarFile);

        /// <summary>
        /// 获取用户下的子级用户实例。
        /// </summary>
        /// <param name="userId">子级Id。</param>
        /// <param name="parentId">父级Id。</param>
        /// <returns>返回用户实例。</returns>
        TUser FindIndexedUser(int userId, int parentId);

        /// <summary>
        /// 获取用户下的子级用户实例。
        /// </summary>
        /// <param name="userId">子级Id。</param>
        /// <param name="parentId">父级Id。</param>
        /// <returns>返回用户实例。</returns>
        Task<TUser> FindIndexedUserAsync(int userId, int parentId);
    }

    /// <summary>
    /// 用户管理接口。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <typeparam name="TRole">角色类型。</typeparam>
    public interface IUserManager<TUser, TRole>
        : IUserManager<TUser>
        where TUser : UserBase, new()
    {
        /// <summary>
        /// 获取用户的所有角色。
        /// </summary>
        /// <param name="user">用户实例对象。</param>
        /// <returns>返回当前用户的所有角色列表。</returns>
        Task<IList<string>> GetRolesAsync(TUser user);

        /// <summary>
        /// 获取角色Id。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回角色Id集合。</returns>
        int[] GetRoleIds(int userId);

        /// <summary>
        /// 获取角色Id。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回角色Id集合。</returns>
        Task<int[]> GetRoleIdsAsync(int userId);

        /// <summary>
        /// 获取角色列表。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回角色Id集合。</returns>
        IEnumerable<TRole> GetRoles(int userId);

        /// <summary>
        /// 获取角色列表。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回角色Id集合。</returns>
        Task<IEnumerable<TRole>> GetRolesAsync(int userId);

        /// <summary>
        /// 获取最高级角色实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回用户实例对象。</returns>
        TRole GetMaxRole(int userId);

        /// <summary>
        /// 获取最高级角色实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回用户实例对象。</returns>
        Task<TRole> GetMaxRoleAsync(int userId);

        /// <summary>
        /// 将用户添加到角色中。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="roleIds">角色Id列表。</param>
        /// <returns>返回添加结果。</returns>
        bool AddUserToRoles(int userId, int[] roleIds);

        /// <summary>
        /// 将用户添加到角色中。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="roleIds">角色Id列表。</param>
        /// <returns>返回添加结果。</returns>
        Task<bool> AddUserToRolesAsync(int userId, int[] roleIds);

        /// <summary>
        /// 设置用户角色。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="roleIds">角色Id列表。</param>
        /// <returns>返回添加结果。</returns>
        bool SetUserToRoles(int userId, int[] roleIds);

        /// <summary>
        /// 设置用户角色。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="roleIds">角色Id列表。</param>
        /// <returns>返回设置结果。</returns>
        Task<bool> SetUserToRolesAsync(int userId, int[] roleIds);
    }
}