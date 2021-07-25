using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Gentings.Blazored
{
    /// <summary>
    /// �ṩ����˵��õĽű�������
    /// </summary>
    public class JSRuntime : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
        /// <summary>
        /// ��ʼ����<see cref="JSRuntime"/>��
        /// </summary>
        /// <param name="jsRuntime">�ű�����ʱ��</param>
        public JSRuntime(IJSRuntime jsRuntime)
        {
            _moduleTask = new Lazy<Task<IJSObjectReference>>(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/Gentings.Blazored/gtcore.js").AsTask());
        }

        /// <summary>
        /// ִ�нű�������
        /// </summary>
        /// <param name="method">�������ơ�</param>
        /// <param name="args">������</param>
        public async ValueTask ExecuteAsync(string method, params object[] args)
        {
            var module = await _moduleTask.Value;
            await module.InvokeVoidAsync(method, args);
        }

        /// <summary>
        /// ִ�нű����������ҷ������ݡ�
        /// </summary>
        /// <typeparam name="TValue">���ص��������͡�</typeparam>
        /// <param name="method">�������ơ�</param>
        /// <param name="args">������</param>
        /// <returns>��������ʵ����</returns>
        public async ValueTask<TValue> GetResultAsync<TValue>(string method, params object[] args)
        {
            var module = await _moduleTask.Value;
            return await module.InvokeAsync<TValue>(method, args);
        }

        /// <summary>
        /// ��ȡ�ĵ����⡣
        /// </summary>
        /// <returns>�����ĵ����⡣</returns>
        public ValueTask<string> GetTitleAsync() => GetResultAsync<string>("getTitle");

        /// <summary>
        /// �����ĵ����⡣
        /// </summary>
        /// <param name="title">�ĵ����⡣</param>
        public ValueTask SetTitleAsync(string title) => ExecuteAsync("setTitle", title);

        /// <summary>
        /// ���û��Ƴ�Ԫ�ص����ԡ�
        /// </summary>
        /// <param name="selector">Ԫ��ѡ������</param>
        /// <param name="name">�������ơ�</param>
        /// <param name="value">����ֵ����ֵΪfalse����nullʱ���Ƴ����ԡ�</param>
        public ValueTask SetAttributeAsync(string selector, string name, object value)
        {
            return ExecuteAsync("setAttribute", selector, name, value);
        }

        /// <summary>
        /// ����Ԫ�ص����ԡ�
        /// </summary>
        /// <param name="selector">Ԫ��ѡ������</param>
        /// <param name="name">�������ơ�</param>
        public ValueTask<string> GetAttributeAsync(string selector, string name)
        {
            return GetResultAsync<string>("getAttribute", selector, name);
        }

        /// <summary>
        /// ��ȡ��ǰ���ش洢ʵ����
        /// </summary>
        /// <param name="key">�洢Ψһ����</param>
        /// <returns>���ص�ǰ���ش洢ʵ����</returns>
        public ValueTask<string> GetLocalStorageAsync(string key) => GetResultAsync<string>("getLocalStorage", key);

        /// <summary>
        /// ��ȡ��ǰ���ش洢ʵ����
        /// </summary>
        /// <typeparam name="TValue">��������ʵ����</typeparam>
        /// <param name="key">�洢Ψһ����</param>
        /// <returns>���ص�ǰ���ش洢ʵ����</returns>
        public async ValueTask<TValue> GetLocalStorageAsync<TValue>(string key)
        {
            var data = await GetLocalStorageAsync(key);
            if (string.IsNullOrEmpty(data))
                return default;
            return JsonSerializer.Deserialize<TValue>(data);
        }

        /// <summary>
        /// ���ñ��ش洢ʵ����
        /// </summary>
        /// <param name="key">�洢Ψһ����</param>
        /// <param name="data">�����ַ�����</param>
        /// <returns>���ص�ǰ�洢����</returns>
        public ValueTask SetLocalStorageAsync(string key, string data = null) => ExecuteAsync("setLocalStorage", key, data);

        /// <summary>
        /// ���ñ��ش洢ʵ����
        /// </summary>
        /// <param name="key">�洢Ψһ����</param>
        /// <param name="data">�����ַ�����</param>
        /// <returns>���ص�ǰ�洢����</returns>
        public ValueTask SetLocalStorageAsync(string key, object data) =>
            SetLocalStorageAsync(key, JsonSerializer.Serialize(data));

        /// <summary>
        /// ��ȡ��ǰ����Sessionʵ����
        /// </summary>
        /// <param name="key">�洢Ψһ����</param>
        /// <returns>���ص�ǰ����Sessionʵ����</returns>
        public ValueTask<string> GetSessionStorageAsync(string key) => GetResultAsync<string>("getSessionStorage", key);

        /// <summary>
        /// ��ȡ��ǰ����Sessionʵ����
        /// </summary>
        /// <typeparam name="TValue">��������ʵ����</typeparam>
        /// <param name="key">�洢Ψһ����</param>
        /// <returns>���ص�ǰ����Sessionʵ����</returns>
        public async ValueTask<TValue> GetSessionStorageAsync<TValue>(string key)
        {
            var data = await GetSessionStorageAsync(key);
            if (string.IsNullOrEmpty(data))
                return default;
            return JsonSerializer.Deserialize<TValue>(data);
        }

        /// <summary>
        /// ���ñ���Sessionʵ����
        /// </summary>
        /// <param name="key">�洢Ψһ����</param>
        /// <param name="data">�����ַ�����</param>
        /// <returns>���ص�ǰSession����</returns>
        public ValueTask SetSessionStorageAsync(string key, string data = null) => ExecuteAsync("setSessionStorage", key, data);

        /// <summary>
        /// ���ñ���Sessionʵ����
        /// </summary>
        /// <param name="key">�洢Ψһ����</param>
        /// <param name="data">����ʵ����</param>
        /// <returns>���ص�ǰSession����</returns>
        public ValueTask SetSessionStorageAsync(string key, object data) =>
            SetSessionStorageAsync(key, JsonSerializer.Serialize(data));

        /// <summary>
        /// �����ʽ��
        /// </summary>
        /// <param name="selector">Ԫ��ѡ������</param>
        /// <param name="className">��ʽ���ơ�</param>
        public ValueTask AddClass(string selector, string className) => ExecuteAsync("addClass", selector, className);

        /// <summary>
        /// ɾ����ʽ��
        /// </summary>
        /// <param name="selector">Ԫ��ѡ������</param>
        /// <param name="className">��ʽ���ơ�</param>
        public ValueTask RemoveClass(string selector, string className) => ExecuteAsync("removeClass", selector, className);

        /// <summary>
        /// �滻��ʽ��
        /// </summary>
        /// <param name="selector">Ԫ��ѡ������</param>
        /// <param name="className">��ʽ���ơ�</param>
        public ValueTask ToggleClass(string selector, string className) => ExecuteAsync("toggleClass", selector, className);

        /// <summary>
        /// �ж��Ƿ������ʽ��
        /// </summary>
        /// <param name="selector">Ԫ��ѡ������</param>
        /// <param name="className">��ʽ���ơ�</param>
        /// <returns>�����жϽ����</returns>
        public ValueTask<bool> HasClass(string selector, string className) => GetResultAsync<bool>("hasClass", selector, className);

        /// <summary>
        /// �ͷ���Դ��
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
