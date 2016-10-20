using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Sitko.ModelSelector.Predicates
{
    public class LessThanEqualsPredicate<TModel, TProperty> : SinglePredicate<TModel, TProperty>
    {
        public LessThanEqualsPredicate(Expression<Func<TModel, TProperty>> property, TProperty value)
            : base(property, value)
        {
        }

        [UsedImplicitly]
        public LessThanEqualsPredicate(string propertyName, TProperty value) : base(propertyName, value)
        {
        }

        public override PredicateType Type { get; } = PredicateType.LessThanEquals;

        public override string ToDynamicLinqString(int index)
        {
            return $"{PropertyName}<=@{index}";
        }
    }
}
