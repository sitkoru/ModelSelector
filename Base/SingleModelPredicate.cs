using System;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Sitko.ModelSelector.Base
{
    public abstract class SinglePredicate<TModel, TProperty> : Predicate<TModel, TProperty>
    {
        [JsonProperty("value")] private readonly TProperty _value;

        protected SinglePredicate(Expression<Func<TModel, TProperty>> property, TProperty value) : base(property)
        {
            _value = value;
        }

        protected SinglePredicate(string propertyName, TProperty value) : base(propertyName)
        {
            _value = value;
        }

        public override bool IsMultiple { get; } = false;

        public override object GetAttribute()
        {
            return _value;
        }
    }
}