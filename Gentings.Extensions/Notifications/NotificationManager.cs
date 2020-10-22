using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gentings.Data;
using Gentings.Settings;
using Microsoft.AspNetCore.Http;

namespace Gentings.Extensions.Notifications
{
    /// <summary>
    /// 通知管理实现类。
    /// </summary>
    public abstract class NotificationManager : ObjectManager<Notification>, INotificationManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISettingsManager _settingsManager;

        /// <summary>
        /// 初始化类<see cref="NotificationManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="httpContextAccessor">Http上下文访问接口。</param>
        /// <param name="settingsManager">配置管理接口。</param>
        protected NotificationManager(IDbContext<Notification> context, IHttpContextAccessor httpContextAccessor, ISettingsManager settingsManager) : base(context)
        {
            _httpContextAccessor = httpContextAccessor;
            _settingsManager = settingsManager;
        }

        /// <summary>
        /// 当前用户ID。
        /// </summary>
        protected int UserId => _httpContextAccessor.HttpContext.User.GetUserId();

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回判断结果。</returns>
        public override bool IsDuplicated(Notification model)
        {
            var date = DateTimeOffset.Now.AddHours(-_settingsManager.GetSettings<NotificationSettings>().DuplicateHours);
            return Context.Any(x => x.UserId == model.UserId && x.Message == model.Message && x.CreatedDate > date);
        }

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回判断结果。</returns>
        /// <param name="cancellationToken">取消标识。</param>
        public override async Task<bool> IsDuplicatedAsync(Notification model, CancellationToken cancellationToken = default)
        {
            var settings = await _settingsManager.GetSettingsAsync<NotificationSettings>();
            var date = DateTimeOffset.Now.AddHours(-settings.DuplicateHours);
            return await Context.AnyAsync(x => x.UserId == model.UserId && x.Message == model.Message && x.CreatedDate > date, cancellationToken);
        }

        /// <summary>
        /// 加载当前用户最新得通知。
        /// </summary>
        /// <param name="size">通知记录数。</param>
        /// <returns>返回通知列表。</returns>
        public virtual IEnumerable<Notification> Load(int size = 0)
        {
            if (size == 0)
            {
                size = _settingsManager.GetSettings<NotificationSettings>().MaxSize;
            }

            return Context.AsQueryable()
                .WithNolock()
                .InnerJoin<NotificationType>((n, t) => n.TypeId == t.Id)
                .Select()
                .Select<NotificationType>(x => new { x.IconUrl, x.Name })
                .OrderByDescending(x => x.CreatedDate)
                .Where(x => x.UserId == UserId)
                .AsEnumerable(size);
        }

        /// <summary>
        /// 加载当前用户最新得通知。
        /// </summary>
        /// <param name="size">通知记录数。</param>
        /// <returns>返回通知列表。</returns>
        public virtual async Task<IEnumerable<Notification>> LoadAsync(int size = 0)
        {
            if (size == 0)
            {
                size = (await _settingsManager.GetSettingsAsync<NotificationSettings>()).MaxSize;
            }

            return await Context.AsQueryable()
                .WithNolock()
                .InnerJoin<NotificationType>((n, t) => n.TypeId == t.Id)
                .Select()
                .Select<NotificationType>(x => new { x.IconUrl, x.Name })
                .OrderByDescending(x => x.CreatedDate)
                .Where(x => x.UserId == UserId)
                .AsEnumerableAsync(size);
        }

        /// <summary>
        /// 获取当前用户得通知数量。
        /// </summary>
        /// <returns>返回通知得数量。</returns>
        public virtual int GetSize()
        {
            return Context.Count(x => x.UserId == UserId);
        }

        /// <summary>
        /// 获取当前用户得通知数量。
        /// </summary>
        /// <returns>返回通知得数量。</returns>
        public virtual Task<int> GetSizeAsync()
        {
            return Context.CountAsync(x => x.UserId == UserId);
        }

        /// <summary>
        /// 保存对象实例。
        /// </summary>
        /// <param name="userId">用户列表。</param>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回保存结果。</returns>
        public virtual DataResult Save(int[] userId, Notification model)
        {
            var settings = _settingsManager.GetSettings<NotificationSettings>();
            var date = DateTimeOffset.Now.AddHours(-settings.DuplicateHours);
            if (Context.BeginTransaction(db =>
            {
                foreach (var id in userId)
                {
                    model.Id = 0;
                    model.UserId = id;
                    if (db.Any(x => x.UserId == model.UserId && x.Message == model.Message && x.CreatedDate > date))
                        continue;
                    db.Create(model);
                }
                return true;
            }))
            {
                return DataAction.Created;
            }

            return DataAction.CreatedFailured;
        }

        /// <summary>
        /// 保存对象实例。
        /// </summary>
        /// <param name="userId">用户列表。</param>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回保存结果。</returns>
        public virtual async Task<DataResult> SaveAsync(int[] userId, Notification model)
        {
            var settings = await _settingsManager.GetSettingsAsync<NotificationSettings>();
            var date = DateTimeOffset.Now.AddHours(-settings.DuplicateHours);
            if (await Context.BeginTransactionAsync(async db =>
            {
                foreach (var id in userId)
                {
                    model.Id = 0;
                    model.UserId = id;
                    if (await db.AnyAsync(x => x.UserId == model.UserId && x.Message == model.Message && x.CreatedDate > date))
                        continue;
                    await db.CreateAsync(model);
                }
                return true;
            }))
            {
                return DataAction.Created;
            }

            return DataAction.CreatedFailured;
        }

        /// <summary>
        /// 设置状态。
        /// </summary>
        /// <param name="id">通知id。</param>
        /// <param name="status">状态。</param>
        /// <returns>返回设置结果。</returns>
        public virtual bool SetStatus(int id, NotificationStatus status)
        {
            return Context.Update(id, new { status });
        }

        /// <summary>
        /// 设置状态。
        /// </summary>
        /// <param name="id">通知id。</param>
        /// <param name="status">状态。</param>
        /// <returns>返回设置结果。</returns>
        public virtual Task<bool> SetStatusAsync(int id, NotificationStatus status)
        {
            return Context.UpdateAsync(id, new { status });
        }

        /// <summary>
        /// 设置状态。
        /// </summary>
        /// <param name="ids">通知id。</param>
        /// <param name="status">状态。</param>
        /// <returns>返回设置结果。</returns>
        public virtual bool SetStatus(int[] ids, NotificationStatus status)
        {
            return Context.Update(x => x.Id.Included(ids), new { status });
        }

        /// <summary>
        /// 设置状态。
        /// </summary>
        /// <param name="ids">通知id。</param>
        /// <param name="status">状态。</param>
        /// <returns>返回设置结果。</returns>
        public virtual Task<bool> SetStatusAsync(int[] ids, NotificationStatus status)
        {
            return Context.UpdateAsync(x => x.Id.Included(ids), new { status });
        }

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        public override void Clear()
        {
            Context.Delete(x => x.UserId == UserId);
        }

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        public override Task ClearAsync(CancellationToken cancellationToken = default)
        {
            return Context.DeleteAsync(x => x.UserId == UserId, cancellationToken);
        }
    }
}