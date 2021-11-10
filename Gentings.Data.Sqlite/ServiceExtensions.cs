using Microsoft.Extensions.DependencyInjection;
using Gentings.Data.Internal;
using Gentings.Data.Migrations;
using Gentings.Data.Migrations.Models;
using Gentings.Data.Query;
using Gentings.Data.Query.Translators;
using Gentings.Data.Sqlite.Migrations;

namespace Gentings.Data.Sqlite
{
    /// <summary>
    /// 数据库相关的服务器扩展。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加SQLite数据库服务。
        /// </summary>
        /// <param name="builder">服务集合。</param>
        /// <returns>返回服务集合实例。</returns>
        public static IServiceBuilder AddSqlite(this IServiceBuilder builder)
        {
            return builder.AddSqlite(options =>
            {
                var section = builder.Configuration.GetSection("Data");
                foreach (var current in section.GetChildren())
                {
                    switch (current.Key.ToLower())
                    {
                        case "name":
                            options.ConnectionString =
                                $"Data Source=.;Initial Catalog={current.Value};Integrated Security=True;";
                            break;
                        case "connectionstring":
                            options.ConnectionString = current.Value;
                            break;
                        case "prefix":
                            options.Prefix = current.Value;
                            break;
                        default:
                            options[current.Key] = current.Value;
                            break;
                    }
                }
            });
        }

        /// <summary>
        /// 添加SQLite数据库服务。
        /// </summary>
        /// <param name="builder">构建服务实例。</param>
        /// <param name="options">数据源选项。</param>
        /// <returns>返回服务集合实例。</returns>
        public static IServiceBuilder AddSqlite(this IServiceBuilder builder, Action<DatabaseOptions> options)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(options, nameof(options));
            var source = new DatabaseOptions();
            options(source);

            return builder.AddDataInitializer()
                .AddServices(services => services
                .AddSingleton<IDatabase, SqliteDatabase>()
                .Configure<DatabaseOptions>(o =>
                {
                    o.ConnectionString = source.ConnectionString;
                    o.Prefix = source.Prefix?.Trim();
                    o.Provider = "Sqlite";
                })
                .AddSingleton(typeof(IDbContext<>), typeof(DbContext<>))
                .AddTransient<IDataMigrator, DataMigrator>()
                .AddTransient<IMigrationRepository, SqliteMigrationRepository>()
                .AddTransient<IMigrationsSqlGenerator, SqliteMigrationsSqlGenerator>()
                .AddSingleton<IQuerySqlGenerator, SqliteQuerySqlGenerator>()
                .AddSingleton<ITypeMapper, SqliteTypeMapper>()
                .AddSingleton<ISqlHelper, SqliteHelper>()
                .AddSingleton<IMemberTranslator, SqliteCompositeMemberTranslator>()
                .AddSingleton<IMethodCallTranslator, SqliteCompositeMethodCallTranslator>()
                .AddSingleton<IExpressionFragmentTranslator, SqliteCompositeExpressionFragmentTranslator>()
                .AddSingleton<IExpressionVisitorFactory, SqliteExpressionVisitorFactory>());
        }
    }
}