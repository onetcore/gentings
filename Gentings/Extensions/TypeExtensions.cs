using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Gentings.Extensions.Internal;

namespace Gentings.Extensions
{
    /// <summary>
    /// 匹配实现类。
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly ConcurrentDictionary<Type, string> _tables = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<Type, IEntityType> _types = new ConcurrentDictionary<Type, IEntityType>();

        /// <summary>
        /// 获取数据库表格。
        /// </summary>
        /// <param name="type">当前类型。</param>
        /// <returns>返回表格名称。</returns>
        public static string GetTableName(this Type type)
        {
            return _tables.GetOrAdd(type, key =>
            {
                TypeInfo info = type.GetTypeInfo();
                TableAttribute defined = info.GetCustomAttribute<TableAttribute>();
                if (defined != null)
                {
                    if (defined.Schema != null)
                        defined.Schema += ".";
                    return $"{defined.Schema}$pre:{defined.Name}";
                }
                TargetAttribute model = info.GetCustomAttribute<TargetAttribute>();
                if (model != null)
                    return GetTableName(model.Target);
                string name = info.Assembly.GetName().Name;
                int index = name.LastIndexOf('.');
                if (index != -1)
                    name = name.Substring(index);
                name += '_' + info.Name;
                return $"$pre:{name}";
            });
        }

        /// <summary>
        /// 获取实体类型。
        /// </summary>
        /// <returns>返回实体类型。</returns>
        /// <param name="type">当前类型。</param>
        public static IEntityType GetEntityType(this Type type)
        {
            return _types.GetOrAdd(type, key =>
            {
                EntityType entity = new EntityType(type);
                entity.Table = GetTableName(type);
                return entity;
            });
        }
    }
}