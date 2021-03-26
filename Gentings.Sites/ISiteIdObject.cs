using Gentings.Extensions;

namespace Gentings.Sites
{
    /// <summary>
    /// 支持SaaS的对象接口。
    /// </summary>
    /// <typeparam name="TKey">唯一键类型。</typeparam>
    public interface ISiteIdObject<TKey> : ISite, IIdObject<TKey>
    {

    }

    /// <summary>
    /// 支持SaaS的对象接口。
    /// </summary>
    public interface ISiteIdObject : ISiteIdObject<int>, IIdObject
    {

    }
}