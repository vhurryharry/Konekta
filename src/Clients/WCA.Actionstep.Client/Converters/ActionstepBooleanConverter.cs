using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace WCA.Actionstep.Client.Converters
{
    public class ActionstepBooleanConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(bool);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (reader.TokenType != JsonToken.String)
            {
                // throw new JsonReaderException($"Failed to parse Actionstep boolean value. Expected a string with \"T\" or \"F\"/. Value found was of type {reader.TokenType}");
                // See if this produces a nicer error
                return new JsonSerializer().Deserialize(reader, objectType);
            }

            JToken token = JToken.Load(reader);
            var value = token.Value<string>();

            if ("T" == value)
                return true;
            else if ("F" == value)
                return false;

            // throw new JsonReaderException($"Failed to parse Actionstep boolean value. Expected \"T\" or \"F\"/. Saw {value} instead.");
            // See if this produces a nicer error
            return new JsonSerializer().Deserialize(reader, objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (serializer is null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            if ((bool)value)
                serializer.Serialize(writer, "T");
            else
                serializer.Serialize(writer, "F");
        }
    }
}
