using Gentings.Data;
using Gentings.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace Gentings.ChatServers
{
    /// <summary>
    /// 消息管理接口。
    /// </summary>
    public interface IMessageManager : IObjectManager<Message, long>, ISingletonService
    {

    }

    /// <summary>
    /// 消息管理接口。
    /// </summary>
    public class MessageManager : ObjectManager<Message, long>, IMessageManager
    {
        /// <summary>
        /// 初始化类<see cref="UserManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        public MessageManager(IDbContext<Message> context) : base(context)
        {
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <returns>返回添加结果。</returns>
        public override bool Create(Message model)
        {
            return Context.BeginTransaction(db =>
            {
                db.Create(model);
                var fdb = db.As<Friend>();
                return fdb.Update(x => x.UserId == model.Receiver && x.FriendId == model.Sender, x => new { Unreads = x.Unreads + 1 });
            });
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回添加结果。</returns>
        public override Task<bool> CreateAsync(Message model, CancellationToken cancellationToken = default)
        {
            return Context.BeginTransactionAsync(async db =>
            {
                await db.CreateAsync(model);
                var fdb = db.As<Friend>();
                return await fdb.UpdateAsync(x => x.UserId == model.Receiver && x.FriendId == model.Sender, x => new { Unreads = x.Unreads + 1 });
            }, cancellationToken: cancellationToken);
        }
    }
}
