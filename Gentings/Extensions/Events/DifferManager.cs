using Gentings.Data;

namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 对象更改管理类。
    /// </summary>
    public class DifferManager : IDifferManager
    {
        private readonly IDbContext<Differ> _db;
        /// <summary>
        /// 初始化类<see cref="DifferManager"/>。
        /// </summary>
        /// <param name="db">数据库操作接口实例。</param>
        public DifferManager(IDbContext<Differ> db)
        {
            _db = db;
        }

        /// <summary>
        /// 添加对象对比实例。
        /// </summary>
        /// <param name="differ">对象对比实例。</param>
        /// <returns>返回添加结果。</returns>
        public virtual bool Create(IObjectDiffer differ)
        {
            return _db.BeginTransaction(db =>
            {
                foreach (var item in differ)
                {
                    db.Create(item);
                }
                return true;
            }, 300);
        }

        /// <summary>
        /// 添加对象对比实例。
        /// </summary>
        /// <param name="differ">对象对比实例。</param>
        /// <returns>返回添加结果。</returns>
        public virtual Task<bool> CreateAsync(IObjectDiffer differ)
        {
            return _db.BeginTransactionAsync(async db =>
            {
                foreach (var item in differ)
                {
                    await db.CreateAsync(item);
                }
                return true;
            }, 300);
        }

        /// <summary>
        /// 分页加载对象对比实例列表。
        /// </summary>
        /// <param name="query">对象对比查询实例。</param>
        /// <returns>返回对象对比实例列表。</returns>
        public virtual IPageEnumerable<Differ> Load(DifferQuery query)
        {
            return _db.Load(query);
        }

        /// <summary>
        /// 分页加载对象对比实例列表。
        /// </summary>
        /// <param name="query">对象对比查询实例。</param>
        /// <returns>返回对象对比实例列表。</returns>
        public virtual Task<IPageEnumerable<Differ>> LoadAsync(DifferQuery query)
        {
            return _db.LoadAsync(query);
        }

        /// <summary>
        /// 删除对象对比实例。
        /// </summary>
        /// <param name="ids">对象对比实例Id集合。</param>
        /// <returns>返回删除结果。</returns>
        public virtual bool Delete(int[] ids)
        {
            return _db.Delete(x => x.Id.Included(ids));
        }

        /// <summary>
        /// 删除对象对比实例。
        /// </summary>
        /// <param name="ids">对象对比实例Id集合。</param>
        /// <returns>返回删除结果。</returns>
        public virtual Task<bool> DeleteAsync(int[] ids)
        {
            return _db.DeleteAsync(x => x.Id.Included(ids));
        }

        /// <summary>
        /// 删除对象对比实例。
        /// </summary>
        /// <param name="typeName">类型名称。</param>
        /// <returns>返回删除结果。</returns>
        public virtual bool Delete(string typeName)
        {
            return _db.Delete(x => x.TypeName == typeName);
        }

        /// <summary>
        /// 删除对象对比实例。
        /// </summary>
        /// <param name="typeName">类型名称。</param>
        /// <returns>返回删除结果。</returns>
        public virtual Task<bool> DeleteAsync(string typeName)
        {
            return _db.DeleteAsync(x => x.TypeName == typeName);
        }

        /// <summary>
        /// 删除对象对比实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回删除结果。</returns>
        public virtual bool Delete(int userId)
        {
            return _db.Delete(x => x.UserId == userId);
        }

        /// <summary>
        /// 删除对象对比实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回删除结果。</returns>
        public virtual Task<bool> DeleteAsync(int userId)
        {
            return _db.DeleteAsync(x => x.UserId == userId);
        }

        /// <summary>
        /// 查询对象对比实例。
        /// </summary>
        /// <param name="id">对象对比Id。</param>
        /// <returns>返回对象对比实例。</returns>
        public virtual Differ GetDiffer(int id)
        {
            return _db.Find(id);
        }

        /// <summary>
        /// 查询对象对比实例。
        /// </summary>
        /// <param name="id">对象对比Id。</param>
        /// <returns>返回对象对比实例。</returns>
        public virtual Task<Differ> GetDifferAsync(int id)
        {
            return _db.FindAsync(id);
        }
    }
}