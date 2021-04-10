// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
namespace Gentings.Security.Properties
{
    using System;
    using Gentings.Localization;

    /// <summary>
    /// 读取资源文件。
    /// </summary>
    internal class Resources
    {
        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public static string GetString(string key)
        {
            return ResourceManager.GetString(typeof(Resources), key);
        }

        /// <summary>
        /// 游客
        /// </summary>
        internal static string Anonymous => GetString("Anonymous");

        /// <summary>
        /// 允许用户可以登录到后台管理，拥有后台管理权限。
        /// </summary>
        internal static string DefaultPermissions_Administrator_Description => GetString("DefaultPermissions_Administrator_Description");

        /// <summary>
        /// 后台管理
        /// </summary>
        internal static string DefaultPermissions_Administrator_Name => GetString("DefaultPermissions_Administrator_Name");

        /// <summary>
        /// 开发者权限，建站时候初始化的信息。
        /// </summary>
        internal static string DefaultPermissions_Developer_Description => GetString("DefaultPermissions_Developer_Description");

        /// <summary>
        /// 建站初始配置
        /// </summary>
        internal static string DefaultPermissions_Developer_Name => GetString("DefaultPermissions_Developer_Name");

        /// <summary>
        /// 拥有者权限，允许用户配置网站信息。
        /// </summary>
        internal static string DefaultPermissions_Owner_Description => GetString("DefaultPermissions_Owner_Description");

        /// <summary>
        /// 网站配置管理
        /// </summary>
        internal static string DefaultPermissions_Owner_Name => GetString("DefaultPermissions_Owner_Name");

        /// <summary>
        /// 积分
        /// </summary>
        internal static string DefaultScoreName => GetString("DefaultScoreName");

        /// <summary>
        /// 并发处理错误，对象已经被更改。
        /// </summary>
        internal static string ErrorDescriptor_ConcurrencyFailure => GetString("ErrorDescriptor_ConcurrencyFailure");

        /// <summary>
        /// 很抱歉，发生了未知错误，操作失败，请重试。
        /// </summary>
        internal static string ErrorDescriptor_DefaultError => GetString("ErrorDescriptor_DefaultError");

        /// <summary>
        /// 电子邮件'{0}'已经存在。
        /// </summary>
        internal static string ErrorDescriptor_DuplicateEmail => GetString("ErrorDescriptor_DuplicateEmail");

        /// <summary>
        /// 角色唯一键'{0}'已经存在。
        /// </summary>
        internal static string ErrorDescriptor_DuplicateNormalizedRoleName => GetString("ErrorDescriptor_DuplicateNormalizedRoleName");

        /// <summary>
        /// 角色名称'{0}'已经存在。
        /// </summary>
        internal static string ErrorDescriptor_DuplicateRoleName => GetString("ErrorDescriptor_DuplicateRoleName");

        /// <summary>
        /// 用户名称'{0}'已经存在。
        /// </summary>
        internal static string ErrorDescriptor_DuplicateUserName => GetString("ErrorDescriptor_DuplicateUserName");

        /// <summary>
        /// 电子邮件'{0}'无效。
        /// </summary>
        internal static string ErrorDescriptor_InvalidEmail => GetString("ErrorDescriptor_InvalidEmail");

        /// <summary>
        /// 角色名称'{0}'无效。
        /// </summary>
        internal static string ErrorDescriptor_InvalidRoleName => GetString("ErrorDescriptor_InvalidRoleName");

        /// <summary>
        /// 用户标识无效。
        /// </summary>
        internal static string ErrorDescriptor_InvalidToken => GetString("ErrorDescriptor_InvalidToken");

        /// <summary>
        /// 用户名称'{0}'无效， 只能包含字母和数字。
        /// </summary>
        internal static string ErrorDescriptor_InvalidUserName => GetString("ErrorDescriptor_InvalidUserName");

        /// <summary>
        /// 用户登录已经存在。
        /// </summary>
        internal static string ErrorDescriptor_LoginAlreadyAssociated => GetString("ErrorDescriptor_LoginAlreadyAssociated");

        /// <summary>
        /// 用户安全戳不能为空。
        /// </summary>
        internal static string ErrorDescriptor_NullSecurityStamp => GetString("ErrorDescriptor_NullSecurityStamp");

        /// <summary>
        /// 密码错误。
        /// </summary>
        internal static string ErrorDescriptor_PasswordMismatch => GetString("ErrorDescriptor_PasswordMismatch");

        /// <summary>
        /// 密码最少必须包含一个('0'-'9')字符。
        /// </summary>
        internal static string ErrorDescriptor_PasswordRequiresDigit => GetString("ErrorDescriptor_PasswordRequiresDigit");

        /// <summary>
        /// 密码最少必须包含一个('a'-'z')小写字符。
        /// </summary>
        internal static string ErrorDescriptor_PasswordRequiresLower => GetString("ErrorDescriptor_PasswordRequiresLower");

        /// <summary>
        /// 密码最少必须包含一个标点符号字符。
        /// </summary>
        internal static string ErrorDescriptor_PasswordRequiresNonAlphanumeric => GetString("ErrorDescriptor_PasswordRequiresNonAlphanumeric");

        /// <summary>
        /// 密码最少必须包含{0}个不同的字符串。
        /// </summary>
        internal static string ErrorDescriptor_PasswordRequiresUniqueChars => GetString("ErrorDescriptor_PasswordRequiresUniqueChars");

