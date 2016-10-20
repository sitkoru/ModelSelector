using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Sitko.ModelSelector.Predicates
{
    public class GreaterThanPredicate<TModel, TProperty> : SinglePredicate<TModel, TProperty>
    {
        public GreaterThanPredicate(Expression<Func<TModel, TProperty>> property, TProperty value)
            : base(property, value)
        {
        }

        [UsedImplicitly]
        public GreaterThanPredicate(string propertyName, TProperty value) : base(propertyName, value)
        {
        }

        public override PredicateType Type { get; } = PredicateType.GreaterThan;

        public override string ToDynamicLinqString(int index)
        {
            return $"{PropertyName}>@{index}";
        }
    }
}