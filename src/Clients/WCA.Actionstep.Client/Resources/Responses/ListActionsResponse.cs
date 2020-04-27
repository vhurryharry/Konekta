using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using WCA.Actionstep.Client.Converters;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public class ListActionsResponse : ActionstepResponseBase<ActionstepAction>
    {
        [JsonConverter(typeof(SingleOrArrayConverter<ActionstepAction>))]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<ActionstepAction> Actions { get; set; } = new List<ActionstepAction>();
    }

    public class ParticipantLinks
    {
        public string PhysicalCountry { get; set; }
        public string MailingCountry { get; set; }
        public string Division { get; set; }
    }

    public class ActionParticipantLinks
    {
        public string Action { get; set; }
        public string ParticipantType { get; set; }
        public string Participant { get; set; }
    }
}
