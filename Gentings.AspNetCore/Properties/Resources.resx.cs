// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
namespace Gentings.AspNetCore.Properties
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
        /// MM月dd日
        /// </summary>
        internal static string DateFormat => GetString("DateFormat");

        /// <summary>
        /// {0} 天前
        /// </summary>
        internal static string DaysBefore => GetString("DaysBefore");

        /// <summary>
        /// 请选择
        /// </summary>
        internal static string DropdownListTagHelper_SelectDefaultText => GetString("DropdownListTagHelper_SelectDefaultText");

        /// <summary>
        /// 无法找到枚举类型，需要设置Value值或者使用For指定枚举属性！
        /// </summary>
        internal static string EnumDropdownListTagHelper_TypeNotFound => GetString("EnumDropdownListTagHelper_TypeNotFound");

        /// <summary>
        /// 应用不存在
        /// </summary>
        internal static string ErrorCode_ApplicationNotFound => GetString("ErrorCode_ApplicationNotFound");

        /// <summary>
        /// 密钥不正确
        /// </summary>
        internal static string ErrorCode_AppSecretInvalid => GetString("ErrorCode_AppSecretInvalid");

        /// <summary>
        /// FileProvider不存在！
        /// </summary>
        internal static string FileProviderNotFound => GetString("FileProviderNotFound");

        /// <summary>
        /// {0} 小时前
        /// </summary>
        internal static string HoursBefore => GetString("HoursBefore");

        /// <summary>
        /// Bootstrap
        /// </summary>
        internal static string ImportLibrary_Bootstrap => GetString("ImportLibrary_Bootstrap");

        /// <summary>
        /// Feather图标
        /// </summary>
        internal static string ImportLibrary_Feather => GetString("ImportLibrary_Feather");

        /// <summary>
        /// Font Awesome图标
        /// </summary>
        internal static string ImportLibrary_FontAwesome => GetString("ImportLibrary_FontAwesome");

        /// <summary>
        /// GtCore
        /// </summary>
        internal static string ImportLibrary_GtCore => GetString("ImportLibrary_GtCore");

        /// <summary>
        /// jQuery
        /// </summary>
        internal static string ImportLibrary_JQuery => GetString("ImportLibrary_JQuery");

        /// <summary>
        /// 无
        /// </summary>
        internal static string ImportLibrary_None => GetString("ImportLibrary_None");

        /// <summary>
        /// 刚刚
        /// </summary>
        internal static string JustNow => GetString("JustNow");

        /// <summary>
        /// 全屏显示
        /// </summary>
        internal static string Mozmd_FullScreen => GetString("Mozmd_FullScreen");

        /// <summary>
        /// 预览
        /// </summary>
        internal static string Mozmd_ModePreview => GetString("Mozmd_ModePreview");

        /// <summary>
        /// 加粗
        /// </summary>
        internal static string Mozmd_Syntax_Bold => GetString("Mozmd_Syntax_Bold");

        /// <summary>
        /// 代码
        /// </summary>
        internal static string Mozmd_Syntax_Code => GetString("Mozmd_Syntax_Code");

        /// <summary>
        /// 标题
        /// </summary>
        internal static string Mozmd_Syntax_Header => GetString("Mozmd_Syntax_Header");

        /// <summary>
        /// 图片
        /// </summary>
        internal static string Mozmd_Syntax_Image => GetString("Mozmd_Syntax_Image");

        /// <summary>
        /// 斜体
        /// </summary>
        internal static string Mozmd_Syntax_Italic => GetString("Mozmd_Syntax_Italic");

        /// <summary>
        /// 链接
        /// </summary>
        internal static string Mozmd_Syntax_Link => GetString("Mozmd_Syntax_Link");

        /// <summary>
        /// 排序
        /// </summary>
        internal static string Mozmd_Syntax_Ol => GetString("Mozmd_Syntax_Ol");

        /// <summary>
        /// 引用
        /// </summary>
        internal static string Mozmd_Syntax_Quote => GetString("Mozmd_Syntax_Quote");

        /// <summary>
        /// 列表
        /// </summary>
        internal static string Mozmd_Syntax_Ul => GetString("Mozmd_Syntax_Ul");

        /// <summary>
        /// {0} 分钟前
        /// </summary>
        internal static string MunitesBefore => GetString("MunitesBefore");

        /// <summary>
        /// 上一页
        /// </summary>
        internal static string PageTagHelper_LastPage => GetString("PageTagHelper_LastPage");

        /// <summary>
        /// 下一页
        /// </summary>
        internal static string PageTagHelper_NextPage => GetString("PageTagHelper_NextPage");

        /// <summary>
        /// 第{0}页
        /// </summary>
        internal static string PageTagHelper_NumberPage => GetString("PageTagHelper_NumberPage");

        /// <summary>
        /// 点击刷新验证码
        /// </summary>
        internal static string VerifierTagHelper_ClickRefresh => GetString("VerifierTagHelper_ClickRefresh");

        /// <summary>
        /// yyyy年MM月dd日
        /// </summary>
        internal static string YearDateFormat => GetString("YearDateFormat");
    }
}

