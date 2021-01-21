using System;
using Gentings.Identity.Properties;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Gentings.Identity
{
    /// <summary>
    /// 用户配置。
    /// </summary>
    public class IdentitySettings
    {
        /// <summary>
        /// 获取或设置允许使用的用户名字符，默认为：abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+
        /// </summary>
        public string AllowedUserNameCharacters { get; set; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

        /// <summary>
        /// 获取或设置邮件是否唯一。
        /// </summary>
        public bool RequireUniqueEmail { get; set; }

        /// <summary>
        /// 密码最小长度。
        /// </summary>
        public int RequiredPasswordLength { get; set; } = 6;

        /// <summary>
        /// 密码必须包含的最小唯一字符数。
        /// </summary>
        public int RequiredUniqueChars { get; set; } = 1;

        /// <summary>
        /// 密码是否需要符号类字符。
        /// </summary>
        public bool RequireNonAlphanumeric { get; set; } = true;

        /// <summary>
        /// 密码是否需要小写字母。
        /// </summary>
        public bool RequireLowercase { get; set; } = true;

        /// <summary>
        /// 密码是否需要大写字母。
        /// </summary>
        public bool RequireUppercase { get; set; } = true;

        /// <summary>
        /// 密码是否需要数字字符。
        /// </summary>
        public bool RequireDigit { get; set; } = true;

        /// <summary>
        /// 对新用户是否开启锁定功能。
        /// </summary>
        public bool AllowedLockoutForNewUsers { get; set; } = true;

        /// <summary>
        /// 用户尝试登录失败的次数，当达到这个次数时账号将被锁定。
        /// </summary>
        public int MaxFailedAccessAttempts { get; set; } = 5;

        /// <summary>
        /// 用户登录失败达到次数后，锁定的时间长度。
        /// </summary>
        public TimeSpan DefaultLockoutTimeSpan { get; set; } = TimeSpan.FromMinutes(5.0);

        /// <summary>
        /// 账户是否需要电子邮件确认，如果设置为<code>true</code>，当登录时需要判断是否已经确认了邮件。
        /// </summary>
        public bool RequireConfirmedEmail { get; set; }

        /// <summary>
        /// 账户是否需要电话号码确认，如果设置为<code>true</code>，当登录时需要判断是否已经验证了电话号码。
        /// </summary>
        public bool RequireConfirmedPhoneNumber { get; set; }

        /// <summary>
        /// 账户是否需要进行账号激活，一般在社会化登录时候使用。
        /// </summary>
        public bool RequireConfirmedAccount { get; set; }

        /// <summary>
        /// 积分名称。
        /// </summary>
        public string ScoreName { get; set; } = Resources.DefaultScoreName;

        /// <summary>
        /// 积分单位。
        /// </summary>
        public string ScoreUnit { get; set; }

        /// <summary>
        /// 转换用户配置。
        /// </summary>
        /// <param name="optionsAccessor">代码中配置选项。</param>
        /// <returns>返回用户配置实例。</returns>
        public virtual IdentityOptions ToIdentityOptions(IOptions<IdentityOptions> optionsAccessor)
        {
            var options = new IdentityOptions();
            options.User = new UserOptions
            {
                AllowedUserNameCharacters = AllowedUserNameCharacters,
                RequireUniqueEmail = RequireUniqueEmail
            };
            options.Password = new PasswordOptions
            {
                RequireDigit = RequireDigit,
                RequireLowercase = RequireLowercase,
                RequireNonAlphanumeric = RequireNonAlphanumeric,
                RequireUppercase = RequireUppercase,
                RequiredLength = RequiredPasswordLength,
                RequiredUniqueChars = RequiredUniqueChars
            };
            options.Lockout = new LockoutOptions
            {
                AllowedForNewUsers = AllowedLockoutForNewUsers,
                DefaultLockoutTimeSpan = DefaultLockoutTimeSpan,
                MaxFailedAccessAttempts = MaxFailedAccessAttempts
            };
            options.SignIn = new SignInOptions
            {
                RequireConfirmedAccount = RequireConfirmedAccount,
                RequireConfirmedEmail = RequireConfirmedEmail,
                RequireConfirmedPhoneNumber = RequireConfirmedPhoneNumber
            };
            var config = optionsAccessor?.Value;
            options.ClaimsIdentity = config?.ClaimsIdentity ?? new ClaimsIdentityOptions();
            options.Stores = config?.Stores ?? new StoreOptions();
            options.Tokens = config?.Tokens ?? new TokenOptions();
            return options;
        }
    }
}