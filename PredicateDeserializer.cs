using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitko.ModelSelector.Predicates;
using System.Reflection;

namespace Sitko.ModelSelector
{
    public class PredicateDeserializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var propertyType = serializer.Deserialize(jsonObject["PropertyType"].CreateReader(), typeof(Type)) as Type;
            var valueType = serializer.Deserialize(jsonObject["ValueType"].CreateReader(), typeof(Type)) as Type;
            var modelType = serializer.Deserialize(jsonObject["ModelType"].CreateReader(), typeof(Type)) as Type;
            var propertyName = serializer.Deserialize<string>(jsonObject["PropertyName"].CreateReader());
            var value = serializer.Deserialize(jsonObject["value"].CreateReader(), valueType);
            var predicateTypeEnum = serializer.Deserialize<PredicateType>(jsonObject["Type"].CreateReader());
            var predicateType = PredicateTypes[predicateTypeEnum].MakeGenericType(new Type[] {modelType, propertyType});
            var predicate = Activator.CreateInstance(predicateType, propertyName, value);

            return predicate;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Predicate).IsAssignableFrom(objectType);
        }

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
            {PredicateType.NotNull, typeof(NotIsNullPredicate<,>)},
        };
    }
}