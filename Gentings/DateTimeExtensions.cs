﻿namespace Gentings
{
    /// <summary>
    /// 日期扩展类型。
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 显示为yyyy-MM-dd HH:mm:ss。
        /// </summary>
        /// <param name="dateTime">当前日期实例。</param>
        /// <returns>返回yyyy-MM-dd HH:mm:ss格式的日期字符串。</returns>
        public static string ToNormalString(this DateTime dateTime)
        {
            return dateTime.ToString("G");
        }

        /// <summary>
        /// 显示为yyyy-MM-dd HH:mm:ss。
        /// </summary>
        /// <param name="dateTime">当前日期实例。</param>
        /// <returns>返回yyyy-MM-dd HH:mm:ss格式的日期字符串。</returns>
        public static string? ToNormalString(this DateTime? dateTime)
        {
            return dateTime?.ToString("G");
        }

        /// <summary>
        /// 显示为yyyy-MM-dd HH:mm:ss。
        /// </summary>
        /// <param name="dateTime">当前日期实例。</param>
        /// <returns>返回yyyy-MM-dd HH:mm:ss格式的日期字符串。</returns>
        public static string ToNormalString(this DateTimeOffset dateTime)
        {
            return dateTime.ToString("G");
        }

        /// <summary>
        /// 显示为yyyy-MM-dd HH:mm:ss。
        /// </summary>
        /// <param name="dateTime">当前日期实例。</param>
        /// <returns>返回yyyy-MM-dd HH:mm:ss格式的日期字符串。</returns>
        public static string? ToNormalString(this DateTimeOffset? dateTime)
        {
            return dateTime?.ToString("G");
        }

        /// <summary>
        /// 显示为yyyy-MM-dd HH:mm。
        /// </summary>
        /// <param name="dateTime">当前日期实例。</param>
        /// <returns>返回yyyy-MM-dd HH:mm格式的日期字符串。</returns>
        public static string ToMinuteString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 显示为yyyy-MM-dd HH:mm。
        /// </summary>
        /// <param name="dateTime">当前日期实例。</param>
        /// <returns>返回yyyy-MM-dd HH:mm格式的日期字符串。</returns>
        public static string? ToMinuteString(this DateTime? dateTime)
        {
            return dateTime?.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 显示为yyyy-MM-dd HH:mm。
        /// </summary>
        /// <param name="dateTime">当前日期实例。</param>
        /// <returns>返回yyyy-MM-dd HH:mm格式的日期字符串。</returns>
        public static string ToMinuteString(this DateTimeOffset dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 显示为yyyy-MM-dd HH:mm。
        /// </summary>
        /// <param name="dateTime">当前日期实例。</param>
        /// <returns>返回yyyy-MM-dd HH:mm格式的日期字符串。</returns>
        public static string? ToMinuteString(this DateTimeOffset? dateTime)
        {
            return dateTime?.ToString("yyyy-MM-dd HH:mm");
        }
    }
}