using System.Threading;
using System.Threading.Tasks;

namespace Gentings
{
    /// <summary>
    /// 后台服务基类。
    /// </summary>
    public abstract class BackgroundService : Microsoft.Extensions.Hosting.BackgroundService, IServices
    {
        /// <summary>
        /// 执行后台服务。
        /// 注意：如果在含有数据库的后台服务，需要调用如下代码以等待数据库迁移结束后再执行。
        /// <![CDATA[
        /// await cancellationToken.WaitDataMigrationCompletedAsync();
        /// ]]>
        /// </summary>
        /// <param name="cancellationToken">取消标志。</param>
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}