using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Sitko.ModelSelector.Predicates
{
    public class NotInPredicate<TModel, TProperty> : MultiplePredicate<TModel, TProperty>
    {
        public NotInPredicate(Expression<Func<TModel, TProperty>> property, IEnumerable<TProperty> values)
            : base(property, values)
        {
        }

        [UsedImplicitly]
        public NotInPredicate(string propertyName, IEnumerable<TProperty> values) : base(propertyName, values)
        {
        }

        public override PredicateType Type { get; } = PredicateType.NotIn;

        public override string ToDynamicLinqString(int index)
        {
            return $"!@{index}.Contains({PropertyName})";
        }
    }
}