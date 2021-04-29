// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
namespace Gentings.Properties
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
        /// 属性表达式'{0}'不正确， 表达式必须提供一种属性访问，如： 't => t.MyProperty'；如果式多个属性，需要如下代码表示：'t => new {{ t.MyProperty1, t.MyProperty2 }}'。
        /// </summary>
        internal static string ExpressionExtensions_InvalidPropertiesExpression => GetString("ExpressionExtensions_InvalidPropertiesExpression");

        /// <summary>
        /// 属性表达式'{0}'不正确， 表达式必须提供一种属性访问，如： 't => t.MyProperty'。
        /// </summary>
        internal static string ExpressionExtensions_InvalidPropertyExpression => GetString("ExpressionExtensions_InvalidPropertyExpression");

        /// <summary>
        /// 字节大小必须能被 8 整除。
        /// </summary>
        internal static string RandomNumberGenerator_SizeInvalid => GetString("RandomNumberGenerator_SizeInvalid");

        /// <summary>
        /// 本程序可以使用的命令包含以下内容：
        /// </summary>
        internal static string CommandHandlerFactory_CommandList => GetString("CommandHandlerFactory_CommandList");

        /// <summary>
        /// 关闭!
        /// </summary>
        internal static string CommandConsole_DelayAsync_Close => GetString("CommandConsole_DelayAsync_Close");

        /// <summary>
        /// 程序即将在“{0}”秒后关闭...
        /// </summary>
        internal static string CommandConsole_CloseAsync_DelayClose => GetString("CommandConsole_CloseAsync_DelayClose");

        /// <summary>
        /// 不支持命令“{0}”，请使用.help命令来获取帮助信息！
        /// </summary>
        internal static string CommandConsole_StartAsync_UseHelp => GetString("CommandConsole_StartAsync_UseHelp");

        /// <summary>
        /// 已经成功启动了应用程序，可以输入命令进行手动操作！
        /// </summary>
        internal static string CommandConsole_StartAsync_SuccessStart => GetString("CommandConsole_StartAsync_SuccessStart");

        /// <summary>
        /// 正在初始化应用程序！ 
        /// </summary>
        internal static string CommandConsole_StartAsync_Initailize => GetString("CommandConsole_StartAsync_Initailize");

        /// <summary>
        /// 参数值必须以“{0}”结尾！
        /// </summary>
        internal static string CommandArgs_Read_MustEndsWith => GetString("CommandArgs_Read_MustEndsWith");

        /// <summary>
        /// 显示帮助信息
        /// </summary>
        internal static string CommandHandlerFactory_ExecuteAsync_HelpDescription => GetString("CommandHandlerFactory_ExecuteAsync_HelpDescription");

        /// <summary>
        /// 退出应用程序
        /// </summary>
        internal static string CommandHandlerFactory_ExecuteAsync_Quit => GetString("CommandHandlerFactory_ExecuteAsync_Quit");

        /// <summary>
        /// 不支持命令：.{0}，请使用.help命令查看可用命令
        /// </summary>
        internal static string CommandHandlerFactory_ExecuteAsync_NotSupported => GetString("CommandHandlerFactory_ExecuteAsync_NotSupported");

        /// <summary>
        /// 是否显示调试信息，如果关闭使用命令.debug off
        /// </summary>
        internal static string DebugCommandHandler_Help_DebugOff => GetString("DebugCommandHandler_Help_DebugOff");

        /// <summary>
        /// 开启调试信息显示！
        /// </summary>
        internal static string DebugCommandHandler_ExecuteAsync_DebugOn => GetString("DebugCommandHandler_ExecuteAsync_DebugOn");

        /// <summary>
        /// 关闭调试信息显示！
        /// </summary>
        internal static string DebugCommandHandler_ExecuteAsync_DebugOff => GetString("DebugCommandHandler_ExecuteAsync_DebugOff");

        /// <summary>
        /// 日报表
        /// </summary>
        internal static string IndexedMode_Day => GetString("IndexedMode_Day");

        /// <summary>
        /// 小时
        /// </summary>
        internal static string IndexedMode_Hour => GetString("IndexedMode_Hour");

        /// <summary>
        /// 月报表
        /// </summary>
        internal static string IndexedMode_Month => GetString("IndexedMode_Month");

        /// <summary>
        /// 年报表
        /// </summary>
        internal static string IndexedMode_Year => GetString("IndexedMode_Year");

        /// <summary>
        /// 未能获取表单文件实例或者文件长度为0！
        /// </summary>
        internal static string StorageDirectory_FormFileInvalid => GetString("StorageDirectory_FormFileInvalid");

        /// <summary>
        /// 参数'{0}'不能为空。
        /// </summary>
        internal static string Check_ArgumentIsEmpty => GetString("Check_ArgumentIsEmpty");

        /// <summary>
        /// 参数'{1}'的'{0}'属性不能为空。
        /// </summary>
        internal static string Check_ArgumentPropertyNull => GetString("Check_ArgumentPropertyNull");

        /// <summary>
        /// 参数'{0}'集合最少需要包含一个值。
        /// </summary>
        internal static string Check_CollectionArgumentIsEmpty => GetString("Check_CollectionArgumentIsEmpty");

        /// <summary>
        /// 恭喜你，你已经成功添加了“{0}”。
        /// </summary>
        internal static string DataAction_Created => GetString("DataAction_Created");

        /// <summary>
        /// 很抱歉，添加“{0}”失败了。
        /// </summary>
        internal static string DataAction_CreatedFailured => GetString("DataAction_CreatedFailured");

        /// <summary>
        /// 恭喜你，你已经成功删除了所选择的{0}。
        /// </summary>
        internal static string DataAction_Deleted => GetString("DataAction_Deleted");

        /// <summary>
        /// 很抱歉，删除“{0}”失败了。
        /// </summary>
        internal static string DataAction_DeletedFailured => GetString("DataAction_DeletedFailured");

        /// <summary>
        /// 很抱歉，删除“{0}”失败了，因为包含的子项不为空。
        /// </summary>
        internal static string DataAction_DeletedFailuredItemsNotEmpty => GetString("DataAction_DeletedFailuredItemsNotEmpty");

        /// <summary>
        /// 很抱歉，“{0}”已经存在，操作失败。
        /// </summary>
        internal static string DataAction_Duplicate => GetString("DataAction_Duplicate");

        /// <summary>
        /// 恭喜你，你已经成功完成了“{0}”。
        /// </summary>
        internal static string DataAction_Success => GetString("DataAction_Success");

        /// <summary>
        /// 很抱歉，发生了未知错误，操作失败，请重试。
        /// </summary>
        internal static string DataAction_UnknownError => GetString("DataAction_UnknownError");

        /// <summary>
        /// 恭喜你，你已经成功更新了“{0}”。
        /// </summary>
        internal static string DataAction_Updated => GetString("DataAction_Updated");

        /// <summary>
        /// 很抱歉，更新“{0}”失败了。
        /// </summary>
        internal static string DataAction_UpdatedFailured => GetString("DataAction_UpdatedFailured");

        /// <summary>
        /// 数据库迁移完成。
        /// </summary>
        internal static string DataMigration_Completed => GetString("DataMigration_Completed");

        /// <summary>
        /// 数据库迁移错误（请查看日志文件）：
        /// </summary>
        internal static string DataMigration_Error => GetString("DataMigration_Error");

        /// <summary>
        /// 数据库迁移失败。
        /// </summary>
        internal static string DataMigration_Failured => GetString("DataMigration_Failured");

        /// <summary>
        /// 开始数据库迁移。
        /// </summary>
        internal static string DataMigration_Start => GetString("DataMigration_Start");

        /// <summary>
        /// 添加了
        /// </summary>
        internal static string DataResult_Created => GetString("DataResult_Created");

        /// <summary>
        /// 删除了
        /// </summary>
        internal static string DataResult_Deleted => GetString("DataResult_Deleted");

        /// <summary>
        /// 更新了
        /// </summary>
        internal static string DataResult_Updated => GetString("DataResult_Updated");

        /// <summary>
        /// 应用程序初始进程
        /// </summary>
        internal static string InitializerHostedService => GetString("InitializerHostedService");

        /// <summary>
        /// 应用程序执行开始时执行的后台服务，数据库初始化等操作
        /// </summary>
        internal static string InitializerHostedService_Description => GetString("InitializerHostedService_Description");

        /// <summary>
        /// 网站初始化失败！
        /// </summary>
        internal static string InitializerHostedService_InitializedFailured => GetString("InitializerHostedService_InitializedFailured");

        /// <summary>
        /// 启动网站完成。
        /// </summary>
        internal static string InitializerHostedService_Started => GetString("InitializerHostedService_Started");

        /// <summary>
        /// 启动网站失败。
        /// </summary>
        internal static string InitializerHostedService_StartFailured => GetString("InitializerHostedService_StartFailured");

        /// <summary>
        /// 启动网站...
        /// </summary>
        internal static string InitializerHostedService_Starting => GetString("InitializerHostedService_Starting");

        /// <summary>
        /// 数据迁移出错：{0}。
        /// </summary>
        internal static string MigrationError => GetString("MigrationError");

        /// <summary>
        /// 完成
        /// </summary>
        internal static string MigrationStatus_Completed => GetString("MigrationStatus_Completed");

        /// <summary>
        /// 错误
        /// </summary>
        internal static string MigrationStatus_Error => GetString("MigrationStatus_Error");

        /// <summary>
        /// 迁移中
        /// </summary>
        internal static string MigrationStatus_Normal => GetString("MigrationStatus_Normal");

        /// <summary>
        /// 类型“{1}”的属性“{0}”必须包含get访问器。
        /// </summary>
        internal static string ClrPropertyGetterFactory_NoGetter => GetString("ClrPropertyGetterFactory_NoGetter");

        /// <summary>
        /// 类型“{1}”的属性“{0}”必须包含set访问器。
        /// </summary>
        internal static string ClrPropertySetterFactory_NoSetter => GetString("ClrPropertySetterFactory_NoSetter");

        /// <summary>
        /// 实体“{0}”的主键{1}包含的不值一个属性！
        /// </summary>
        internal static string TypeExtensions_PrimaryKeyIsNotSingleField => GetString("TypeExtensions_PrimaryKeyIsNotSingleField");

        /// <summary>
        /// 每个类只能包含一个版本“TimestampAttribute”特性属性。
        /// </summary>
        internal static string Property_RowVersionOnlyOnePropertyEachClass => GetString("Property_RowVersionOnlyOnePropertyEachClass");

        /// <summary>
        /// 在“{1}”找不到“{0}”的操作符。
        /// </summary>
        internal static string MigrationsSqlGenerator_UnknownOperation => GetString("MigrationsSqlGenerator_UnknownOperation");

        /// <summary>
        /// “TimestampAttribute”特性属性的数据类型必须为byte[]。
        /// </summary>
        internal static string Property_TypeMustBeBytes => GetString("Property_TypeMustBeBytes");

        /// <summary>
        /// 数据类型'{0}'暂时还不支持。
        /// </summary>
        internal static string SqlExpressionVisitor_UnsupportedType => GetString("SqlExpressionVisitor_UnsupportedType");

        /// <summary>
        /// 参数错误：{0}。
        /// </summary>
        internal static string ErrorCode_InvalidParameters => GetString("ErrorCode_InvalidParameters");

        /// <summary>
        /// 发生未知错误
        /// </summary>
        internal static string ErrorCode_UnknownError => GetString("ErrorCode_UnknownError");

        /// <summary>
        /// 验证失败
        /// </summary>
        internal static string ErrorCode_ValidError => GetString("ErrorCode_ValidError");

        /// <summary>
        /// 新增了
        /// </summary>
        internal static string DifferAction_Add => GetString("DifferAction_Add");

        /// <summary>
        /// ”{0}“(”{1}“)
        /// </summary>
        internal static string DifferAction_AddFormat => GetString("DifferAction_AddFormat");

        /// <summary>
        /// 修改了
        /// </summary>
        internal static string DifferAction_Modify => GetString("DifferAction_Modify");

        /// <summary>
        /// “{0}”由”{1}“修改为“{2}”
        /// </summary>
        internal static string DifferAction_ModifyFormat => GetString("DifferAction_ModifyFormat");

        /// <summary>
        /// 移除了
        /// </summary>
        internal static string DifferAction_Remove => GetString("DifferAction_Remove");

        /// <summary>
        /// ”{0}“(”{1}“)
        /// </summary>
        internal static string DifferAction_RemoveFormat => GetString("DifferAction_RemoveFormat");

        /// <summary>
        /// 你已经对比过一次新对象，不能重复对比变更对象！
        /// </summary>
        internal static string Differ_Duplicated_Differed => GetString("Differ_Duplicated_Differed");

        /// <summary>
        /// 原有对象已经初始化，不能重复调用初始化方法！
        /// </summary>
        internal static string Differ_Duplicated_Initialized => GetString("Differ_Duplicated_Initialized");

        /// <summary>
        /// 对象变更原始对象未初始化，请在修改对象时候先调用Init方法。
        /// </summary>
        internal static string Differ_Uninitialized => GetString("Differ_Uninitialized");

        /// <summary>
        /// 成功
        /// </summary>
        internal static string EventLevel_Success => GetString("EventLevel_Success");

        /// <summary>
        /// 错误
        /// </summary>
        internal static string EventLevel_Error => GetString("EventLevel_Error");

        /// <summary>
        /// 消息
        /// </summary>
        internal static string EventLevel_Information => GetString("EventLevel_Information");

        /// <summary>
        /// 警告
        /// </summary>
        internal static string EventLevel_Warning => GetString("EventLevel_Warning");

        /// <summary>
        /// 系统
        /// </summary>
        internal static string EventType => GetString("EventType");

        /// <summary>
        /// 日
        /// </summary>
        internal static string Interval_Day => GetString("Interval_Day");

        /// <summary>
        /// 每天
        /// </summary>
        internal static string Interval_Each_Day => GetString("Interval_Each_Day");

        /// <summary>
        /// 每月
        /// </summary>
        internal static string Interval_Each_Month => GetString("Interval_Each_Month");

        /// <summary>
        /// 每年
        /// </summary>
        internal static string Interval_Each_Year => GetString("Interval_Each_Year");

        /// <summary>
        /// 格式错误，必须为MM-dd HH:mm格式。
        /// </summary>
        internal static string Interval_Format_Error => GetString("Interval_Format_Error");

        /// <summary>
        /// 月
        /// </summary>
        internal static string Interval_Month => GetString("Interval_Month");

        /// <summary>
        /// 秒
        /// </summary>
        internal static string Interval_Second => GetString("Interval_Second");

        /// <summary>
        /// 每隔
        /// </summary>
        internal static string Interval_Seconds => GetString("Interval_Seconds");

        /// <summary>
        /// 年
        /// </summary>
        internal static string Interval_Year => GetString("Interval_Year");

        /// <summary>
        /// [服务]{0}执行错误：{1}。
        /// </summary>
        internal static string TaskExecuteError => GetString("TaskExecuteError");

        /// <summary>
        /// 后台任务进程
        /// </summary>
        internal static string TaskHostedService => GetString("TaskHostedService");

        /// <summary>
        /// 用于执行后台任务定时服务，后台单独线程监听定时任务操作
        /// </summary>
        internal static string TaskHostedService_Description => GetString("TaskHostedService_Description");

        /// <summary>
        /// 完成
        /// </summary>
        internal static string ErrorCode_Success => GetString("ErrorCode_Success");

        /// <summary>
        /// 超过了索引长度。
        /// </summary>
        internal static string SourceReader_OutOfIndex => GetString("SourceReader_OutOfIndex");

        /// <summary>
        /// 起始位置字符和‘{0}’不匹配！
        /// </summary>
        internal static string SourceReader_StartCharNotMatch => GetString("SourceReader_StartCharNotMatch");

        /// <summary>
        /// 不能在服务内部设置 IsStack 和 Interval 属性！
        /// </summary>
        internal static string Argument_CannotSetIsStackAndInterval => GetString("Argument_CannotSetIsStackAndInterval");

        /// <summary>
        /// [数据库]执行SQL错误：
        /// </summary>
        internal static string Database_SqlExecuteError => GetString("Database_SqlExecuteError");

        /// <summary>
        /// 
        /// </summary>
        internal static string FileHelper_InvalidByte => GetString("FileHelper_InvalidByte");
    }
}

