namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 事件管理接口。
    /// </summary>
    public interface IEventManager 
    {
        #region eventTypes
        /// <summary>
        /// 添加事件类型实例。
        /// </summary>
        /// <param name="eventType">事件类型实例。</param>
        /// <returns>返回添加结果。</returns>
        bool Create(EventType eventType);

        /// <summary>
        /// 添加事件类型实例。
        /// </summary>
        /// <param name="eventType">事件类型实例。</param>
        /// <returns>返回添加结果。</returns>
        Task<bool> CreateAsync(EventType eventType);

        /// <summary>
        /// 更新事件类型实例。
        /// </summary>
        /// <param name="eventType">事件类型实例。</param>
        /// <returns>返回更新结果。</returns>
        bool Update(EventType eventType);

        /// <summary>
        /// 更新事件类型实例。
        /// </summary>
        /// <param name="eventType">事件类型实例。</param>
        /// <returns>返回更新结果。</returns>
        Task<bool> UpdateAsync(EventType eventType);

        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <param name="id">事件Id。</param>
        /// <returns>返回事件类型。</returns>
        EventType? GetEventType(int id);

        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <param name="id">事件Id。</param>
        /// <returns>返回事件类型。</returns>
        Task<EventType?> GetEventTypeAsync(int id);

        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <param name="eventType">事件类型名称。</param>
        /// <returns>返回事件类型。</returns>
        EventType? GetEventType(string eventType);

        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <param name="eventType">事件类型名称。</param>
        /// <returns>返回事件类型。</returns>
        Task<EventType?> GetEventTypeAsync(string eventType);

        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <returns>返回事件类型。</returns>
        IEnumerable<EventType> GetEventTypes();

        /// <summary>
        /// 获取事件类型。
        /// </summary>
        /// <returns>返回事件类型。</returns>
        Task<IEnumerable<EventType>> GetEventTypesAsync();

        /// <summary>
        /// 删除事件类型实例。
        /// </summary>
        /// <param name="ids">事件类型Id列表。</param>
        /// <returns>返回删除结果。</returns>
        bool DeleteEventTypes(int[] ids);

        /// <summary>
        /// 删除事件类型实例。
        /// </summary>
        /// <param name="ids">事件类型Id列表。</param>
        /// <returns>返回删除结果。</returns>
        Task<bool> DeleteEventTypesAsync(int[] ids);
        #endregion

        #region events
        /// <summary>
        /// 添加事件实例。
        /// </summary>
        /// <param name="@event">事件实例。</param>
        /// <returns>返回添加结果。</returns>
        bool Create(Event @event);

        /// <summary>
        /// 添加事件实例。
        /// </summary>
        /// <param name="@event">事件实例。</param>
        /// <returns>返回添加结果。</returns>
        Task<bool> CreateAsync(Event @event);

        /// <summary>
        /// 分页查询事件列表。
        /// </summary>
        /// <param name="query">事件查询实例。</param>
        /// <returns>返回事件查询列表。</returns>
        IPageEnumerable<Event> Load(EventQuery query);

        /// <summary>
        /// 分页查询事件列表。
        /// </summary>
        /// <param name="query">事件查询实例。</param>
        /// <returns>返回事件查询列表。</returns>
        Task<IPageEnumerable<Event>> LoadAsync(EventQuery query);

        /// <summary>
        /// 查询用户最新事件列表。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="size">最新数量。</param>
        /// <returns>返回事件查询列表。</returns>
        IEnumerable<Event> LoadUserEvents(int userId, int size);

        /// <summary>
        /// 查询用户最新事件列表。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="size">最新数量。</param>
        /// <returns>返回事件查询列表。</returns>
        Task<IEnumerable<Event>> LoadUserEventsAsync(int userId, int size);

        /// <summary>
        /// 获取事件消息实例。
        /// </summary>
        /// <param name="id">事件消息列表。</param>
        /// <returns>返回事件消息实例。</returns>
        Event? GetEvent(int id);

        /// <summary>
        /// 获取事件消息实例。
        /// </summary>
        /// <param name="id">事件消息列表。</param>
        /// <returns>返回事件消息实例。</returns>
        Task<Event?> GetEventAsync(int id);

        /// <summary>
        /// 删除事件实例。
        /// </summary>
        /// <param name="ids">事件Id列表。</param>
        /// <returns>返回删除结果。</returns>
        bool DeleteEvents(int[] ids);

        /// <summary>
        /// 删除事件实例。
        /// </summary>
        /// <param name="ids">事件Id列表。</param>
        /// <returns>返回删除结果。</returns>
        Task<bool> DeleteEventsAsync(int[] ids);
        #endregion
    }
}