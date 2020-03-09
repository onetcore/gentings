using Gentings.Data.Migrations;
using Gentings.Storages.Media;
using Microsoft.Extensions.DependencyInjection;

namespace Gentings.Storages
{
    /// <summary>
    /// 服务注册扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加文件存储服务。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>返回服务构建实例。</returns>
        public static IServiceBuilder AddMediaStorages(this IServiceBuilder builder)
        {
            return builder.AddServices(services =>
            {
                services.AddSingleton<IMediaDirectory, DefaultMediaDirectory>();
                services.AddTransient<IDataMigration, DefaultMediaDataMigration>();
                services.ConfigureOptions(typeof(ResourceOptions<IMediaDirectory>));
            });
        }

        /// <summary>
        /// 添加静态资源目录。
        /// </summary>
        /// <typeparam name="TAssemblyResourceType">程序集资源类型。</typeparam>
        /// <param name="builder">服务集合。</param>
        /// <returns>服务集合。</returns>
        /// <remarks>
        /// 1.编辑“.csproj”项目文件，添加以下代码段（将文件夹设置为嵌入资源）：
        /// <code>
        /// <![CDATA[
        ///  <ItemGroup>
        ///    <EmbeddedResource Include = "wwwroot\**" />
        ///  </ItemGroup>
        /// ]]>
        /// </code>
        /// 2.实现接口<see cref="IServiceConfigurer"/>，调用本方法注册。
        /// 
        /// 注意：资源目录为wwwroot，项目只能有一个wwwroot目录，为了不和其他程序集冲突，在wwwroot目录下文件夹最好和Areas目录下的文件夹一样。
        /// </remarks>
        public static IServiceBuilder AddResources<TAssemblyResourceType>(this IServiceBuilder builder) =>
            builder.AddServices(services => services.ConfigureOptions(typeof(ResourceOptions<TAssemblyResourceType>)));
    }
}