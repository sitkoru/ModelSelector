using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Sitko.ModelSelector.Predicates
{
    public class NotContainsPredicate<TModel, TProperty> : SinglePredicate<TModel, TProperty>
    {
        public NotContainsPredicate(Expression<Func<TModel, TProperty>> property, TProperty value)
            : base(property, value)
        {
        }

        [UsedImplicitly]
        public NotContainsPredicate(string propertyName, TProperty value) : base(propertyName, value)
        {
        }

        public override PredicateType Type { get; } = PredicateType.NotContains;

        public override string ToDynamicLinqString(int index)
        {
            return $"!{PropertyName}.Contains(@{index})";
        }
    }
}
