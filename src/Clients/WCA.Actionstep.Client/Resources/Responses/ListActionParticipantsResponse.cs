using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public class ListActionParticipantsResponse : ActionstepResponseBase<ActionParticipant>
    {
        public List<ActionParticipant> ActionParticipants { get; } = new List<ActionParticipant>();
        public ListActionParticipantsLinks Linked { get; set; } = new ListActionParticipantsLinks();

        public List<Participant> this[string participantTypeName]
        {
            get
            {
                var participantType = Linked.ParticipantTypes.SingleOrDefault(pt => pt.Name == participantTypeName);
                if (participantType == null) return new List<Participant>();

                var actionParticipants = ActionParticipants
                    .Where(ap => ap.Links.ParticipantType == participantType.Id.ToString(CultureInfo.InvariantCulture))
                    .ToList();

                if (actionParticipants.Count == 0) return null;

                var participants = Linked.Participants
                    .Where(p => actionParticipants.Exists(ap => ap.Links.Participant == p.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();

                return participants;
            }
        }
    }

    public class ListActionParticipantsLinks
    {
        public List<Participant> Participants { get; } = new List<Participant>();
        public List<ParticipantType> ParticipantTypes { get; } = new List<ParticipantType>();
    }
}
