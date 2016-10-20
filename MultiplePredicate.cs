using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sitko.ModelSelector
{
    public abstract class MultiplePredicate<TModel, TProperty> : Predicate<TModel, TProperty>
    {
        [JsonProperty("value")] private readonly IEnumerable<TProperty> _values;

        protected MultiplePredicate(Expression<Func<TModel, TProperty>> property, IEnumerable<TProperty> values)
            : base(property)
        {
            _values = values;
            PropertyType = typeof(TProperty);
            ValueType = typeof(IEnumerable<TProperty>);

        }

        protected MultiplePredicate(string propertyName, IEnumerable<TProperty> values) : base(propertyName)
        {
            _values = values;
            PropertyType = typeof(TProperty);
            ValueType = typeof(IEnumerable<TProperty>);
        }

        public override object GetAttribute()
        {
            return _values;
        }
    }
}
