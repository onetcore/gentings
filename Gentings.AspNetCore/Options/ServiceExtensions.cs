using Gentings.Security;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace Gentings.AspNetCore.Options
{
    /// <summary>
    /// 服务注册扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 附加上用户模型特性实例。
        /// </summary>
        /// <typeparam name="TUser">模型类型。</typeparam>
        /// <param name="builder">服务构建实例对象。</param>
        /// <returns>返回服务构建实例对象。</returns>
        public static IServiceBuilder AddModelUI<TUser>(this IServiceBuilder builder)
            where TUser : class, IUser
        {
            return builder.AddModelUI<TUserModelUIAttribute, TUser>((pcc, c) => pcc.Add(c));
        }

        /// <summary>
        /// 附加上模型特性实例。
        /// </summary>
        /// <typeparam name="TModelUIAttribute">模型特性类型。</typeparam>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="builder">服务构建实例对象。</param>
        /// <returns>返回服务构建实例对象。</returns>
        public static IServiceBuilder AddModelUI<TModelUIAttribute, TModel>(this IServiceBuilder builder)
            where TModel : class
            where TModelUIAttribute : ModelUIAttribute
        {
            return builder.AddModelUI<TModelUIAttribute, TModel>((pcc, c) => pcc.Add(c));
        }

        /// <summary>
        /// 附加上模型特性实例。
        /// </summary>
        /// <typeparam name="TModelUIAttribute">模型特性类型。</typeparam>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="builder">服务构建实例对象。</param>
        /// <param name="action">Razor页面转换操作方法。</param>
        /// <returns>返回服务构建实例对象。</returns>
        public static IServiceBuilder AddModelUI<TModelUIAttribute, TModel>(this IServiceBuilder builder, Action<PageConventionCollection, IPageApplicationModelConvention> action)
            where TModel : class
            where TModelUIAttribute : ModelUIAttribute
        {
            var convention = new ModelUIConvention<TModelUIAttribute, TModel>();
            return builder.AddServices(services => services.PostConfigureAll<RazorPagesOptions>(options => action(options.Conventions, convention)));
        }

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
        public static IServiceBuilder AddResources<TAssemblyResourceType>(this IServiceBuilder builder)
        {
            return builder.AddServices(services => services.ConfigureOptions(typeof(ResourceOptions<TAssemblyResourceType>)));
        }
    }
}