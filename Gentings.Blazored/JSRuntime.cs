using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Gentings.Blazored
{
    /// <summary>
    /// 提供给后端调用的脚本函数。
    /// </summary>
    public class JSRuntime : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
        /// <summary>
        /// 初始化类<see cref="JSRuntime"/>。
        /// </summary>
        /// <param name="jsRuntime">脚本运行时。</param>
        public JSRuntime(IJSRuntime jsRuntime)
        {
            _moduleTask = new Lazy<Task<IJSObjectReference>>(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/Gentings.Blazored/gtcore.js").AsTask());
        }

        /// <summary>
        /// 执行脚本方法。
        /// </summary>
        /// <param name="method">方法名称。</param>
        /// <param name="args">参数。</param>
        public async ValueTask ExecuteAsync(string method, params object[] args)
        {
            var module = await _moduleTask.Value;
            await module.InvokeVoidAsync(method, args);
        }

        /// <summary>
        /// 执行脚本方法，并且返回数据。
        /// </summary>
        /// <typeparam name="TValue">返回的数据类型。</typeparam>
        /// <param name="method">方法名称。</param>
        /// <param name="args">参数。</param>
        /// <returns>返回数据实例。</returns>
        public async ValueTask<TValue> GetResultAsync<TValue>(string method, params object[] args)
        {
            var module = await _moduleTask.Value;
            return await module.InvokeAsync<TValue>(method, args);
        }

        /// <summary>
        /// 获取文档标题。
        /// </summary>
        /// <returns>返回文档标题。</returns>
        public ValueTask<string> GetTitleAsync() => GetResultAsync<string>("getTitle");

        /// <summary>
        /// 设置文档标题。
        /// </summary>
        /// <param name="title">文档标题。</param>
        public ValueTask SetTitleAsync(string title) => ExecuteAsync("setTitle", title);

        /// <summary>
        /// 设置或移除元素的属性。
        /// </summary>
        /// <param name="selector">元素选择器。</param>
        /// <param name="name">属性名称。</param>
        /// <param name="value">属性值，当值为false或者null时候将移除属性。</param>
        public ValueTask SetAttributeAsync(string selector, string name, object value)
        {
            return ExecuteAsync("setAttribute", selector, name, value);
        }

        /// <summary>
        /// 设置元素的属性。
        /// </summary>
        /// <param name="selector">元素选择器。</param>
        /// <param name="name">属性名称。</param>
        public ValueTask<string> GetAttributeAsync(string selector, string name)
        {
            return GetResultAsync<string>("getAttribute", selector, name);
        }

        /// <summary>
        /// 获取当前本地存储实例。
        /// </summary>
        /// <param name="key">存储唯一键。</param>
        /// <returns>返回当前本地存储实例。</returns>
        public ValueTask<string> GetLocalStorageAsync(string key) => GetResultAsync<string>("getLocalStorage", key);

        /// <summary>
        /// 获取当前本地存储实例。
        /// </summary>
        /// <typeparam name="TValue">返回类型实例。</typeparam>
        /// <param name="key">存储唯一键。</param>
        /// <returns>返回当前本地存储实例。</returns>
        public async ValueTask<TValue> GetLocalStorageAsync<TValue>(string key)
        {
            var data = await GetLocalStorageAsync(key);
            if (string.IsNullOrEmpty(data))
                return default;
            return JsonSerializer.Deserialize<TValue>(data);
        }

        /// <summary>
        /// 设置本地存储实例。
        /// </summary>
        /// <param name="key">存储唯一键。</param>
        /// <param name="data">数据字符串。</param>
        /// <returns>返回当前存储任务。</returns>
        public ValueTask SetLocalStorageAsync(string key, string data = null) => ExecuteAsync("setLocalStorage", key, data);

        /// <summary>
        /// 设置本地存储实例。
        /// </summary>
        /// <param name="key">存储唯一键。</param>
        /// <param name="data">数据字符串。</param>
        /// <returns>返回当前存储任务。</returns>
        public ValueTask SetLocalStorageAsync(string key, object data) =>
            SetLocalStorageAsync(key, JsonSerializer.Serialize(data));

        /// <summary>
        /// 获取当前本地Session实例。
        /// </summary>
        /// <param name="key">存储唯一键。</param>
        /// <returns>返回当前本地Session实例。</returns>
        public ValueTask<string> GetSessionStorageAsync(string key) => GetResultAsync<string>("getSessionStorage", key);

        /// <summary>
        /// 获取当前本地Session实例。
        /// </summary>
        /// <typeparam name="TValue">返回类型实例。</typeparam>
        /// <param name="key">存储唯一键。</param>
        /// <returns>返回当前本地Session实例。</returns>
        public async ValueTask<TValue> GetSessionStorageAsync<TValue>(string key)
        {
            var data = await GetSessionStorageAsync(key);
            if (string.IsNullOrEmpty(data))
                return default;
            return JsonSerializer.Deserialize<TValue>(data);
        }

        /// <summary>
        /// 设置本地Session实例。
        /// </summary>
        /// <param name="key">存储唯一键。</param>
        /// <param name="data">数据字符串。</param>
        /// <returns>返回当前Session任务。</returns>
        public ValueTask SetSessionStorageAsync(string key, string data = null) => ExecuteAsync("setSessionStorage", key, data);

        /// <summary>
        /// 设置本地Session实例。
        /// </summary>
        /// <param name="key">存储唯一键。</param>
        /// <param name="data">数据实例。</param>
        /// <returns>返回当前Session任务。</returns>
        public ValueTask SetSessionStorageAsync(string key, object data) =>
            SetSessionStorageAsync(key, JsonSerializer.Serialize(data));

        /// <summary>
        /// 添加样式。
        /// </summary>
        /// <param name="selector">元素选择器。</param>
        /// <param name="className">样式名称。</param>
        public ValueTask AddClass(string selector, string className) => ExecuteAsync("addClass", selector, className);

        /// <summary>
        /// 删除样式。
        /// </summary>
        /// <param name="selector">元素选择器。</param>
        /// <param name="className">样式名称。</param>
        public ValueTask RemoveClass(string selector, string className) => ExecuteAsync("removeClass", selector, className);

        /// <summary>
        /// 替换样式。
        /// </summary>
        /// <param name="selector">元素选择器。</param>
        /// <param name="className">样式名称。</param>
        public ValueTask ToggleClass(string selector, string className) => ExecuteAsync("toggleClass", selector, className);

        /// <summary>
        /// 判断是否包含样式。
        /// </summary>
        /// <param name="selector">元素选择器。</param>
        /// <param name="className">样式名称。</param>
        /// <returns>返回判断结果。</returns>
        public ValueTask<bool> HasClass(string selector, string className) => GetResultAsync<bool>("hasClass", selector, className);

        /// <summary>
        /// 释放资源。
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
