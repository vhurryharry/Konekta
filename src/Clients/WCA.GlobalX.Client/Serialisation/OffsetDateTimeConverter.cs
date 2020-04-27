using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using NodaTime.Text;
using NodaTime.Utility;
using System;

namespace WCA.GlobalX.Client.Serialisation
{
    /// <summary>
    /// Required because the Azure Durable Functions runtime doesn't allow us to
    /// customise its <see cref="JsonSerializerSettings"/>, and the way that NodaTime
    /// JsonConverters are constructed doesn't allow them to be used directly via the
    /// <see cref="JsonConverter"/> attribute.
    /// </summary>
    public class OffsetDateTimeConverter : NodaConverterBase<OffsetDateTime>
    {
        private readonly OffsetDateTimePattern _pattern = OffsetDateTimePattern.Rfc3339;

        protected override OffsetDateTime ReadJsonImpl(JsonReader reader, JsonSerializer serializer)
        {
            if (reader is null) throw new ArgumentNullException(nameof(reader));

            if (reader.TokenType != JsonToken.String)
            {
                throw new InvalidNodaDataException(
                    $"Unexpected token parsing {typeof(OffsetDateTime).Name}. Expected String, got {reader.TokenType}.");
            }
            string text = reader.Value.ToString();
            return OffsetDateTimePattern.Rfc3339.Parse(text).Value;
        }

        protected override void WriteJsonImpl(JsonWriter writer, OffsetDateTime value, JsonSerializer serializer)
        {
            if (writer is null) throw new ArgumentNullException(nameof(writer));
            writer.WriteValue(_pattern.Format(value));
        }
    }
}
