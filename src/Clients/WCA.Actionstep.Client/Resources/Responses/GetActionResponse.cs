using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public class GetActionResponse : ActionstepResponseBase<ActionstepAction>
    {
        [JsonProperty("actions")]
        public ActionstepAction Action { get; set; } = new ActionstepAction();
        public ActionLinked Linked { get; set; } = new ActionLinked();
        public string OrgName => Linked.Participants?.SingleOrDefault(p => p.IsCompany)?.CompanyName;

        // Derived Properties
        [JsonIgnore]
        public string ActionTypeName
        {
            get
            {
                var actionTypeId = Action.Links.ActionType;
                var actionType = Linked.ActionTypes.SingleOrDefault(a => a.Id == actionTypeId);
                return actionType?.Name ?? string.Empty;
            }
        }
    }

    public class ActionLinked
    {
        public IList<ActionType> ActionTypes { get; } = new List<ActionType>();
        public IList<Participant> Participants { get; } = new List<Participant>();
    }
}
