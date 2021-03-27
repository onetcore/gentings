using System.Threading.Tasks;
using Gentings.Extensions.Notifications;
using Gentings.Extensions.Properties;
using Gentings.Extensions.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions
{
    /// <summary>
    /// 控制器基类。
    /// </summary>
    public abstract class ControllerBase : AspNetCore.ControllerBase
    {
        /// <summary>
        /// 从表单中读取扩展属性。
        /// </summary>
        /// <param name="extend">扩展实例。</param>
        /// <param name="form">表单集合。</param>
        protected void Merge(ExtendBase extend, IFormCollection form)
        {
            foreach (var key in form.Keys)
            {
                if (key.StartsWith("ex:"))
                {
                    extend[key[3..]] = form[key];
                }
            }
        }

        /// <summary>
        /// 获取网站配置。
        /// </summary>
        /// <returns>返回网站配置实例。</returns>
        protected async Task<IActionResult> GetSettingsAsync<TSettings>()
            where TSettings : class, new()
        {
            var settings = await GetRequiredService<ISettingsManager>().GetSettingsAsync<TSettings>();
            return OkResult(settings);
        }

        /// <summary>
        /// 保存网站配置。
        /// </summary>
        /// <param name="settings">配置实例。</param>
        /// <param name="name">配置名称，用于日志保存。</param>
        /// <returns>返回保存结果。</returns>
        protected async Task<IActionResult> SaveSettingsAsync<TSettings>(TSettings settings, string name)
            where TSettings : class, new()
        {
            var result = await GetRequiredService<ISettingsManager>().SaveSettingsAsync(settings);
            if (result)
            {
                await LogAsync(Resources.SaveSettings, name);
                return OkResult();
            }
            return BadResult(Resources.SaveSettingsFailured);
        }

        private INotifier _notifier;
        /// <summary>
        /// 通知信息。
        /// </summary>
        protected INotifier Notifier => _notifier ??= GetRequiredService<INotifier>();
    }
}