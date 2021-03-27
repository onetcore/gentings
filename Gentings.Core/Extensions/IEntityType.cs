﻿using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Gentings.Extensions
{
    /// <summary>
    /// 实体类型接口。
    /// </summary>
    public interface IEntityType
    {
        /// <summary>
        /// 表格名称。
        /// </summary>
        string Table { get; }

        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 类型。
        /// </summary>
        Type ClrType { get; }

        /// <summary>
        /// 获取主键。
        /// </summary>
        IKey PrimaryKey { get; }

        /// <summary>
        /// 行版本属性。
        /// </summary>
        IProperty RowVersion { get; }

        /// <summary>
        /// 并发验证属性。
        /// </summary>
        IKey ConcurrencyKey { get; }

        /// <summary>
        /// 自增长列。
        /// </summary>
        IProperty Identity { get; }

        /// <summary>
        /// 通过名称查找属性实例。
        /// </summary>
        /// <param name="name">属性名称。</param>
        /// <returns>返回属性实例对象。</returns>
        IProperty FindProperty(string name);

        /// <summary>
        /// 获取当前类型的所有属性列表。
        /// </summary>
        /// <returns>所有属性列表。</returns>
        IEnumerable<IProperty> GetProperties();

        /// <summary>
        /// 从数据库读取器中读取当前实例对象。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="reader">数据库读取器。</param>
        /// <returns>返回当前模型实例对象。</returns>
        TModel Read<TModel>(DbDataReader reader);

        /// <summary>
        /// 克隆相同属性名称的实例。
        /// </summary>
        /// <typeparam name="TModel">目标对象类型。</typeparam>
        /// <param name="instance">当前实例对象。</param>
        /// <returns>返回目标对象实例。</returns>
        TModel Cast<TModel>(object instance);
    }
}