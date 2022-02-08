using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions.Sites.SectionRenders.Defaults
{
    /// <summary>
    /// 默认节点模板。
    /// </summary>
    public class DefaultSection : Section
    {
        /// <summary>
        /// html代码。
        /// </summary>
        [NotMapped]
        public string? Html { get => this[nameof(Html)]; set => this[nameof(Html)] = value; }
    }
}
