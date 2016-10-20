using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Sitko.ModelSelector.Predicates
{
    public class EqualsPredicate<TModel, TProperty> : SinglePredicate<TModel, TProperty>
    {
        public EqualsPredicate(Expression<Func<TModel, TProperty>> property, TProperty value) : base(property, value)
        {
        }

        [UsedImplicitly]
        public EqualsPredicate(string propertyName, TProperty value) : base(propertyName, value)
        {
        }

        public override PredicateType Type { get; } = PredicateType.Equals;

        public override string ToDynamicLinqString(int index)
        {
            return $"{PropertyName}==@{index}";
        }
    }
}
