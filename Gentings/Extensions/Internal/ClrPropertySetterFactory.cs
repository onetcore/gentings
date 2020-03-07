using System;
using System.Linq.Expressions;
using System.Reflection;
using Gentings.Properties;

namespace Gentings.Extensions.Internal
{
    /// <summary>
    /// 属性设置访问器工厂类。
    /// </summary>
    public class ClrPropertySetterFactory : ClrAccessorFactory<IClrPropertySetter>
    {
        /// <summary>
        /// 创建当前访问器实例。
        /// </summary>
        /// <typeparam name="TEntity">当前实体类型。</typeparam>
        /// <typeparam name="TValue">属性的CLR类型。</typeparam>
        /// <typeparam name="TNonNullableEnumValue">此属性的基础类型。</typeparam>
        /// <param name="propertyInfo">属性信息实例。</param>
        /// <returns>返回当前属性的访问器实例。</returns>
        protected override IClrPropertySetter CreateGeneric<TEntity, TValue, TNonNullableEnumValue>(
            PropertyInfo propertyInfo)
        {
            PropertyInfo memberInfo = propertyInfo.FindSetterProperty();

            if (memberInfo == null)
            {
                throw new InvalidOperationException(string.Format(
                    Resources.NoSetter, propertyInfo.Name, propertyInfo.DeclaringType.DisplayName(false)));
            }

            ParameterExpression entityParameter = Expression.Parameter(typeof(TEntity), "entity");
            ParameterExpression valueParameter = Expression.Parameter(typeof(TValue), "value");

            Action<TEntity, TValue> setter = Expression.Lambda<Action<TEntity, TValue>>(
                Expression.Assign(
                    Expression.MakeMemberAccess(entityParameter, memberInfo),
                    valueParameter),
                entityParameter,
                valueParameter).Compile();

            Type propertyType = propertyInfo.PropertyType;
            return propertyType.IsNullableType()
                   && propertyType.UnwrapNullableType().GetTypeInfo().IsEnum
                ? new NullableEnumClrPropertySetter<TEntity, TValue, TNonNullableEnumValue>(setter)
                : (IClrPropertySetter)new ClrPropertySetter<TEntity, TValue>(setter);
        }
    }
}