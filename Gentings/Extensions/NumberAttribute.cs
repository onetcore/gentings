namespace Gentings.Extensions
{
    /// <summary>
    /// 数值类型精度。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NumberAttribute : Attribute
    {
        /// <summary>
        /// 数据长度。
        /// </summary>
        public int Precision { get; }

        /// <summary>
        /// 小数长度。
        /// </summary>
        public int Scale { get; }

        /// <summary>
        /// 数值精度，一般用于<see cref="decimal"/>类型。
        /// </summary>
        /// <param name="precision">数据长度。</param>
        /// <param name="scale">小数长度。</param>
        public NumberAttribute(int precision = 18, int scale = 2)
        {
            Precision = precision;
            Scale = scale;
        }
    }
}