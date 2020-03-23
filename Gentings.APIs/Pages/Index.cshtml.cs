using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Gentings.APIs.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationPartManager _applicationPartManager;

        public IndexModel(ILogger<IndexModel> logger, ApplicationPartManager applicationPartManager)
        {
            _logger = logger;
            _applicationPartManager = applicationPartManager;
        }

        public void OnGet()
        {
            var controllerFeature = new ControllerFeature();
            _applicationPartManager.PopulateFeature(controllerFeature);
            Controllers = controllerFeature.Controllers
                .Select(x => new ControllerEntity
                {
                    Namespace = x.Namespace,
                    Controller = x.FullName,
                    ModuleName = x.Module.Name,
                    Actions = x.DeclaredMethods
                        .Where(m => m.IsPublic && !m.IsDefined(typeof(NonActionAttribute)))
                        .Select(y => new ActionEntity
                        {
                            Name = y.Name,
                            ParameterCount = y.GetParameters().Length,
                            Parameters = y.GetParameters()
                                .Select(z => new ActionParameterEntity
                                {
                                    Name = z.Name,
                                    ParameterType = z.ParameterType.FullName,
                                    Position = z.Position,
                                    Attributes = z.CustomAttributes.Select(m => new AttributeEntity
                                    {
                                        FullName = m.AttributeType.FullName,
                                    })
                                })
                                .OrderBy(z => z.Name)
                        })
                        .OrderBy(y => y.Name),
                })
                .OrderBy(x => x.Controller);
        }

        /// <summary>
        /// 控制器列表。
        /// </summary>
        public IEnumerable<ControllerEntity> Controllers { get; private set; }

        /// <summary>
        /// 控制器实体。
        /// </summary>
        public class ControllerEntity
        {
            /// <summary>
            /// 命名空间。
            /// </summary>
            public string Namespace { get; set; }
            /// <summary>
            /// 控制器名称。
            /// </summary>
            public string Controller { get; set; }
            /// <summary>
            /// 程序集名称。
            /// </summary>
            public string ModuleName { get; set; }

            /// <summary>
            /// API方法。
            /// </summary>
            public IEnumerable<ActionEntity> Actions { get; set; }
        }

        /// <summary>
        /// API实体。
        /// </summary>
        public class ActionEntity
        {
            /// <summary>
            /// 行为名称。
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 参数数量。
            /// </summary>
            public int ParameterCount { get; set; }

            /// <summary>
            /// 参数列表。
            /// </summary>
            public IEnumerable<ActionParameterEntity> Parameters { get; set; }
        }

        /// <summary>
        /// 参数实体。
        /// </summary>
        public class ActionParameterEntity
        {
            /// <summary>
            /// 名称。
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 类型。
            /// </summary>
            public string ParameterType { get; set; }
            /// <summary>
            /// 位置。
            /// </summary>
            public int Position { get; set; }
            /// <summary>
            /// 特性。
            /// </summary>
            public IEnumerable<AttributeEntity> Attributes { get; set; }
        }

        /// <summary>
        /// 特性实体。
        /// </summary>
        public class AttributeEntity
        {
            /// <summary>
            /// 全名。
            /// </summary>
            public string FullName { get; set; }
        }
    }
}