        /// <summary>
        /// 密码最少必须包含一个('A'-'Z')大写字符。
        /// </summary>
        internal static string ErrorDescriptor_PasswordRequiresUpper => GetString("ErrorDescriptor_PasswordRequiresUpper");

        /// <summary>
        /// 密码最少必须{0}个字符。
        /// </summary>
        internal static string ErrorDescriptor_PasswordTooShort => GetString("ErrorDescriptor_PasswordTooShort");

        /// <summary>
        /// 兑换码赎回失败。
        /// </summary>
        internal static string ErrorDescriptor_RecoveryCodeRedemptionFailed => GetString("ErrorDescriptor_RecoveryCodeRedemptionFailed");

        /// <summary>
        /// 角色{0}不存在。
        /// </summary>
        internal static string ErrorDescriptor_RoleNotFound => GetString("ErrorDescriptor_RoleNotFound");

        /// <summary>
        /// 用户密码已经存在。
        /// </summary>
        internal static string ErrorDescriptor_UserAlreadyHasPassword => GetString("ErrorDescriptor_UserAlreadyHasPassword");

        /// <summary>
        /// 用户已经在角色'{0}'中。
        /// </summary>
        internal static string ErrorDescriptor_UserAlreadyInRole => GetString("ErrorDescriptor_UserAlreadyInRole");

        /// <summary>
        /// 用户已经被锁定。
        /// </summary>
        internal static string ErrorDescriptor_UserLockedOut => GetString("ErrorDescriptor_UserLockedOut");

        /// <summary>
        /// 用户锁定状态未启用。
        /// </summary>
        internal static string ErrorDescriptor_UserLockoutNotEnabled => GetString("ErrorDescriptor_UserLockoutNotEnabled");

        /// <summary>
        /// 用户{0}不存在。
        /// </summary>
        internal static string ErrorDescriptor_UserNameNotFound => GetString("ErrorDescriptor_UserNameNotFound");

        /// <summary>
        /// 用户不存在。
        /// </summary>
        internal static string ErrorDescriptor_UserNotFound => GetString("ErrorDescriptor_UserNotFound");

        /// <summary>
        /// 用户不在角色'{0}'中。
        /// </summary>
        internal static string ErrorDescriptor_UserNotInRole => GetString("ErrorDescriptor_UserNotInRole");

        /// <summary>
        /// 退出了登录。
        /// </summary>
        internal static string Logout_Success => GetString("Logout_Success");

        /// <summary>
        /// 已确认
        /// </summary>
        internal static string NotificationStatus_Confirmed => GetString("NotificationStatus_Confirmed");

        /// <summary>
        /// 已过期
        /// </summary>
        internal static string NotificationStatus_Expired => GetString("NotificationStatus_Expired");

        /// <summary>
        /// 新通知
        /// </summary>
        internal static string NotificationStatus_New => GetString("NotificationStatus_New");

        /// <summary>
        /// 已展示
        /// </summary>
        internal static string NotificationStatus_Notified => GetString("NotificationStatus_Notified");

        /// <summary>
        /// 系统通知清理服务
        /// </summary>
        internal static string NotificationTaskService => GetString("NotificationTaskService");

        /// <summary>
        /// 清理每个用户得系统通知，删除多余得系统通知
        /// </summary>
        internal static string NotificationTaskService_Description => GetString("NotificationTaskService_Description");

        /// <summary>
        /// 管理员角色权限不能被设置！
        /// </summary>
        internal static string PermissionSetCannotBeOwner => GetString("PermissionSetCannotBeOwner");

        /// <summary>
        /// 允许
        /// </summary>
        internal static string PermissionValue_Allow => GetString("PermissionValue_Allow");

        /// <summary>
        /// 禁止
        /// </summary>
        internal static string PermissionValue_Deny => GetString("PermissionValue_Deny");

        /// <summary>
        /// 未设置
        /// </summary>
        internal static string PermissionValue_NotSet => GetString("PermissionValue_NotSet");

        /// <summary>
        /// 消费
        /// </summary>
        internal static string ScoreType_Consume => GetString("ScoreType_Consume");

        /// <summary>
        /// 充值
        /// </summary>
        internal static string ScoreType_Recharge => GetString("ScoreType_Recharge");

        /// <summary>
        /// 退款
        /// </summary>
        internal static string ScoreType_Refund => GetString("ScoreType_Refund");

        /// <summary>
        /// 提款
        /// </summary>
        internal static string ScoreType_Withdraw => GetString("ScoreType_Withdraw");

        /// <summary>
        /// 更新积分日志失败
        /// </summary>
        internal static string UpdateScoreStatus_LogError => GetString("UpdateScoreStatus_LogError");

        /// <summary>
        /// 积分不足
        /// </summary>
        internal static string UpdateScoreStatus_NotEnough => GetString("UpdateScoreStatus_NotEnough");

        /// <summary>
        /// 更新积分失败
        /// </summary>
        internal static string UpdateScoreStatus_ScoreError => GetString("UpdateScoreStatus_ScoreError");

        /// <summary>
        /// 成功
        /// </summary>
        internal static string UpdateScoreStatus_Success => GetString("UpdateScoreStatus_Success");
    }
}

