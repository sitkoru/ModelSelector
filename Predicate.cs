using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Sitko.ModelSelector
{
    public abstract class Predicate
    {
        public abstract PredicateType Type { get; }
        public abstract string ToDynamicLinqString(int index);

        public abstract object GetAttribute();
    }

    public abstract class Predicate<TModel, TProperty> : Predicate
    {
        [JsonProperty]
        protected string PropertyName { get; private set; }

        [JsonProperty]
        [UsedImplicitly]
        protected Type PropertyType { get; set; }

        [JsonProperty]
        [UsedImplicitly]
        protected Type ValueType { get; set; }

        [JsonProperty]
        [UsedImplicitly]
        private Type ModelType { get; }

        protected Predicate(Expression<Func<TModel, TProperty>> property)
        {
            PropertyName = GetPropertyName(property);
            PropertyType = typeof(TProperty);
            ModelType = typeof(TModel);
            ValueType = typeof(TProperty);
        }

        protected Predicate(string propertyName)
        {
            PropertyName = propertyName;
            PropertyType = typeof(TProperty);
            ModelType = typeof(TModel);
            ValueType = typeof(TProperty);
        }

        private static string GetPropertyName(Expression<Func<TModel, TProperty>> property)
        {
            return ((MemberExpression)property.Body).Member.Name;
        }
    }
}
