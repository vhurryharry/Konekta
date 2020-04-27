using Newtonsoft.Json;
using WCA.Actionstep.Client.Converters;

namespace WCA.Actionstep.Client.Resources
{
    public class DataCollection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(ActionstepBooleanConverter))]
        public bool MultipleRecords { get; set; }
        public int Order { get; set; }
        public string Label { get; set; }
        [JsonConverter(typeof(ActionstepBooleanConverter))]
        public bool AlwaysShowDescriptions { get; set; }
        public Link Links { get; set; }

        public class Link
        {
            public int ActionType { get; set; }
        }
    }
}
