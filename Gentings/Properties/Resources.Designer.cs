﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gentings.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Gentings.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性
        ///   重写当前线程的 CurrentUICulture 属性。
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
        ///   查找类似 参数&apos;{0}&apos;不能为空。 的本地化字符串。
        /// </summary>
        internal static string ArgumentIsEmpty {
            get {
                return ResourceManager.GetString("ArgumentIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 参数&apos;{1}&apos;的&apos;{0}&apos;属性不能为空。 的本地化字符串。
        /// </summary>
        internal static string ArgumentPropertyNull {
            get {
                return ResourceManager.GetString("ArgumentPropertyNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 参数&apos;{0}&apos;集合最少需要包含一个值。 的本地化字符串。
        /// </summary>
        internal static string CollectionArgumentIsEmpty {
            get {
                return ResourceManager.GetString("CollectionArgumentIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 恭喜你，你已经成功添加了“{0}”。 的本地化字符串。
        /// </summary>
        internal static string DataAction_Created {
            get {
                return ResourceManager.GetString("DataAction_Created", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 很抱歉，添加“{0}”失败了。 的本地化字符串。
        /// </summary>
        internal static string DataAction_CreatedFailured {
            get {
                return ResourceManager.GetString("DataAction_CreatedFailured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 恭喜你，你已经成功删除了所选择的{0}。 的本地化字符串。
        /// </summary>
        internal static string DataAction_Deleted {
            get {
                return ResourceManager.GetString("DataAction_Deleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 很抱歉，删除“{0}”失败了。 的本地化字符串。
        /// </summary>
        internal static string DataAction_DeletedFailured {
            get {
                return ResourceManager.GetString("DataAction_DeletedFailured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 很抱歉，删除“{0}”失败了，因为包含的子项不为空。 的本地化字符串。
        /// </summary>
        internal static string DataAction_DeletedFailuredItemsNotEmpty {
            get {
                return ResourceManager.GetString("DataAction_DeletedFailuredItemsNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 很抱歉，“{0}”已经存在，操作失败。 的本地化字符串。
        /// </summary>
        internal static string DataAction_Duplicate {
            get {
                return ResourceManager.GetString("DataAction_Duplicate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 恭喜你，你已经成功完成了“{0}”。 的本地化字符串。
        /// </summary>
        internal static string DataAction_Success {
            get {
                return ResourceManager.GetString("DataAction_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 很抱歉，发生了未知错误，操作失败，请重试。 的本地化字符串。
        /// </summary>
        internal static string DataAction_UnknownError {
            get {
                return ResourceManager.GetString("DataAction_UnknownError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 恭喜你，你已经成功更新了“{0}”。 的本地化字符串。
        /// </summary>
        internal static string DataAction_Updated {
            get {
                return ResourceManager.GetString("DataAction_Updated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 很抱歉，更新“{0}”失败了。 的本地化字符串。
        /// </summary>
        internal static string DataAction_UpdatedFailured {
            get {
                return ResourceManager.GetString("DataAction_UpdatedFailured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 数据库迁移完成。 的本地化字符串。
        /// </summary>
        internal static string DataMigration_Completed {
            get {
                return ResourceManager.GetString("DataMigration_Completed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 数据库迁移错误（请查看日志文件）： 的本地化字符串。
        /// </summary>
        internal static string DataMigration_Error {
            get {
                return ResourceManager.GetString("DataMigration_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 数据库迁移失败。 的本地化字符串。
        /// </summary>
        internal static string DataMigration_Failured {
            get {
                return ResourceManager.GetString("DataMigration_Failured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 开始数据库迁移。 的本地化字符串。
        /// </summary>
        internal static string DataMigration_Start {
            get {
                return ResourceManager.GetString("DataMigration_Start", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 添加了 的本地化字符串。
        /// </summary>
        internal static string DataResult_Created {
            get {
                return ResourceManager.GetString("DataResult_Created", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 删除了 的本地化字符串。
        /// </summary>
        internal static string DataResult_Deleted {
            get {
                return ResourceManager.GetString("DataResult_Deleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 更新了 的本地化字符串。
        /// </summary>
        internal static string DataResult_Updated {
            get {
                return ResourceManager.GetString("DataResult_Updated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 参数错误：{0}。 的本地化字符串。
        /// </summary>
        internal static string ErrorCode_InvalidParameters {
            get {
                return ResourceManager.GetString("ErrorCode_InvalidParameters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 发生未知错误 的本地化字符串。
        /// </summary>
        internal static string ErrorCode_UnknownError {
            get {
                return ResourceManager.GetString("ErrorCode_UnknownError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 验证失败 的本地化字符串。
        /// </summary>
        internal static string ErrorCode_ValidError {
            get {
                return ResourceManager.GetString("ErrorCode_ValidError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 未能获取表单文件实例或者文件长度为0！ 的本地化字符串。
        /// </summary>
        internal static string FormFileInvalid {
            get {
                return ResourceManager.GetString("FormFileInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 日 的本地化字符串。
        /// </summary>
        internal static string Interval_Day {
            get {
                return ResourceManager.GetString("Interval_Day", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 每天 的本地化字符串。
        /// </summary>
        internal static string Interval_Each_Day {
            get {
                return ResourceManager.GetString("Interval_Each_Day", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 每月 的本地化字符串。
        /// </summary>
        internal static string Interval_Each_Month {
            get {
                return ResourceManager.GetString("Interval_Each_Month", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 每年 的本地化字符串。
        /// </summary>
        internal static string Interval_Each_Year {
            get {
                return ResourceManager.GetString("Interval_Each_Year", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 格式错误，必须为MM-dd HH:mm格式。 的本地化字符串。
        /// </summary>
        internal static string Interval_Format_Error {
            get {
                return ResourceManager.GetString("Interval_Format_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 月 的本地化字符串。
        /// </summary>
        internal static string Interval_Month {
            get {
                return ResourceManager.GetString("Interval_Month", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 秒 的本地化字符串。
        /// </summary>
        internal static string Interval_Second {
            get {
                return ResourceManager.GetString("Interval_Second", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 每隔 的本地化字符串。
        /// </summary>
        internal static string Interval_Seconds {
            get {
                return ResourceManager.GetString("Interval_Seconds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 年 的本地化字符串。
        /// </summary>
        internal static string Interval_Year {
            get {
                return ResourceManager.GetString("Interval_Year", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 数据迁移出错：{0}。 的本地化字符串。
        /// </summary>
        internal static string MigrationError {
            get {
                return ResourceManager.GetString("MigrationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 完成 的本地化字符串。
        /// </summary>
        internal static string MigrationStatus_Completed {
            get {
                return ResourceManager.GetString("MigrationStatus_Completed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 错误 的本地化字符串。
        /// </summary>
        internal static string MigrationStatus_Error {
            get {
                return ResourceManager.GetString("MigrationStatus_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 迁移中 的本地化字符串。
        /// </summary>
        internal static string MigrationStatus_Normal {
            get {
                return ResourceManager.GetString("MigrationStatus_Normal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 类型“{1}”的属性“{0}”必须包含get访问器。 的本地化字符串。
        /// </summary>
        internal static string NoGetter {
            get {
                return ResourceManager.GetString("NoGetter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 类型“{1}”的属性“{0}”必须包含set访问器。 的本地化字符串。
        /// </summary>
        internal static string NoSetter {
            get {
                return ResourceManager.GetString("NoSetter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 实体“{0}”的主键{1}包含的不值一个属性！ 的本地化字符串。
        /// </summary>
        internal static string PrimaryKeyIsNotSingleField {
            get {
                return ResourceManager.GetString("PrimaryKeyIsNotSingleField", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 每个类只能包含一个版本“TimestampAttribute”特性属性。 的本地化字符串。
        /// </summary>
        internal static string RowVersionOnlyOnePropertyEachClass {
            get {
                return ResourceManager.GetString("RowVersionOnlyOnePropertyEachClass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 [服务]{0}执行错误：{1}。 的本地化字符串。
        /// </summary>
        internal static string TaskExecuteError {
            get {
                return ResourceManager.GetString("TaskExecuteError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 “TimestampAttribute”特性属性的数据类型必须为byte[]。 的本地化字符串。
        /// </summary>
        internal static string TypeMustBeBytes {
            get {
                return ResourceManager.GetString("TypeMustBeBytes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 在“{1}”找不到“{0}”的操作符。 的本地化字符串。
        /// </summary>
        internal static string UnknownOperation {
            get {
                return ResourceManager.GetString("UnknownOperation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 数据类型&apos;{0}&apos;暂时还不支持。 的本地化字符串。
        /// </summary>
        internal static string UnsupportedType {
            get {
                return ResourceManager.GetString("UnsupportedType", resourceCulture);
            }
        }
    }
}
