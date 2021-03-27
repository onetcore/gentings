using Gentings.Data;

namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 对象修改查询实例。
    /// </summary>
    public class DifferQuery : QueryBase<Differ>
    {
        /// <summary>
        /// 类型。
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 用户Id。
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 属性名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 初始化查询上下文。
        /// </summary>
        /// <param name="context">查询上下文。</param>
        protected internal override void Init(IQueryContext<Differ> context)
        {
            context.WithNolock().OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Type))
                context.Where(x => x.TypeName == Type);
            if (!string.IsNullOrEmpty(Name))
                context.Where(x => x.PropertyName == Name);
            if (UserId > 0)
                context.Where(x => x.UserId == UserId);
        }
    }
}