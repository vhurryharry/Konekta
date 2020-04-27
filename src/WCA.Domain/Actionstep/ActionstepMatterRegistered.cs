using NodaTime;
using WCA.Domain.CQRS;

namespace WCA.Domain.Actionstep
{
    public class ActionstepMatterRegistered : EventBase
    {
        public string OrgKey { get; }
        public int MatterId { get; }
        public string RequestedByUserId { get; }

        public ActionstepMatterRegistered(Instant eventCreatedAt, string orgKey, int matterId, string requestedByUserId)
            : base(eventCreatedAt)
        {
            OrgKey = orgKey;
            MatterId = matterId;
            RequestedByUserId = requestedByUserId;
        }
    }
}
