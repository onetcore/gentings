using Gentings.Extensions.Categories;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.ChatServers
{
    /// <summary>
    /// 分类。
    /// </summary>
    [Table("chat_Friends_Categories")]
    public class Category : CategoryBase
    {
        /// <summary>
        /// 用户Id。
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 好友数量。
        /// </summary>
        public int Count { get; set; }
    }
}
