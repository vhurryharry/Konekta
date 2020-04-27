using Newtonsoft.Json;
using WCA.Actionstep.Client.Converters;

namespace WCA.Actionstep.Client.Resources
{
    public class ActionstepUser
    {
        public int Id { get; set; }

        [JsonConverter(typeof(ActionstepBooleanConverter))]
        public bool IsCurrent { get; set; }

        public string EmailAddress { get; set; }

        [JsonConverter(typeof(ActionstepBooleanConverter))]
        public bool IsActive { get; set; }

        [JsonConverter(typeof(ActionstepBooleanConverter))]
        public bool HasAuthority { get; set; }
    }
}
