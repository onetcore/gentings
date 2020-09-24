namespace Gentings.Extensions
{
    /// <summary>
    /// 分组接口。
    /// </summary>
    /// <typeparam name="TGroup">分组类型。</typeparam>
    public interface IGroupable<TGroup> : IParentable<TGroup>
        where TGroup : IGroupable<TGroup>
    {
        /// <summary>
        /// 分组名称。
        /// </summary>
        string Name { get; }
    }
}