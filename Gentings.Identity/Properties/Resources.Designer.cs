﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gentings.Identity.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Gentings.Identity.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性，对
        ///   使用此强类型资源类的所有资源查找执行重写。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 允许用户可以管理初始配置相关内容! 的本地化字符串。
        /// </summary>
        internal static string DefaultPermissions_Administrator_Description {
            get {
                return ResourceManager.GetString("DefaultPermissions_Administrator_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 初始配置管理 的本地化字符串。
        /// </summary>
        internal static string DefaultPermissions_Administrator_Name {
            get {
                return ResourceManager.GetString("DefaultPermissions_Administrator_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 积分 的本地化字符串。
        /// </summary>
        internal static string DefaultScoreName {
            get {
                return ResourceManager.GetString("DefaultScoreName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 并发处理错误，对象已经被更改。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_ConcurrencyFailure {
            get {
                return ResourceManager.GetString("ErrorDescriptor_ConcurrencyFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 很抱歉，发生了未知错误，操作失败，请重试。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_DefaultError {
            get {
                return ResourceManager.GetString("ErrorDescriptor_DefaultError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 电子邮件&apos;{0}&apos;已经存在。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_DuplicateEmail {
            get {
                return ResourceManager.GetString("ErrorDescriptor_DuplicateEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 角色唯一键&apos;{0}&apos;已经存在。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_DuplicateNormalizedRoleName {
            get {
                return ResourceManager.GetString("ErrorDescriptor_DuplicateNormalizedRoleName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 角色名称&apos;{0}&apos;已经存在。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_DuplicateRoleName {
            get {
                return ResourceManager.GetString("ErrorDescriptor_DuplicateRoleName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户名称&apos;{0}&apos;已经存在。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_DuplicateUserName {
            get {
                return ResourceManager.GetString("ErrorDescriptor_DuplicateUserName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 电子邮件&apos;{0}&apos;无效。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_InvalidEmail {
            get {
                return ResourceManager.GetString("ErrorDescriptor_InvalidEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 角色名称&apos;{0}&apos;无效。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_InvalidRoleName {
            get {
                return ResourceManager.GetString("ErrorDescriptor_InvalidRoleName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户标识无效。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_InvalidToken {
            get {
                return ResourceManager.GetString("ErrorDescriptor_InvalidToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户名称&apos;{0}&apos;无效， 只能包含字母和数字。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_InvalidUserName {
            get {
                return ResourceManager.GetString("ErrorDescriptor_InvalidUserName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户登录已经存在。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_LoginAlreadyAssociated {
            get {
                return ResourceManager.GetString("ErrorDescriptor_LoginAlreadyAssociated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户安全戳不能为空。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_NullSecurityStamp {
            get {
                return ResourceManager.GetString("ErrorDescriptor_NullSecurityStamp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 密码错误。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_PasswordMismatch {
            get {
                return ResourceManager.GetString("ErrorDescriptor_PasswordMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 密码最少必须包含一个(&apos;0&apos;-&apos;9&apos;)字符。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_PasswordRequiresDigit {
            get {
                return ResourceManager.GetString("ErrorDescriptor_PasswordRequiresDigit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 密码最少必须包含一个(&apos;a&apos;-&apos;z&apos;)小写字符。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_PasswordRequiresLower {
            get {
                return ResourceManager.GetString("ErrorDescriptor_PasswordRequiresLower", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 密码最少必须包含一个标点符号字符。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_PasswordRequiresNonAlphanumeric {
            get {
                return ResourceManager.GetString("ErrorDescriptor_PasswordRequiresNonAlphanumeric", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 密码最少必须包含{0}个不同的字符串。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_PasswordRequiresUniqueChars {
            get {
                return ResourceManager.GetString("ErrorDescriptor_PasswordRequiresUniqueChars", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 密码最少必须包含一个(&apos;A&apos;-&apos;Z&apos;)大写字符。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_PasswordRequiresUpper {
            get {
                return ResourceManager.GetString("ErrorDescriptor_PasswordRequiresUpper", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 密码最少必须{0}个字符。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_PasswordTooShort {
            get {
                return ResourceManager.GetString("ErrorDescriptor_PasswordTooShort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 兑换码赎回失败。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_RecoveryCodeRedemptionFailed {
            get {
                return ResourceManager.GetString("ErrorDescriptor_RecoveryCodeRedemptionFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 角色{0}不存在。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_RoleNotFound {
            get {
                return ResourceManager.GetString("ErrorDescriptor_RoleNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户密码已经存在。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_UserAlreadyHasPassword {
            get {
                return ResourceManager.GetString("ErrorDescriptor_UserAlreadyHasPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户已经在角色&apos;{0}&apos;中。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_UserAlreadyInRole {
            get {
                return ResourceManager.GetString("ErrorDescriptor_UserAlreadyInRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户已经被锁定。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_UserLockedOut {
            get {
                return ResourceManager.GetString("ErrorDescriptor_UserLockedOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户锁定状态未启用。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_UserLockoutNotEnabled {
            get {
                return ResourceManager.GetString("ErrorDescriptor_UserLockoutNotEnabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户{0}不存在。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_UserNameNotFound {
            get {
                return ResourceManager.GetString("ErrorDescriptor_UserNameNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户不存在。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_UserNotFound {
            get {
                return ResourceManager.GetString("ErrorDescriptor_UserNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户不在角色&apos;{0}&apos;中。 的本地化字符串。
        /// </summary>
        internal static string ErrorDescriptor_UserNotInRole {
            get {
                return ResourceManager.GetString("ErrorDescriptor_UserNotInRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 已确认 的本地化字符串。
        /// </summary>
        internal static string NotificationStatus_Confirmed {
            get {
                return ResourceManager.GetString("NotificationStatus_Confirmed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 已过期 的本地化字符串。
        /// </summary>
        internal static string NotificationStatus_Expired {
            get {
                return ResourceManager.GetString("NotificationStatus_Expired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 新通知 的本地化字符串。
        /// </summary>
        internal static string NotificationStatus_New {
            get {
                return ResourceManager.GetString("NotificationStatus_New", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 已展示 的本地化字符串。
        /// </summary>
        internal static string NotificationStatus_Notified {
            get {
                return ResourceManager.GetString("NotificationStatus_Notified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 系统通知清理服务 的本地化字符串。
        /// </summary>
        internal static string NotificationTaskService {
            get {
                return ResourceManager.GetString("NotificationTaskService", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 清理每个用户得系统通知，删除多余得系统通知 的本地化字符串。
        /// </summary>
        internal static string NotificationTaskService_Description {
            get {
                return ResourceManager.GetString("NotificationTaskService_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 管理员角色权限不能被设置！ 的本地化字符串。
        /// </summary>
        internal static string PermissionSetCannotBeOwner {
            get {
                return ResourceManager.GetString("PermissionSetCannotBeOwner", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 允许 的本地化字符串。
        /// </summary>
        internal static string PermissionValue_Allow {
            get {
                return ResourceManager.GetString("PermissionValue_Allow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 禁止 的本地化字符串。
        /// </summary>
        internal static string PermissionValue_Deny {
            get {
                return ResourceManager.GetString("PermissionValue_Deny", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 未设置 的本地化字符串。
        /// </summary>
        internal static string PermissionValue_NotSet {
            get {
                return ResourceManager.GetString("PermissionValue_NotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 消费 的本地化字符串。
        /// </summary>
        internal static string ScoreType_Consume {
            get {
                return ResourceManager.GetString("ScoreType_Consume", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 其他 的本地化字符串。
        /// </summary>
        internal static string ScoreType_Others {
            get {
                return ResourceManager.GetString("ScoreType_Others", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 充值 的本地化字符串。
        /// </summary>
        internal static string ScoreType_Recharge {
            get {
                return ResourceManager.GetString("ScoreType_Recharge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 退款 的本地化字符串。
        /// </summary>
        internal static string ScoreType_Refund {
            get {
                return ResourceManager.GetString("ScoreType_Refund", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 更新积分日志失败 的本地化字符串。
        /// </summary>
        internal static string UpdateScoreStatus_LogError {
            get {
                return ResourceManager.GetString("UpdateScoreStatus_LogError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 积分不足 的本地化字符串。
        /// </summary>
        internal static string UpdateScoreStatus_NotEnough {
            get {
                return ResourceManager.GetString("UpdateScoreStatus_NotEnough", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 更新积分失败 的本地化字符串。
        /// </summary>
        internal static string UpdateScoreStatus_ScoreError {
            get {
                return ResourceManager.GetString("UpdateScoreStatus_ScoreError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 成功 的本地化字符串。
        /// </summary>
        internal static string UpdateScoreStatus_Success {
            get {
                return ResourceManager.GetString("UpdateScoreStatus_Success", resourceCulture);
            }
        }
    }
}
