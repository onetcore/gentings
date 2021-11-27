using Gentings.AspNetCore.TagHelpers;
using Gentings.Extensions.Settings;
using Gentings.Storages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.Storages.TagHelpers
{
    /// <summary>
    /// 扩展名称。
    /// </summary>
    [HtmlTargetElement("gt:extension-dropdownlist")]
    public class ExtensionNameDropdownListTagHelper : DropdownListTagHelper
    {
        private readonly IMediaDirectory _mediaDirectory;
        private readonly INamedStringManager _stringManager;
        /// <summary>
        /// 初始化类<see cref="ExtensionNameDropdownListTagHelper"/>。
        /// </summary>
        /// <param name="mediaDirectory">媒体文件夹接口。</param>
        /// <param name="stringManager">字典管理接口。</param>
        public ExtensionNameDropdownListTagHelper(IMediaDirectory mediaDirectory, INamedStringManager stringManager)
        {
            _mediaDirectory = mediaDirectory;
            _stringManager = stringManager;
        }

        /// <summary>
        /// 初始化选项列表。
        /// </summary>
        /// <returns>返回选项列表。</returns>
        protected override async Task<IEnumerable<SelectListItem>> InitAsync()
        {
            var extensionNames = await _mediaDirectory.LoadExtensionNamesAsync();
            var items = new List<SelectListItem>();
            foreach (var extensionName in extensionNames)
            {
                items.Add(new SelectListItem(await _stringManager.GetOrAddStringAsync($"extensionname.{extensionName}"), extensionName));
            }

            return items;
        }
    }
}