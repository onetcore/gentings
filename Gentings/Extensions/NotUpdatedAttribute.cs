using System;

namespace Gentings.Extensions
{
    /// <summary>
    /// 忽略更新。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotUpdatedAttribute : Attribute
    {

    }
}