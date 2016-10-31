using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Sitko.ModelSelector.Base;

namespace Sitko.ModelSelector.Predicates
{
    public class InPredicate<TModel, TProperty> : MultiplePredicate<TModel, TProperty>
    {
        public InPredicate(Expression<Func<TModel, TProperty>> property, IEnumerable<TProperty> values)
            : base(property, values)
        {
        }

        [UsedImplicitly]
        public InPredicate(string propertyName, IEnumerable<TProperty> values) : base(propertyName, values)
        {
        }

        public override PredicateType Type { get; } = PredicateType.In;

        public override string ToDynamicLinqString(int index)
        {
            return $"@{index}.Contains({PropertyName})";
        }
    }
}