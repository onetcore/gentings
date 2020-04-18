namespace Gentings.Data.Initializers
{
    internal class DefaultInitializerManager : InitializerManager
    {
        /// <summary>
        /// 初始化类<see cref="DefaultInitializerManager"/>。
        /// </summary>
        /// <param name="context">数据库操作接口。</param>
        public DefaultInitializerManager(IDbContext<Lisence> context) : base(context)
        {
        }
    }
}