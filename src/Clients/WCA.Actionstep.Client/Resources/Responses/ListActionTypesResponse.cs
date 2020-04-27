using System.Collections.Generic;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public class ListActionTypesResponse : ActionstepResponseBase<ActionType>
    {
        public List<ActionType> ActionTypes { get; }
    }
}
