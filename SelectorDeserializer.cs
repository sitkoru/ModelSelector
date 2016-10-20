using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Sitko.ModelSelector
{
    public class SelectorDeserializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var type = serializer.Deserialize(jsonObject["ModelType"].CreateReader(), typeof(Type)) as Type;
            var selectorType = typeof(ModelSelector<>).MakeGenericType(new[] {type});
            var selector = Activator.CreateInstance(selectorType) as ModelSelector;
            if (selector != null)
            {
                var predicates = serializer.Deserialize<List<Predicate>>(jsonObject["Predicates"].CreateReader());
                selector.AddPredicates(predicates);
            }
            return selector;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ModelSelector).IsAssignableFrom(objectType);
        }
    }
}