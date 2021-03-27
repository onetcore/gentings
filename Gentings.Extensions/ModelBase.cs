using Gentings.Extensions.Notifications;
using Gentings.Extensions.Settings;

namespace Gentings.Extensions
{
    /// <summary>
    /// 页面模型基类。
    /// </summary>
    public abstract class ModelBase : AspNetCore.ModelBase
    {
        /// <summary>
        /// 获取字典字符串。
        /// </summary>
        /// <param name="key">字典唯一键。</param>
        /// <returns>返回当前字典实例。</returns>
        public string GetSettingString(string key) =>
            GetRequiredService<ISettingDictionaryManager>().GetOrAddSettings(key);

        private INotifier _notifier;
        /// <summary>
        /// 通知信息。
        /// </summary>
        protected INotifier Notifier => _notifier ??= GetRequiredService<INotifier>();
    }
}
