using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Sitko.ModelSelector.Json;
using Sitko.ModelSelector.Predicates;

namespace Sitko.ModelSelector.Base
{
    public class ModelSelector
    {
        [JsonProperty("predicates")] protected readonly List<Predicate> Predicates = new List<Predicate>();

        protected virtual void AddPredicate(Predicate predicate)
        {
            Predicates.Add(predicate);
        }

        public void AddPredicates(IEnumerable<Predicate> predicates)
        {
            Predicates.AddRange(predicates);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, GetJsonSettings());
        }

        public static ModelSelector<T> FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<ModelSelector<T>>(json, GetJsonSettings());
        }


        private static JsonSerializerSettings GetJsonSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new SelectorJsonConverter());
            return settings;
        }
    }

    public class ModelSelector<T> : ModelSelector
    {
        private Expression<Func<T, bool>> _compiledExpression;

        public IQueryable<T> ApplyPredicates(IQueryable<T> model)
        {
            var builded = true;
            if (_compiledExpression == null)
                builded = BuildLinqExpression();
            if (builded)
                model = model.Where(_compiledExpression);
            return model;
        }


        private bool BuildLinqExpression()
        {
            var predicates = GetPredicates();
            if (predicates.Length > 0)
            {
                var where = new string[predicates.Length];
                var index = 0;
                var attributes = new List<object>();
                foreach (var selectorPredicate in predicates)
                {
                    where[index] = selectorPredicate.ToDynamicLinqString(index);
                    var attribute = selectorPredicate.GetAttribute();
                    if (attribute != null)
                        attributes.Add(selectorPredicate.GetAttribute());
                    index++;
                }

                var e = DynamicExpressionParser.ParseLambda(false, typeof(T), null, string.Join(" && ", where),
                    attributes.ToArray());
                _compiledExpression = Expression.Lambda<Func<T, bool>>(e.Body, e.Parameters);
                return true;
            }
            return false;
        }

        private Predicate[] GetPredicates()
        {
            return Predicates.ToArray();
        }

        protected override void AddPredicate(Predicate predicate)
        {
            Predicates.Add(predicate);
            if (_compiledExpression != null)
                BuildLinqExpression();
        }


        public ModelSelector<T> Equals<TProperty>(Expression<Func<T, TProperty>> property, TProperty value)
        {
            AddPredicate(new EqualsPredicate<T, TProperty>(property, value));
            return this;
        }

        public ModelSelector<T> NotEquals<TProperty>(Expression<Func<T, TProperty>> property, TProperty value)
        {
            AddPredicate(new NotEqualsPredicate<T, TProperty>(property, value));
            return this;
        }

        public ModelSelector<T> GreaterThan<TProperty>(Expression<Func<T, TProperty>> property, TProperty value)
        {
            AddPredicate(new GreaterThanPredicate<T, TProperty>(property, value));
            return this;
        }

        public ModelSelector<T> GreaterThanEquals<TProperty>(Expression<Func<T, TProperty>> property, TProperty value)
        {
            AddPredicate(new GreaterThanEqualsPredicate<T, TProperty>(property, value));
            return this;
        }

        public ModelSelector<T> LessThan<TProperty>(Expression<Func<T, TProperty>> property, TProperty value)
        {
            AddPredicate(new LessThanPredicate<T, TProperty>(property, value));
            return this;
        }

        public ModelSelector<T> Contains<TProperty>(Expression<Func<T, TProperty>> property, TProperty value)
        {
            AddPredicate(new ContainsPredicate<T, TProperty>(property, value));
            return this;
        }

        public ModelSelector<T> NotContains<TProperty>(Expression<Func<T, TProperty>> property, TProperty value)
        {
            AddPredicate(new NotContainsPredicate<T, TProperty>(property, value));
            return this;
        }

        public ModelSelector<T> LessThanEquals<TProperty>(Expression<Func<T, TProperty>> property, TProperty value)
        {
            AddPredicate(new LessThanEqualsPredicate<T, TProperty>(property, value));
            return this;
        }

        public ModelSelector<T> In<TProperty>(Expression<Func<T, TProperty>> property, IEnumerable<TProperty> values)
        {
            AddPredicate(new InPredicate<T, TProperty>(property, values));
            return this;
        }

        public ModelSelector<T> NotIn<TProperty>(Expression<Func<T, TProperty>> property, IEnumerable<TProperty> values)
        {
            AddPredicate(new NotInPredicate<T, TProperty>(property, values));
            return this;
        }

        public ModelSelector<T> IsNull<TProperty>(Expression<Func<T, TProperty>> property)
        {
            AddPredicate(new IsNullPredicate<T, TProperty>(property));
            return this;
        }

        public ModelSelector<T> NotIsNull<TProperty>(Expression<Func<T, TProperty>> property)
        {
            AddPredicate(new NotIsNullPredicate<T, TProperty>(property));
            return this;
        }
    }
}