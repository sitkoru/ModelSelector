using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitko.ModelSelector.Base;
using Sitko.ModelSelector.Exceptions;
using Sitko.ModelSelector.Predicates;

namespace Sitko.ModelSelector.Json
{
    public class SelectorJsonConverter : JsonConverter
    {
        private static readonly Dictionary<PredicateType, Type> PredicateTypes = new Dictionary<PredicateType, Type>
        {
            {PredicateType.Equals, typeof(EqualsPredicate<,>)},
            {PredicateType.NotEquals, typeof(NotEqualsPredicate<,>)},
            {PredicateType.GreaterThan, typeof(GreaterThanPredicate<,>)},
            {PredicateType.GreaterThanEquals, typeof(GreaterThanEqualsPredicate<,>)},
            {PredicateType.LessThan, typeof(LessThanPredicate<,>)},
            {PredicateType.LessThanEquals, typeof(LessThanEqualsPredicate<,>)},
            {PredicateType.In, typeof(InPredicate<,>)},
            {PredicateType.NotIn, typeof(NotInPredicate<,>)},
            {PredicateType.Contains, typeof(ContainsPredicate<,>)},
            {PredicateType.NotContains, typeof(NotContainsPredicate<,>)},
            {PredicateType.IsNull, typeof(IsNullPredicate<,>)},
            {PredicateType.NotNull, typeof(NotIsNullPredicate<,>)}
        };

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = JToken.FromObject(value);
            t.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var selector = Activator.CreateInstance(objectType) as Base.ModelSelector;
            if (selector != null)
            {
                var predicatesToken = jsonObject["predicates"];
                var predicates = new List<Predicate>();
                var modelType = objectType.GetTypeInfo().GenericTypeArguments[0];
                foreach (var predicateToken in predicatesToken)
                    predicates.Add(ParsePredicate(predicateToken, modelType, serializer));
                selector.AddPredicates(predicates);
            }
            return selector;
        }

        private static Predicate ParsePredicate(JToken predicateToken, Type modelType, JsonSerializer serializer)
        {
            var propertyName = serializer.Deserialize<string>(predicateToken["propertyName"].CreateReader());
            var property = modelType.GetProperty(propertyName);

            if (property == null)
                throw new PropertyNotFoundException($"Property {propertyName} not found in type {modelType}");

            var propertyType = modelType.GetProperty(propertyName).PropertyType;
            var isMultiple = serializer.Deserialize<bool>(predicateToken["isMultiple"].CreateReader());
            var valueType = !isMultiple ? propertyType : typeof(IEnumerable<>).MakeGenericType(propertyType);
            var value = serializer.Deserialize(predicateToken["value"].CreateReader(), valueType);
            var predicateTypeEnum = serializer.Deserialize<PredicateType>(predicateToken["type"].CreateReader());
            var predicateType = PredicateTypes[predicateTypeEnum].MakeGenericType(modelType, propertyType);
            var predicate = Activator.CreateInstance(predicateType, propertyName, value) as Predicate;

            return predicate;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Base.ModelSelector).IsAssignableFrom(objectType);
        }
    }
}