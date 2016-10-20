using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Sitko.ModelSelector.Predicates
{
    public class ContainsPredicate<TModel, TProperty> : SinglePredicate<TModel, TProperty>
    {
        public ContainsPredicate(Expression<Func<TModel, TProperty>> property, TProperty value)
            : base(property, value)
        {
        }

        [UsedImplicitly]
        public ContainsPredicate(string propertyName, TProperty value) : base(propertyName, value)
        {
        }

        public override PredicateType Type { get; } = PredicateType.Contains;

        public override string ToDynamicLinqString(int index)
        {
            return $"{PropertyName}.Contains(@{index})";
        }
    }
}
