using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;

namespace Gentings.AspNetCore.Options
{
    /// <summary>
    /// 页面应用模型转换类型。
    /// </summary>
    /// <typeparam name="TModelUIAttribute">模型特性类型实例。</typeparam>
    /// <typeparam name="TModel">模型类型实例。</typeparam>
    internal class ModelUIConvention<TModelUIAttribute, TModel> : IPageApplicationModelConvention
        where TModel : class
        where TModelUIAttribute : ModelUIAttribute
    {
        /// <summary>
        /// 获取真实的模型基类类型。
        /// </summary>
        /// <param name="model">页面应用模型实例。</param>
        public void Apply(PageApplicationModel model)
        {
            var attribute = model.ModelType.GetCustomAttribute<TModelUIAttribute>();
            if (attribute == null)
                return;

            ValidateTemplate(attribute.Template);
            var template = attribute.Template.MakeGenericType(typeof(TModel));
            model.ModelType = template.GetTypeInfo();
        }

        private void ValidateTemplate(Type template)
        {
            if (template.IsAbstract || !template.IsGenericTypeDefinition)
            {
                throw new InvalidOperationException("Implementation type can't be abstract or non generic.");
            }
            var genericArguments = template.GetGenericArguments();
            if (genericArguments.Length != 1)
            {
                throw new InvalidOperationException("Implementation type contains wrong generic arity.");
            }
        }
    }
}
