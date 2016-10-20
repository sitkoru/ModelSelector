using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Sitko.ModelSelector.Predicates
{
    public class IsNullPredicate<TModel, TProperty> : Predicate<TModel, TProperty>
    {
        public IsNullPredicate(Expression<Func<TModel, TProperty>> property) : base(property)
        {
        }

        [UsedImplicitly]
        public IsNullPredicate(string propertyName, TProperty value) : base(propertyName)
        {
        }

        public override PredicateType Type { get; } = PredicateType.IsNull;

        public override string ToDynamicLinqString(int index)
        {
            return $"{PropertyName} == null";
        }

        public override object GetAttribute()
        {
            return null;
        }
    }
}