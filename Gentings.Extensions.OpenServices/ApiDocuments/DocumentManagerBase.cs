using Gentings.Documents.XmlDocuments;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace Gentings.Extensions.OpenServices.ApiDocuments
{
    /// <summary>
    /// 接口管理实现基类。
    /// </summary>
    public abstract class DocumentManagerBase : IDocumentManagerBase
    {
        private readonly IMemoryCache _cache;
        private readonly IApiDescriptionGroupCollectionProvider _provider;
        /// <summary>
        /// 初始化类<see cref="DocumentManagerBase"/>。
        /// </summary>
        /// <param name="cache">缓存接口。</param>
        /// <param name="provider">Action描述实例提供者。</param>
        protected DocumentManagerBase(IMemoryCache cache, IApiDescriptionGroupCollectionProvider provider)
        {
            _cache = cache;
            _provider = provider;
        }

        /// <summary>
        /// 获取所有API描述。
        /// </summary>
        /// <returns>返回API描述列表。</returns>
        public virtual IEnumerable<ApiDescriptor> GetApiDescriptors()
        {
            return _cache.GetOrCreate(typeof(ApiDescriptor), ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                return _provider.ApiDescriptionGroups.Items
                    .SelectMany(x => x.Items)
                    .Where(IsValidated)
                    .Select(x =>
                    {
                        var controller = x.ActionDescriptor as ControllerActionDescriptor;
                        return new ApiDescriptor
                        {
                            GroupName = x.GroupName ?? "core",
                            ControllerName = controller.ControllerName,
                            Assembly = new AssemblyInfo(controller.ControllerTypeInfo.Assembly),
                            ActionName = controller.ActionName,
                            DisplayName = controller.DisplayName,
                            RouteTemplate = x.RelativePath.ToLower(),
                            HttpMethod = x.HttpMethod,
                            RouteValues = controller.RouteValues,
                            ResponseTypes = x.SupportedResponseTypes,
                            Parameters = x.ParameterDescriptions,
                            Summary = controller.MethodInfo.GetSummary(),
                            IsAnonymous = controller.IsAnonymous(),
                        };
                    })
                    .OrderBy(x => x.Assembly.AssemblyName)
                    .ThenBy(x => x.ControllerName)
                    .ToList();
            });
        }

        /// <summary>
        /// 判断是否符合当前文档实例。
        /// </summary>
        /// <param name="descriptor">控制器操作实例。</param>
        /// <returns>返回判断结果。</returns>
        protected virtual bool IsValidated(ApiDescription descriptor)
        {
            return true;
        }

        /// <summary>
        /// 获取程序集名称。
        /// </summary>
        /// <returns>程序集名称列表。</returns>
        public virtual IEnumerable<AssemblyInfo> GetAssemblies()
        {
            return GetApiDescriptors().Select(x => x.Assembly).Distinct().OrderBy(x => x);
        }

        /// <summary>
        /// 分组获取API描述实例。
        /// </summary>
        /// <returns>返回API描述实例字典列表。</returns>
        public virtual IDictionary<string, IEnumerable<ApiDescriptor>> GetGroupApiDescriptors()
        {
            return GetApiDescriptors()
                .GroupBy(x => x.GroupName, x => x, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(x => x.Key, x => x.AsEnumerable());
        }
    }
}