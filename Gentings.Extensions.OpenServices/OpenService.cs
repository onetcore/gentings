using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions.OpenServices
{
    /// <summary>
    /// 公开的API服务。
    /// </summary>
    [Table("core_Services")]
    public class OpenService : IIdObject
    {
        /// <summary>
        /// 获取或设置唯一Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 路由路径。
        /// </summary>
        [Size(256)]
        public string Route { get; set; }

        /// <summary>
        /// 方法。
        /// </summary>
        [Size(20)]
        public string HttpMethod { get; set; }

        /// <summary>
        /// 分类。
        /// </summary>
        [Size(64)]
        public string Category { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否禁用。
        /// </summary>
        public bool Disabled { get; set; }
    }
}