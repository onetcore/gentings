using System.Threading.Tasks;
using Gentings.Data;

namespace Gentings.Extensions.Notifications
{
    /// <summary>
    /// 通知扩展类。
    /// </summary>
    public static class NotifierExtensions
    {
        /// <summary>
        /// 事务发送通知。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="db">当前事务接口实例。</param>
        /// <param name="userId">用户Id。</param>
        /// <param name="typeName">通知类型。</param>
        /// <param name="message">通知内容。</param>
        /// <param name="args">通知内容参数。</param>
        public static void SendNotifier<TModel>(this IDbContext<TModel> db, int userId, string typeName,
            string message, params object[] args)
        {
            var notification = Create(userId, db.GetTypeId(typeName), message, args);
            db.As<Notification>().Create(notification);
        }

        /// <summary>
        /// 事务发送通知。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="db">当前事务接口实例。</param>
        /// <param name="userId">用户Id。</param>
        /// <param name="typeName">通知类型。</param>
        /// <param name="message">通知内容。</param>
        /// <param name="args">通知内容参数。</param>
        public static async Task SendNotifierAsync<TModel>(this IDbContext<TModel> db, int userId, string typeName,
            string message, params object[] args)
        {
            var notification = Create(userId, await db.GetTypeIdAsync(typeName), message, args);
            await db.As<Notification>().CreateAsync(notification);
        }

        /// <summary>
        /// 事务发送通知。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="db">当前事务接口实例。</param>
        /// <param name="userId">用户Id。</param>
        /// <param name="typeName">通知类型。</param>
        /// <param name="message">通知内容。</param>
        /// <param name="args">通知内容参数。</param>
        public static void SendNotifier<TModel>(this IDbContext<TModel> db, int[] userId, string typeName,
            string message, params object[] args)
        {
            var typeId = db.GetTypeId(typeName);
            var ndb = db.As<Notification>();
            foreach (var id in userId)
            {
                var notification = Create(id, typeId, message, args);
                ndb.Create(notification);
            }
        }

        /// <summary>
        /// 事务发送通知。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="db">当前事务接口实例。</param>
        /// <param name="userId">用户Id。</param>
        /// <param name="typeName">通知类型。</param>
        /// <param name="message">通知内容。</param>
        /// <param name="args">通知内容参数。</param>
        public static async Task SendNotifierAsync<TModel>(this IDbContext<TModel> db, int[] userId,
            string typeName, string message, params object[] args)
        {
            var typeId = await db.GetTypeIdAsync(typeName);
            var ndb = db.As<Notification>();
            foreach (var id in userId)
            {
                var notification = Create(id, typeId, message, args);
                await ndb.CreateAsync(notification);
            }
        }


        /// <summary>
        /// 实例化一个通知。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="typeId">通知类型。</param>
        /// <param name="message">通知内容。</param>
        /// <param name="args">通知内容参数。</param>
        /// <returns>返回通知实例。</returns>
        private static Notification Create(int userId, int typeId, string message, params object[] args)
        {
            var notification = new Notification();
            notification.UserId = userId;
            notification.TypeId = typeId;
            notification.Message = string.Format(message, args);
            return notification;
        }

        /// <summary>
        /// 通过类型名称获取类型Id。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="db">当前事务接口实例。</param>
        /// <param name="typeName">类型名称。</param>
        /// <returns>返回类型Id。</returns>
        private static int GetTypeId<TModel>(this IDbContext<TModel> db, string typeName)
        {
            var tdb = db.As<NotificationType>();
            var type = tdb.Find(x => x.Name == typeName);
            if (type == null)
            {
                type = new NotificationType { Name = typeName };
                tdb.Create(type);
            }

            return type.Id;
        }

        /// <summary>
        /// 通过类型名称获取类型Id。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="db">当前事务接口实例。</param>
        /// <param name="typeName">类型名称。</param>
        /// <returns>返回类型Id。</returns>
        private static async Task<int> GetTypeIdAsync<TModel>(this IDbContext<TModel> db, string typeName)
        {
            var tdb = db.As<NotificationType>();
            var type = await tdb.FindAsync(x => x.Name == typeName);
            if (type == null)
            {
                type = new NotificationType { Name = typeName };
                await tdb.CreateAsync(type);
            }

            return type.Id;
        }
    }
}