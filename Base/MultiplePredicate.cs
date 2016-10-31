using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Sitko.ModelSelector.Base
{
    public abstract class MultiplePredicate<TModel, TProperty> : Predicate<TModel, TProperty>
    {
        [JsonProperty("value")] private readonly IEnumerable<TProperty> _values;

        protected MultiplePredicate(Expression<Func<TModel, TProperty>> property, IEnumerable<TProperty> values)
            : base(property)
        {
            _values = values;
        }

        protected MultiplePredicate(string propertyName, IEnumerable<TProperty> values) : base(propertyName)
        {
            _values = values;
        }

        public override bool IsMultiple { get; } = true;

        public override object GetAttribute()
        {
            return _values;
        }
    }
}