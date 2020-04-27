using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NodaTime;
using WCA.Actionstep.Client.Converters;

namespace WCA.Actionstep.Client.Resources
{
    public class ActionstepAction
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Reference { get; set; }

        public int Priority { get; set; }

        public string Status { get; set; }

        public OffsetDateTime StatusTimestamp { get; set; }

        [JsonConverter(typeof(ActionstepBooleanConverter))]
        public bool? IsBillableOverride { get; set; }

        public LocalDate CreatedTimestamp { get; set; }

        public OffsetDateTime ModifiedTimestamp { get; set; }

        [JsonConverter(typeof(ActionstepBooleanConverter))]
        public bool? IsDeleted { get; set; }

        public string DeletedBy { get; set; }

        public OffsetDateTime? DeletedTimestamp { get; set; }

        [JsonConverter(typeof(ActionstepBooleanConverter))]
        public bool? IsFavorite { get; set; }

        public string OverrideBillingStatus { get; set; }

        public OffsetDateTime? LastAccessTimestamp { get; set; }

        // public Participant AssignedTo { get; set; }
        public Division Division { get; set; } = new Division();

        public ActionType ActionType { get; set; } = new ActionType();
        // public Participant[] PrimaryParticipants { get; set; }

        public Step Step { get; set; } = new Step();

        public BillSettings BillSettings { get; set; } = new BillSettings();

        public List<ActionstepAction> RelatedActions { get; } = new List<ActionstepAction>();

        public Link Links { get; set; } = new Link();

        public class Link
        {
            public int ActionType { get; set; }
            public int AssignedTo { get; set; }
        }
    }
}
