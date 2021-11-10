using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;

namespace Gentings.Data.Sqlite
{
    /// <summary>
    /// Sqlite数据库。
    /// </summary>
    public class SqliteDatabase : Database
    {
        /// <summary>
        /// 初始化 <see cref="SqliteDatabase"/> 类的新实例。
        /// </summary>
        /// <param name="logger">日志接口。</param>
        /// <param name="options">配置选项。</param>
        /// <param name="sqlHelper">SQL辅助接口。</param>
        public SqliteDatabase(ILogger<SqliteDatabase> logger, IOptions<DatabaseOptions> options, ISqlHelper sqlHelper)
            : base(logger, SqliteFactory.Instance, options, sqlHelper)
        {
        }

        /// <summary>
        /// 获取数据库版本信息。
        /// </summary>
        /// <returns>返回数据库版本信息。</returns>
        public override string GetVersion()
        {
            return ExecuteScalar("SELECT sqlite_version();").ToString();
        }

        public override Task ImportAsync(DataTable table)
        {
            throw new NotImplementedException();
        }
    }
}