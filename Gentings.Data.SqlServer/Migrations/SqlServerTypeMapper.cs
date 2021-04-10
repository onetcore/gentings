﻿using System;
using System.Collections.Generic;
using Gentings.Data.Migrations;
using Gentings.Data.SqlServer.Properties;

namespace Gentings.Data.SqlServer.Migrations
{
    /// <summary>
    /// SQLServer类型匹配实现类。
    /// </summary>
    public class SqlServerTypeMapper : TypeMapper
    {
        private readonly IDictionary<Type, string> _simpleMappings
            = new Dictionary<Type, string>
            {
                {typeof(int), "int"},
                {typeof(long), "bigint"},
                {typeof(DateTime), "datetime2"},
                {typeof(Guid), "uniqueidentifier"},
                {typeof(bool), "bit"},
                {typeof(byte), "tinyint"},
                {typeof(double), "float"},
                {typeof(DateTimeOffset), "datetimeoffset"},
                {typeof(char), "int"},
                {typeof(sbyte), "smallint"},
                {typeof(ushort), "int"},
                {typeof(uint), "bigint"},
                {typeof(ulong), "numeric(20, 0)"},
                {typeof(short), "smallint"},
                {typeof(float), "real"},
                //{typeof(decimal), "decimal"},
                {typeof(TimeSpan), "time"},
                {typeof(byte[]), "varbinary"}
            };

        /// <summary>
        /// 获取数据类型。
        /// </summary>
        /// <param name="type">当前类型实例。</param>
        /// <param name="size">大小。</param>
        /// <param name="rowVersion">是否为RowVersion。</param>
        /// <param name="unicode">是否为Unicode字符集。</param>
        /// <param name="precision">数据长度。</param>
        /// <param name="scale">小数长度。</param>
        /// <returns>返回匹配的数据类型。</returns>
        public override string GetMapping(Type type, int? size = null, bool rowVersion = false, bool? unicode = null,
            int? precision = null,
            int? scale = null)
        {
            Check.NotNull(type, nameof(type));
            if (rowVersion)
            {
                if (type == typeof(byte[]))
                {
                    return "timestamp";
                }

                throw new Exception(Resources.TypeMustBeBytes);
            }

            type = type.UnwrapNullableType().UnwrapEnumType();
            if (type == typeof(string))
            {
                return size > 0 ? $"nvarchar({size})" : "nvarchar(max)";
            }

            if (type == typeof(byte[]))
            {
                return size > 0 ? $"varbinary({size})" : "varbinary(max)";
            }

            if (type == typeof(decimal))
            {
                precision ??= 18;
                scale ??= 2;
                return $"decimal({precision}, {scale})";
            }

            if (_simpleMappings.TryGetValue(type, out var retType))
            {
                return retType;
            }

            throw new NotSupportedException(string.Format(Resources.UnsupportedType, type.DisplayName()));
        }
    }
}