﻿using Gentings.Security;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gentings.Extensions.Emails
{
    /// <summary>
    /// 电子邮件管理接口。
    /// </summary>
    public interface IEmailManager
    {
        /// <summary>
        /// 获取资源，一般为内容。
        /// </summary>
        /// <param name="resourceKey">资源键。</param>
        /// <param name="replacement">替换对象，使用匿名类型实例。</param>
        /// <param name="resourceType">资源所在程序集的类型。</param>
        /// <returns>返回模板文本字符串。</returns>
        string GetTemplate(string resourceKey, object replacement = null, Type resourceType = null);

        /// <summary>
        /// 更新列。
        /// </summary>
        /// <param name="id">当前Id。</param>
        /// <param name="fields">更新匿名对象。</param>
        /// <returns>返回更新结果。</returns>
        bool Update(int id, object fields);

        /// <summary>
        /// 更新列。
        /// </summary>
        /// <param name="id">当前Id。</param>
        /// <param name="fields">更新匿名对象。</param>
        /// <returns>返回更新结果。</returns>
        Task<bool> UpdateAsync(int id, object fields);

        /// <summary>
        /// 删除邮件。
        /// </summary>
        /// <param name="ids">邮件Id列表。</param>
        /// <returns>返回删除结果。</returns>
        Task<bool> DeleteAsync(int[] ids);

        /// <summary>
        /// 添加电子邮件接口。
        /// </summary>
        /// <param name="message">电子邮件实例对象。</param>
        /// <returns>返回添加结果。</returns>
        bool Save(Email message);

        /// <summary>
        /// 添加电子邮件接口。
        /// </summary>
        /// <param name="message">电子邮件实例对象。</param>
        /// <returns>返回添加结果。</returns>
        Task<bool> SaveAsync(Email message);

        /// <summary>
        /// 判断电子邮件是否已经存在，用<see cref="Email.HashKey"/>判断。
        /// </summary>
        /// <param name="message">电子邮件实例对象。</param>
        /// <param name="expiredSeconds">过期时间（秒）。</param>
        /// <returns>返回判断结果。</returns>
        bool IsExisted(Email message, int expiredSeconds = 300);

        /// <summary>
        /// 判断电子邮件是否已经存在，用<see cref="Email.HashKey"/>判断。
        /// </summary>
        /// <param name="message">电子邮件实例对象。</param>
        /// <param name="expiredSeconds">过期时间（秒）。</param>
        /// <returns>返回判断结果。</returns>
        Task<bool> IsExistedAsync(Email message, int expiredSeconds = 300);

        /// <summary>
        /// 发送电子邮件。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="emailAddress">电子邮件地址。</param>
        /// <param name="title">标题。</param>
        /// <param name="content">内容。</param>
        /// <param name="action">实例化方法。</param>
        /// <returns>返回发送结果。</returns>
        bool SendEmail(int userId, string emailAddress, string title, string content, Action<Email> action = null);

        /// <summary>
        /// 发送电子邮件。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="emailAddress">电子邮件地址。</param>
        /// <param name="title">标题。</param>
        /// <param name="content">内容。</param>
        /// <param name="action">实例化方法。</param>
        /// <returns>返回发送结果。</returns>
        Task<bool> SendEmailAsync(int userId, string emailAddress, string title, string content, Action<Email> action = null);

        /// <summary>
        /// 发送电子邮件。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="resourceKey">资源键：<paramref name="resourceKey"/>_{Title}，<paramref name="resourceKey"/>_{Content}。</param>
        /// <param name="replacement">替换对象，使用匿名类型实例。</param>
        /// <param name="resourceType">资源所在程序集的类型。</param>
        /// <param name="action">实例化方法。</param>
        /// <returns>返回发送结果。</returns>
        bool SendEmail(IUser user, string resourceKey, object replacement = null, Type resourceType = null, Action<Email> action = null);

        /// <summary>
        /// 发送电子邮件。
        /// </summary>
        /// <param name="user">用户实例。</param>
        /// <param name="resourceKey">资源键：<paramref name="resourceKey"/>_{Title}，<paramref name="resourceKey"/>_{Content}。</param>
        /// <param name="replacement">替换对象，使用匿名类型实例。</param>
        /// <param name="resourceType">资源所在程序集的类型。</param>
        /// <param name="action">实例化方法。</param>
        /// <returns>返回发送结果。</returns>
        Task<bool> SendEmailAsync(IUser user, string resourceKey, object replacement = null, Type resourceType = null, Action<Email> action = null);

        /// <summary>
        /// 加载电子邮件列表。
        /// </summary>
        /// <param name="status">状态。</param>
        /// <returns>返回电子邮件列表。</returns>
        IEnumerable<Email> Load(EmailStatus? status = null);

        /// <summary>
        /// 加载电子邮件列表。
        /// </summary>
        /// <param name="status">状态。</param>
        /// <returns>返回电子邮件列表。</returns>
        Task<IEnumerable<Email>> LoadAsync(EmailStatus? status = null);

        /// <summary>
        /// 加载电子邮件列表。
        /// </summary>
        /// <param name="query">电子邮件查询类型。</param>
        /// <returns>返回电子邮件列表。</returns>
        IPageEnumerable<Email> Load<TQuery>(TQuery query) where TQuery : EmailQuery;

        /// <summary>
        /// 加载电子邮件列表。
        /// </summary>
        /// <param name="query">电子邮件查询类型。</param>
        /// <returns>返回电子邮件列表。</returns>
        Task<IPageEnumerable<Email>> LoadAsync<TQuery>(TQuery query) where TQuery : EmailQuery;

        /// <summary>
        /// 设置失败状态。
        /// </summary>
        /// <param name="id">当前电子邮件Id。</param>
        /// <param name="maxTryTimes">最大失败次数。</param>
        /// <returns>返回设置结果。</returns>
        bool SetFailured(int id, int maxTryTimes);

        /// <summary>
        /// 设置失败状态。
        /// </summary>
        /// <param name="id">当前电子邮件Id。</param>
        /// <param name="maxTryTimes">最大失败次数。</param>
        /// <returns>返回设置结果。</returns>
        Task<bool> SetFailuredAsync(int id, int maxTryTimes);

        /// <summary>
        /// 设置成功状态。
        /// </summary>
        /// <param name="id">当前电子邮件Id。</param>
        /// <param name="settingsId">配置ID。</param>
        /// <returns>返回设置结果。</returns>
        bool SetSuccess(int id, int settingsId);

        /// <summary>
        /// 设置成功状态。
        /// </summary>
        /// <param name="id">当前电子邮件Id。</param>
        /// <param name="settingsId">配置ID。</param>
        /// <returns>返回设置结果。</returns>
        Task<bool> SetSuccessAsync(int id, int settingsId);

        /// <summary>
        /// 通过Id查询电子邮件。
        /// </summary>
        /// <param name="id">电子邮件id。</param>
        /// <returns>返回电子邮件实例。</returns>
        Email Find(int id);

        /// <summary>
        /// 通过Id查询电子邮件。
        /// </summary>
        /// <param name="id">电子邮件id。</param>
        /// <returns>返回电子邮件实例。</returns>
        Task<Email> FindAsync(int id);
    }
}