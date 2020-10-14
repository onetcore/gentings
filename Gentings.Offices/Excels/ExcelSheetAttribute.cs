using System;

namespace Gentings.Offices.Excels
{
    /// <summary>
    /// 工作表名称。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class ExcelSheetAttribute : Attribute
    {
        /// <summary>
        /// 初始化类<see cref="ExcelSheetAttribute"/>。
        /// </summary>
        /// <param name="sheetName">工作表名称。</param>
        /// <param name="sheetId">索引Id。</param>
        public ExcelSheetAttribute(string sheetName = "sheet1", uint sheetId = 1)
        {
            SheetName = sheetName;
            SheetId = sheetId;
        }

        /// <summary>
        /// 工作表名称。
        /// </summary>
        public string SheetName { get; }

        /// <summary>
        /// 索引Id。
        /// </summary>
        public uint SheetId { get; }
    }
}