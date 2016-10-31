using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Sitko.ModelSelector.Base;

namespace Sitko.ModelSelector.Predicates
{
    public class LessThanPredicate<TModel, TProperty> : SinglePredicate<TModel, TProperty>
    {
        public LessThanPredicate(Expression<Func<TModel, TProperty>> property, TProperty value)
            : base(property, value)
        {
        }

        [UsedImplicitly]
        public LessThanPredicate(string propertyName, TProperty value) : base(propertyName, value)
        {
        }

        public override PredicateType Type { get; } = PredicateType.LessThan;

        public override string ToDynamicLinqString(int index)
        {
            return $"{PropertyName}<@{index}";
        }
    }
}