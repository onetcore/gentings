using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions
{
    /// <summary>
    /// 自增长特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IdentityAttribute : DatabaseGeneratedAttribute
    {
        /// <summary>
        /// 标识种子。
        /// </summary>
        public long Seed { get; }

        /// <summary>
        /// 标识增量。
        /// </summary>
        public int Step { get; }

        /// <summary>初始化 <see cref="IdentityAttribute" /> 类的新实例。</summary>
        /// <param name="seed">标识种子。</param>
        /// <param name="Step">标识增量。</param>
        public IdentityAttribute(long seed = 1L, int Step = 1) : base(DatabaseGeneratedOption.Identity)
        {
            Seed = seed;
            this.Step = Step;
        }
    }
}