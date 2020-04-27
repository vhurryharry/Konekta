using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace WCA.Core
{
    /// <summary>
    /// JsonConverter that allows for a property to either be a single object
    /// or an array of that object type. Allows deserialization of either
    /// in to an array. I.e. the POCO can safely be set to an array and
    /// will always be deserialised as such, even if only a single object
    /// is returned in the JSON without an array.
    ///
    /// Credit goes to Brian Rogers:
    /// https://stackoverflow.com/questions/18994685/how-to-handle-both-a-single-item-and-an-array-for-the-same-property-using-json-n
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleOrArrayConverter<T> : JsonConverter
    {
        /// <summary>
        /// Whether to also write single objects without an array.
        ///
        /// If set to true, where a single object is to be serialized,
        /// the JSON object will be written directly as an object instead
        /// of an array (even if the type specifies an array).
        ///
        /// If set to false, writing will be disabled and JSON will be
        /// serialized as per usual (arrays will always be serialized as
        /// arrays, even if empty or with a single object).
        /// </summary>
        public bool WriteSingleAlso { get; set; } = false;

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<T>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                return token.ToObject<List<T>>();
            }
            return new List<T> { token.ToObject<T>() };
        }

        public override bool CanWrite => WriteSingleAlso;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // I think this if statement is redundant because the value of CanWrite
            // should mean that WriteJson never gets called if CanWrite is false.
            // This is just to be on the safe side.
            if (WriteSingleAlso)
            {
                List<T> list = (List<T>)value;
                if (list.Count == 1)
                {
                    value = list[0];
                }
                serializer.Serialize(writer, value);
            }
        }
    }
}
