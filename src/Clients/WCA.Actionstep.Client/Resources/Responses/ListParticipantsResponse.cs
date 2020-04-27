using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public class ListParticipantsResponse : ActionstepResponseBase<Participant>
    {
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<Participant> Participants { get; set; } = new List<Participant>();
    }
}
