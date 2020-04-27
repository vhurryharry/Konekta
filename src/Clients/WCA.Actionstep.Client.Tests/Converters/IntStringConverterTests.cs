using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WCA.Actionstep.Client.Converters;
using Xunit;

namespace WCA.Actionstep.Client.Tests.Converters
{
    public class IntStringConverterTests
    {
        public class SerializableInt
        {
            [JsonConverter(typeof(IntStringConverter))]
            public int IntAsString { get; set; }
        }

        public class SerializableNullableInt
        {
            [JsonConverter(typeof(IntStringConverter))]
            public int? NullableIntAsString { get; set; }
        }

        [Fact]
        public void CanSerializeToString()
        {
            var testObject = new SerializableInt() { IntAsString = 1 };
            var jsonString = JsonConvert.SerializeObject(testObject);
            Assert.Equal(@"{""IntAsString"":""1""}", jsonString);
        }

        [Fact]
        public void CanDeserializeFromString()
        {
            var deserializedObject = JsonConvert.DeserializeObject<SerializableInt>(@"{""IntAsString"":""1""}");
            Assert.Equal(1, deserializedObject.IntAsString);
        }

        [Fact]
        public void CanDeserializeFromInt()
        {
            var deserializedObject = JsonConvert.DeserializeObject<SerializableInt>(@"{""IntAsString"":1}");
            Assert.Equal(1, deserializedObject.IntAsString);
        }

        [Fact]
        public void DeserializeIntFromNullThrows()
        {
            var ex = Assert.Throws<InvalidCastException>(() =>
            {
                var deserializedObject = JsonConvert.DeserializeObject<SerializableInt>(@"{""IntAsString"":null}");
            });

            Assert.Equal("Null object cannot be converted to a value type.", ex.Message);
        }

        [Fact]
        public void DeserializeNonIntThrows()
        {
            var ex = Assert.Throws<FormatException>(() =>
            {
                var deserializedObject = JsonConvert.DeserializeObject<SerializableInt>(@"{""IntAsString"":""NotANumber""}");
            });

            Assert.Equal("Input string was not in a correct format.", ex.Message);
        }

        [Fact]
        public void CanDeserializeNullableNull()
        {
            var deserializedObject = JsonConvert.DeserializeObject<SerializableNullableInt>(@"{""NullableIntAsString"":null}");
            Assert.Null(deserializedObject.NullableIntAsString);
        }

        [Fact]
        public void CanDeserializeNullableInt()
        {
            var deserializedObject = JsonConvert.DeserializeObject<SerializableNullableInt>(@"{""NullableIntAsString"":1}");
            Assert.Equal(1, deserializedObject.NullableIntAsString);
        }

        [Fact]
        public void CanSerializeNullableNull()
        {
            var testObject = new SerializableNullableInt() { NullableIntAsString = null };
            var jsonString = JsonConvert.SerializeObject(testObject);
            Assert.Equal(@"{""NullableIntAsString"":null}", jsonString);
        }

        [Fact]
        public void CanSerializeNullableInt()
        {
            var testObject = new SerializableNullableInt() { NullableIntAsString = 1 };
            var jsonString = JsonConvert.SerializeObject(testObject);
            Assert.Equal(@"{""NullableIntAsString"":""1""}", jsonString);
        }
    }
}