using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Data.Initializers
{
    /// <summary>
    /// 安装实例。
    /// </summary>
    [Table("core_Lisences")]
    public class Lisence
    {
        /// <summary>
        /// 注册码。
        /// </summary>
        public string Registration { get; set; }
    }
}