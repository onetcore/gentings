using System.Threading;
using System.Threading.Tasks;

namespace Gentings.Data.Migrations
{
    /// <summary>
    /// 数据库扩展。
    /// </summary>
    public static class DataMigrationExtensions
    {
        /// <summary>
        /// 等待数据库迁移正确完成，如果不正确将一直等待下去。
        /// </summary>
        /// <param name="cancellationToken">取消标志。</param>
        public static async Task WaitDataMigrationCompletedAsync(this CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (MigrationService.Status == MigrationStatus.Completed)
                {
                    break;
                }

                await Task.Delay(100, cancellationToken);
            }
        }
    }
}