using System.Collections.Generic;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public interface IActionstepResponse
    {
    }

    public interface IActionstepResponse<TResource> : IActionstepResponse
    {
        // Need to figure this out
        // Link[] Links { get; set;  }

        Meta Meta { get; }

        IEnumerable<ActionstepError> Errors { get; set; }
    }

    public abstract class ActionstepResponseBase<TResource> : IActionstepResponse<TResource>
    {
        // Need to figure this out
        // public Link[] Links { get; set; }

        public Meta Meta { get; set; } = new Meta();

        public IEnumerable<ActionstepError> Errors { get; set; }
    }
}
