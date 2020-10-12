namespace Gentings.Storages
{
    /// <summary>
    /// 扩展方法类。
    /// </summary>
    public static class MediaExtensions
    {
        internal static string ToStoragePath(this string md5)
        {
            return $"{md5[1]}\\{md5[3]}\\{md5[12]}\\{md5[16]}\\{md5[20]}\\{md5}.gs";
        }
    }
}