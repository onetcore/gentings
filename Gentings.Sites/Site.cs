using System;
using System.Text.Json.Serialization;
using Gentings.Data.Extensions;
using Gentings.Extensions;

namespace Gentings.Sites
{
    /// <summary>
    /// 配置基类。
    /// </summary>
    [Target(typeof(SiteAdapter))]
    public class Site
    {
        /// <summary>
        /// Id。
        /// </summary>
        [Identity]
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 所属用户。
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// 唯一键。
        /// </summary>
        [JsonIgnore]
        public string SiteKey { get; set; }

        /// <summary>
        /// 网站名称。
        /// </summary>
        [JsonIgnore]
        public string SiteName { get; set; }

        /// <summary>
        /// 简称。
        /// </summary>
        [JsonIgnore]
        public string ShortName { get; set; }

        /// <summary>
        /// 网站描述。
        /// </summary>
        [JsonIgnore]
        public string Description { get; set; }

        /// <summary>
        /// 禁用。
        /// </summary>
        [JsonIgnore]
        public bool Disabled { get; set; }

        /// <summary>
        /// 添加时间。
        /// </summary>
        [JsonIgnore]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
    }
}