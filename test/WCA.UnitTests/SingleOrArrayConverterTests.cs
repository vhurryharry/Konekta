using Newtonsoft.Json;
using System.Collections.Generic;
using WCA.Core;
using Xunit;

namespace WCA.UnitTests
{
    public class SingleOrArrayConverterTests
    {
        [Fact]
        public void CanDeserializeWithArray()
        {
            var sample = @"
{
    ""something"": [
        {
            ""one"": 2
        }
    ]
}
";

            var result = JsonConvert.DeserializeObject<Rootobject>(sample);

            Assert.Equal(2, result.something[0].one);
        }

        [Fact]
        public void CanDeserializeWithObject()
        {
            var sample = @"
{
    ""something"": {
        ""one"": 3
    }
}
";

            var result = JsonConvert.DeserializeObject<Rootobject>(sample);

            Assert.Equal(3, result.something[0].one);
        }
    }

    public class Rootobject
    {
        [JsonConverter(typeof(SingleOrArrayConverter<Something>))]
#pragma warning disable IDE1006 // Naming Styles: DTO for unit test
#pragma warning disable CA2227 // Naming Styles: DTO for unit test
        public List<Something> something { get; set; }
#pragma warning restore CA2227 // Naming Styles
#pragma warning restore IDE1006 // Naming Styles
    }

    public class Something
    {
        public int one { get; set; }
    }
}