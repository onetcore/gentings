using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions.Sites
{
    /// <summary>
    /// 源码基类。
    /// </summary>
    public abstract class SourceBase : ExtendBase
    {
        /// <summary>
        /// 样式。
        /// </summary>
        [NotMapped]
        public string? Style { get => this[nameof(Style)]; set => this[nameof(Style)] = value; }

        /// <summary>
        /// 脚本。
        /// </summary>
        [NotMapped]
        public string? Script { get => this[nameof(Script)]; set => this[nameof(Script)] = value; }

        /// <summary>
        /// HTML代码。
        /// </summary>
        [NotMapped]
        public string? Html { get => this[nameof(Html)]; set => this[nameof(Html)] = value; }
    }
}
