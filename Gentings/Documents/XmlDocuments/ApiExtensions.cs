namespace Gentings.Documents.XmlDocuments
{
    /// <summary>
    /// API扩展类。
    /// </summary>
    public static class ApiExtensions
    {
        /// <summary>
        /// 获取当前类型实例的JSON字符串。
        /// </summary>
        /// <param name="type">类型实例。</param>
        /// <param name="defaultValue">默认字符串。</param>
        /// <returns>返回JSON字符串。</returns>
        public static string ToJsonString(this Type type, string defaultValue)
        {
            var instance = Activator.CreateInstance(type);
            return instance?.ToJsonString() ?? defaultValue;
        }
    }
}