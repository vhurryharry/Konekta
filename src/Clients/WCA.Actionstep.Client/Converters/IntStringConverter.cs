using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace WCA.Actionstep.Client.Converters
{
    public class IntStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(int);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader is null) throw new ArgumentNullException(nameof(reader));

            JToken token = JToken.Load(reader);

            if (token.Type == JTokenType.Null && objectType == typeof(int?))
            {
                return null;
            }

            return token.Value<int>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (serializer is null) throw new ArgumentNullException(nameof(serializer));

            if (value is null)
                serializer.Serialize(writer, value);
            else
                serializer.Serialize(writer, value.ToString());
        }
    }
}
