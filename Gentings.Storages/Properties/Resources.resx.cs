// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
namespace Gentings.Storages.Properties
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
        /// 文件夹不存在！
        /// </summary>
        internal static string DirectoryNotFound => GetString("DirectoryNotFound");

        /// <summary>
        /// 文件名称非法，必须以GUID加扩展名的格式！
        /// </summary>
        internal static string FileNameIsNotGuidFormat => GetString("FileNameIsNotGuidFormat");

        /// <summary>
        /// 文件不存在！
        /// </summary>
        internal static string FileNotFound => GetString("FileNotFound");

        /// <summary>
        /// 未能获取表单文件实例或者文件长度为0！
        /// </summary>
        internal static string FormFileInvalid => GetString("FormFileInvalid");

        /// <summary>
        /// 定期清理存储文件夹和临时文件夹中得空文件夹或无效文件等
        /// </summary>
        internal static string StorageClearTaskExecutor_Description => GetString("StorageClearTaskExecutor_Description");

        /// <summary>
        /// 存储文件夹清理服务
        /// </summary>
        internal static string StorageClearTaskExecutor_Name => GetString("StorageClearTaskExecutor_Name");

        /// <summary>
        /// 存储文件失败！
        /// </summary>
        internal static string StoredFileFailured => GetString("StoredFileFailured");

        /// <summary>
        /// 成功！
        /// </summary>
        internal static string Success => GetString("Success");
    }
}

