using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Sitko.ModelSelector.Predicates
{
    public class NotIsNullPredicate<TModel, TProperty> : Predicate<TModel, TProperty>
    {
        public NotIsNullPredicate(Expression<Func<TModel, TProperty>> property) : base(property)
        {
        }

        [UsedImplicitly]
        public NotIsNullPredicate(string propertyName, TProperty value) : base(propertyName)
        {
        }

        public override PredicateType Type { get; } = PredicateType.NotNull;

        public override string ToDynamicLinqString(int index)
        {
            return $"{PropertyName} != null";
        }

        public override object GetAttribute()
        {
            return null;
        }
    }
}