using Microsoft.Extensions.DependencyInjection;

namespace Gentings.AspNetCore.Options
{
    /// <summary>
    /// 服务注册扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加静态资源目录。
        /// 
        /// 注意：
        /// 1.资源目录为wwwroot，项目只能有一个wwwroot目录，为了不和其他程序集冲突，在wwwroot目录下文件夹最好和Areas目录下的文件夹一样。
        /// 2.由于VS打包资源文件会把目录非法字符转换为“_”，如：“lib/font-awesome/css/font-awesome.min.css”将被转换为“lib/font_awesome/css/font-awesome.min.css”，
        ///   所以尽量不要在文件夹上面使用非字母名称。
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
        /// </remarks>
        public static IServiceBuilder AddResources<TAssemblyResourceType>(this IServiceBuilder builder) =>
            builder.AddServices(services => services.ConfigureOptions(typeof(ResourceOptions<TAssemblyResourceType>)));
    }
}