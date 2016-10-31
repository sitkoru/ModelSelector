using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Sitko.ModelSelector.Base;

namespace Sitko.ModelSelector.Predicates
{
    public class GreaterThanEqualsPredicate<TModel, TProperty> : SinglePredicate<TModel, TProperty>
    {
        public GreaterThanEqualsPredicate(Expression<Func<TModel, TProperty>> property, TProperty value)
            : base(property, value)
        {
        }

        [UsedImplicitly]
        public GreaterThanEqualsPredicate(string propertyName, TProperty value) : base(propertyName, value)
        {
        }

        public override PredicateType Type { get; } = PredicateType.GreaterThanEquals;

        public override string ToDynamicLinqString(int index)
        {
            return $"{PropertyName}>=@{index}";
        }
    }
}