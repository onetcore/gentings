using System;
using Microsoft.Extensions.DependencyInjection;
using Gentings.Data.Internal;
using Gentings.Data.Migrations;
using Gentings.Data.Migrations.Models;
using Gentings.Data.Query;
using Gentings.Data.Query.Translators;
using Gentings.Data.SqlServer.Migrations;
using Gentings.Data.SqlServer.Query;
using Gentings.Data.SqlServer.Query.Translators;

namespace Gentings.Data.SqlServer
{
    /// <summary>
    /// 数据库相关的服务器扩展。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加SQLServer数据库服务。
        /// </summary>
        /// <param name="builder">服务集合。</param>
        /// <returns>返回服务集合实例。</returns>
        public static IServiceBuilder AddSqlServer(this IServiceBuilder builder)
        {
            return builder.AddSqlServer(options =>
            {
                var section = builder.Configuration.GetSection("Data");
                foreach (var current in section.GetChildren())
                {
                    switch (current.Key.ToLower())
                    {
                        case "name":
                            options.ConnectionString = $"Data Source=.;Initial Catalog={current.Value};Integrated Security=True;";
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
        /// 添加SQLServer数据库服务。
        /// </summary>
        /// <param name="builder">构建服务实例。</param>
        /// <param name="options">数据源选项。</param>
        /// <returns>返回服务集合实例。</returns>
        public static IServiceBuilder AddSqlServer(this IServiceBuilder builder, Action<DatabaseOptions> options)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(options, nameof(options));
            var source = new DatabaseOptions();
            options(source);

            return builder.AddServices(services => services
                    .AddSingleton<IDatabase, SqlServerDatabase>()
                    .Configure<DatabaseOptions>(o =>
                    {
                        o.ConnectionString = source.ConnectionString;
                        o.Prefix = source.Prefix?.Trim();
                        o.Provider = "SqlServer";
                    })
                    .AddSingleton(typeof(IDbContext<>), typeof(DbContext<>))
                    .AddTransient<IDataMigrator, DataMigrator>()
                    .AddTransient<IMigrationRepository, SqlServerMigrationRepository>()
                    .AddTransient<IMigrationsSqlGenerator, SqlServerMigrationsSqlGenerator>()
                    .AddSingleton<IQuerySqlGenerator, SqlServerQuerySqlGenerator>()
                    .AddSingleton<ITypeMapper, SqlServerTypeMapper>()
                    .AddSingleton<ISqlHelper, SqlServerHelper>()
                    .AddSingleton<IMemberTranslator, SqlServerCompositeMemberTranslator>()
                    .AddSingleton<IMethodCallTranslator, SqlServerCompositeMethodCallTranslator>()
                    .AddSingleton<IExpressionFragmentTranslator, SqlServerCompositeExpressionFragmentTranslator>()
                    .AddSingleton<IExpressionVisitorFactory, SqlServerExpressionVisitorFactory>());
        }
    }
}