using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using Gentings.Data;
using Gentings.Properties;

namespace Gentings.Extensions.AspNetCore
{
    /// <summary>
    /// 对象变更对比实现类。
    /// </summary>
    public class ObjectDiffer : IObjectDiffer, IEnumerable<ObjectDiffer.Differ>
    {
        private readonly ILocalizer _localizer;
        private bool _initialized;
        private bool _differed;
        private IEntityType _entityType;

        private readonly IDictionary<string, string> _stored =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 初始化类<see cref="ObjectDiffer"/>。
        /// </summary>
        /// <param name="localizer">本地化接口。</param>
        public ObjectDiffer(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// 存储对象的属性，一般为原有对象实例。
        /// </summary>
        /// <typeparam name="T">当前对象类型。</typeparam>
        /// <param name="oldInstance">原有对象实例。</param>
        /// <returns>返回当前实例。</returns>
        public virtual T Stored<T>(T oldInstance)
        {
            if (_initialized)
            {
                throw new Exception(Resources.Differ_Duplicated_Initialized);
            }

            _initialized = true;
            _entityType = oldInstance.GetType().GetEntityType();
            _typeName = _entityType.ClrType.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ??
                        _entityType.ClrType.GetCustomAttribute<DisplayAttribute>()?.Name ??
                        _localizer.GetString(_entityType.ClrType, $"_{_entityType.Name}");
            foreach (var property in _entityType.GetProperties())
            {
                _stored[property.Name] = GetValue(property, oldInstance);
            }

            return oldInstance;
        }

        private string GetValue(IProperty property, object instance)
        {
            var value = property.Get(instance);
            if (value == null)
            {
                return null;
            }

            return value.GetType().IsEnum ? _localizer?.GetString((Enum) value) : value.ToString();
        }

        private IList<Differ> _entities;
        private string _typeName;

        /// <summary>
        /// 对象新的对象，判断是否已经变更。
        /// </summary>
        /// <param name="newInstance">新对象实例。</param>
        /// <returns>返回对比结果。</returns>
        public virtual bool IsDifference(object newInstance)
        {
            if (!_initialized)
            {
                throw new Exception();
            }

            if (_differed)
            {
                throw new Exception(Resources.Differ_Duplicated_Differed);
            }

            _differed = true;
            _entities = new List<Differ>();
            foreach (var property in _entityType.GetProperties())
            {
                if (!property.IsUpdatable())
                {
                    continue;
                }

                var source = _stored[property.Name];
                var value = GetValue(property, newInstance);
                if (string.IsNullOrEmpty(source))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }

                    //新增
                    _entities.Add(new Differ
                    {
                        Action = DifferAction.Add, TypeName = _typeName, PropertyName = GetName(property), Value = value
                    });
                    continue;
                }

                if (string.IsNullOrEmpty(value))
                {
                    //删除
                    _entities.Add(new Differ
                    {
                        Action = DifferAction.Remove, TypeName = _typeName, PropertyName = GetName(property),
                        Source = source
                    });
                    continue;
                }

                if (source.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                //修改
                _entities.Add(new Differ
                {
                    Action = DifferAction.Modify, TypeName = _typeName, PropertyName = GetName(property),
                    Source = source, Value = value
                });
            }

            return _entities.Count > 0;
        }

        private string GetName(IProperty property)
        {
            if (property.DisplayName != null)
            {
                return property.DisplayName;
            }

            return _localizer == null
                ? property.Name
                : _localizer.GetString(property.DeclaringType.ClrType, property.Name);
        }

        /// <summary>
        /// 对象迭代器。
        /// </summary>
        /// <returns>返回集合迭代实例。</returns>
        public IEnumerator<Differ> GetEnumerator()
        {
            return (_entities ?? Enumerable.Empty<Differ>()).GetEnumerator();
        }

        /// <summary>
        /// 格式化出字符串。
        /// </summary>
        /// <returns>返回日志字符串。</returns>
        public override string ToString()
        {
            if (_entities == null || _entities.Count == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            foreach (var group in _entities.GroupBy(x => x.Action))
            {
                if (builder.Length > 0)
                {
                    builder.Append("; ");
                }

                var list = new List<string>();
                switch (group.Key)
                {
                    case DifferAction.Add:
                        builder.Append(Resources.DifferAction_Add);
                        foreach (var entity in group)
                        {
                            list.Add(string.Format(Resources.DifferAction_AddFormat, entity.PropertyName,
                                entity.Value));
                        }

                        break;
                    case DifferAction.Modify:
                        builder.Append(Resources.DifferAction_Modify);
                        foreach (var entity in group)
                        {
                            list.Add(string.Format(Resources.DifferAction_ModifyFormat, entity.PropertyName,
                                entity.Source,
                                entity.Value));
                        }

                        break;
                    case DifferAction.Remove:
                        builder.Append(Resources.DifferAction_Remove);
                        foreach (var entity in group)
                        {
                            list.Add(string.Format(Resources.DifferAction_RemoveFormat, entity.PropertyName,
                                entity.Source));
                        }

                        break;
                }

                builder.Append(string.Join(",", list));
            }

            return builder.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 变更实体。
        /// </summary>
        public class Differ
        {
            /// <summary>
            /// 日志操作实例。
            /// </summary>
            public DifferAction Action { get; internal set; }

            /// <summary>
            /// 类型名称。
            /// </summary>
            public string TypeName { get; internal set; }

            /// <summary>
            /// 属性名称。
            /// </summary>
            public string PropertyName { get; internal set; }

            /// <summary>
            /// 原始数据。
            /// </summary>
            public string Source { get; internal set; }

            /// <summary>
            /// 修改后得值。
            /// </summary>
            public string Value { get; internal set; }
        }
    }
}