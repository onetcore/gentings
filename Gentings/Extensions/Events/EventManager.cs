using System.Collections.Concurrent;
using Gentings.Data;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 事件管理实现类。
    /// </summary>
    public class EventManager : IEventManager
    {
        private readonly IDbContext<Event> _edb;
        private readonly IDbContext<EventType> _etdb;
        private readonly IMemoryCache _cache;
        private readonly Type _cacheKey = typeof(EventType);

        /// <summary>
        /// 初始化类<see cref="EventManager"/>。
        /// </summary>
        /// <param name="edb">事件数据库操作接口实例。</param>
        /// <param name="etdb">事件类型数据库操作接口。</param>
        /// <param name="cache">缓存实例对象。</param>
        public EventManager(IDbContext<Event> edb, IDbContext<EventType> etdb, IMemoryCache cache)
        {
            _edb = edb;
            _etdb = etdb;
            _cache = cache;
        }

        /// <summary>
        /// 判断是否刷新缓存。
        /// </summary>
        protected virtual bool Refresh(bool result)
        {
            if (result)
            {
                _cache.Remove(_cacheKey);
            }
            return result;
        }

        /// <summary>
        /// 获取事件类型缓存。
        /// </summary>
        /// <returns>事件类型缓存。</returns>
        protected virtual ConcurrentDictionary<int, EventType> GetCached()
        {
            return _cache.GetOrCreate(_cacheKey, ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var types = _etdb.Fetch();
                return new ConcurrentDictionary<int, EventType>(types.ToDictionary(x => x.Id));
            });
        }

        /// <summary>
        /// 获取事件类型缓存。
        /// </summary>
        /// <returns>事件类型缓存。</returns>
        protected virtual Task<ConcurrentDictionary<int, EventType>> GetCachedAsync()
        {
            return _cache.GetOrCreateAsync(_cacheKey, async ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var types = await _etdb.FetchAsync();
                return new ConcurrentDictionary<int, EventType>(types.ToDictionary(x => x.Id));
            });
        }

        /// <summary>
        /// 添加事件类型实例。
        /// </summary>
        /// <param name="eventType">事件类型实例。</param>
        /// <returns>返回添加结果。</returns>
        public virtual bool Create(EventType eventType)
        {
            return Refresh(_etdb.Create(eventType));
        }

        /// <summary>
        /// 添加事件类型实例。
        /// </summary>
        /// <param name="eventType">事件类型实例。</param>
        /// <returns>返回添加结果。</returns>
        public virtual async Task<bool> CreateAsync(EventType eventType)
        {
            return Refresh(await _etdb.CreateAsync(eventType));
        }

        /// <summary>
        /// 更新事件类型实例。
        /// </summary>
        /// <param name="eventType">事件类型实例。</param>
        /// <returns>返回更新结果。</returns>
        public virtual bool Update(EventType eventType)
        {
            return Refresh(_etdb.Update(eventType));
        }

        /// <summary>
        /// 更新事件类型实例。
        /// </summary>
        /// <param name="eventType">事件类型实例。</param>
        /// <returns>返回更新结果。</returns>
        public virtual async Task<bool> UpdateAsync(EventType eventType)
        {
            return Refresh(await _etdb.UpdateAsync(eventType));
        }

        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <param name="id">事件Id。</param>
        /// <returns>返回事件类型。</returns>
        public virtual EventType? GetEventType(int id)
        {
            var eventTypes = GetCached();
            eventTypes.TryGetValue(id, out var eventType);
            return eventType;
        }

        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <param name="id">事件Id。</param>
        /// <returns>返回事件类型。</returns>
        public virtual async Task<EventType?> GetEventTypeAsync(int id)
        {
            var eventTypes = await GetCachedAsync();
            eventTypes.TryGetValue(id, out var eventType);
            return eventType;
        }

        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <param name="eventType">事件类型名称。</param>
        /// <returns>返回事件类型。</returns>
        public virtual EventType? GetEventType(string eventType)
        {
            var eventTypes = GetCached();
            return eventTypes.Values.SingleOrDefault(x => x.Name!.Equals(eventType, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <param name="eventType">事件类型名称。</param>
        /// <returns>返回事件类型。</returns>
        public virtual async Task<EventType?> GetEventTypeAsync(string eventType)
        {
            var eventTypes = await GetCachedAsync();
            return eventTypes.Values.SingleOrDefault(x => x.Name!.Equals(eventType, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <returns>返回事件类型。</returns>
        public virtual IEnumerable<EventType> GetEventTypes()
        {
            var eventTypes = GetCached();
            return eventTypes.Values;
        }

        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <returns>返回事件类型。</returns>
        public virtual async Task<IEnumerable<EventType>> GetEventTypesAsync()
        {
            var eventTypes = await GetCachedAsync();
            return eventTypes.Values;
        }

        /// <summary>
        /// 删除事件类型实例。
        /// </summary>
        /// <param name="ids">事件类型Id列表。</param>
        /// <returns>返回删除结果。</returns>
        public virtual bool DeleteEventTypes(int[] ids)
        {
            return Refresh(_etdb.Delete(x => x.Id.Included(ids)));
        }

        /// <summary>
        /// 删除事件类型实例。
        /// </summary>
        /// <param name="ids">事件类型Id列表。</param>
        /// <returns>返回删除结果。</returns>
        public virtual async Task<bool> DeleteEventTypesAsync(int[] ids)
        {
            return Refresh(await _etdb.DeleteAsync(x => x.Id.Included(ids)));
        }

        /// <summary>
        /// 添加事件实例。
        /// </summary>
        /// <param name="event">事件实例。</param>
        /// <returns>返回添加结果。</returns>
        public virtual bool Create(Event @event)
        {
            return _edb.Create(@event);
        }

        /// <summary>
        /// 添加事件实例。
        /// </summary>
        /// <param name="event">事件实例。</param>
        /// <returns>返回添加结果。</returns>
        public virtual Task<bool> CreateAsync(Event @event)
        {
            return _edb.CreateAsync(@event);
        }

        /// <summary>
        /// 分页查询事件列表。
        /// </summary>
        /// <param name="query">事件查询实例。</param>
        /// <returns>返回事件查询列表。</returns>
        public virtual IPageEnumerable<Event> Load(EventQuery query)
        {
            return _edb.Load(query);
        }

        /// <summary>
        /// 分页查询事件列表。
        /// </summary>
        /// <param name="query">事件查询实例。</param>
        /// <returns>返回事件查询列表。</returns>
        public virtual Task<IPageEnumerable<Event>> LoadAsync(EventQuery query)
        {
            return _edb.LoadAsync(query);
        }

        /// <summary>
        /// 查询用户最新事件列表。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="size">最新数量。</param>
        /// <returns>返回事件查询列表。</returns>
        public virtual IEnumerable<Event> LoadUserEvents(int userId, int size)
        {
            return _edb.AsQueryable()
                .WithNolock()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .AsEnumerable(size);
        }

        /// <summary>
        /// 查询用户最新事件列表。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="size">最新数量。</param>
        /// <returns>返回事件查询列表。</returns>
        public virtual Task<IEnumerable<Event>> LoadUserEventsAsync(int userId, int size)
        {
            return _edb.AsQueryable()
                .WithNolock()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .AsEnumerableAsync(size);
        }

        /// <summary>
        /// 获取事件消息实例。
        /// </summary>
        /// <param name="id">事件消息列表。</param>
        /// <returns>返回事件消息实例。</returns>
        public virtual Event? GetEvent(int id)
        {
            return _edb.Find(id);
        }

        /// <summary>
        /// 获取事件消息实例。
        /// </summary>
        /// <param name="id">事件消息列表。</param>
        /// <returns>返回事件消息实例。</returns>
        public virtual Task<Event?> GetEventAsync(int id)
        {
            return _edb.FindAsync(id);
        }

        /// <summary>
        /// 删除事件实例。
        /// </summary>
        /// <param name="ids">事件Id列表。</param>
        /// <returns>返回删除结果。</returns>
        public virtual bool DeleteEvents(int[] ids)
        {
            return _edb.Delete(x => x.Id.Included(ids));
        }

        /// <summary>
        /// 删除事件实例。
        /// </summary>
        /// <param name="ids">事件Id列表。</param>
        /// <returns>返回删除结果。</returns>
        public virtual Task<bool> DeleteEventsAsync(int[] ids)
        {
            return _edb.DeleteAsync(x => x.Id.Included(ids));
        }
    }
}