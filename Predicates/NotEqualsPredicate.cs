using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Sitko.ModelSelector.Base;

namespace Sitko.ModelSelector.Predicates
{
    public class NotEqualsPredicate<TModel, TProperty> : SinglePredicate<TModel, TProperty>
    {
        public NotEqualsPredicate(Expression<Func<TModel, TProperty>> property, TProperty value) : base(property, value)
        {
        }

        [UsedImplicitly]
        public NotEqualsPredicate(string propertyName, TProperty value) : base(propertyName, value)
        {
        }

        public override PredicateType Type { get; } = PredicateType.NotEquals;

        public override string ToDynamicLinqString(int index)
        {
            return $"{PropertyName}!=@{index}";
        }
    }
}