using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Gentings.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Projects.APIs
{
    /// <summary>
    /// 接口管理实例。
    /// </summary>
    public interface IApiManager : ISingletonService
    {
        /// <summary>
        /// 获取所有API描述。
        /// </summary>
        /// <returns>返回API描述列表。</returns>
        IEnumerable<ApiDescriptor> GetApiDescriptors();

        /// <summary>
        /// 获取程序集名称。
        /// </summary>
        /// <returns>程序集名称列表。</returns>
        IEnumerable<AssemblyInfo> GetAssemblies();
    }

    /// <summary>
    /// 接口管理实现类。
    /// </summary>
    public class ApiManager : IApiManager
    {
        private readonly IMemoryCache _cache;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public ApiManager(IMemoryCache cache, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _cache = cache;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        /// <summary>
        /// 获取所有API描述。
        /// </summary>
        /// <returns>返回API描述列表。</returns>
        public IEnumerable<ApiDescriptor> GetApiDescriptors()
        {
            return _cache.GetOrCreate(typeof(ApiDescriptor), ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                return _actionDescriptorCollectionProvider.ActionDescriptors.Items
                    .Select(x => x as ControllerActionDescriptor)
                    .Where(x => x != null)
                    .Where(x =>
                    {
                        if (!x.ControllerTypeInfo.IsDefined(typeof(ApiControllerAttribute)))
                            return false;
                        var settings = x.ControllerTypeInfo.GetCustomAttribute<ApiExplorerSettingsAttribute>();
                        return settings?.IgnoreApi != true;
                    })
                    .Select(x => new ApiDescriptor
                    {
                        ControllerName = x.ControllerName,
                        Assembly = new AssemblyInfo(x.ControllerTypeInfo.Assembly),
                        ActionName = x.ActionName,
                        DisplayName = x.DisplayName,
                        RouteTemplate = x.AttributeRouteInfo.Template,
                        HttpMethod = x.MethodInfo.GetHttpMethod(),
                        RouteValues = x.RouteValues,
                        Parameters = x.Parameters,
                        Summary = x.MethodInfo.GetSummary()
                    })
                    .OrderBy(x => x.Assembly.AssemblyName)
                    .ThenBy(x => x.ControllerName)
                    .ToList();
            });
        }

        /// <summary>
        /// 获取程序集名称。
        /// </summary>
        /// <returns>程序集名称列表。</returns>
        public IEnumerable<AssemblyInfo> GetAssemblies()
        {
            return GetApiDescriptors().Select(x => x.Assembly).Distinct().OrderBy(x => x);
        }
    }
}