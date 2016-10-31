using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Sitko.ModelSelector.Base
{
    public abstract class Predicate
    {
        [UsedImplicitly]
        [JsonProperty("type")]
        public abstract PredicateType Type { get; }

        public abstract string ToDynamicLinqString(int index);

        public abstract object GetAttribute();
    }

    public abstract class Predicate<TModel, TProperty> : Predicate
    {
        protected Predicate(Expression<Func<TModel, TProperty>> property)
        {
            PropertyName = GetPropertyName(property);
        }

        protected Predicate(string propertyName)
        {
            PropertyName = propertyName;
        }

        [UsedImplicitly]
        [JsonProperty("isMultiple")]
        public abstract bool IsMultiple { get; }

        [JsonProperty("propertyName")]
        protected string PropertyName { get; private set; }

        private static string GetPropertyName(Expression<Func<TModel, TProperty>> property)
        {
            return ((MemberExpression) property.Body).Member.Name;
        }
    }
}