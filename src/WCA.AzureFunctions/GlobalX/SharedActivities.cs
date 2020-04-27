using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Threading.Tasks;
using WCA.Core.Features.GlobalX;
using static WCA.Core.Features.GlobalX.ValidateActionstepMatterCommand;

namespace WCA.AzureFunctions.GlobalX
{
    public class SharedActivities
    {
        private readonly IMediator _mediator;

        public SharedActivities(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Disable("ALL_JOBS_DISABLED")]
        [FunctionName(nameof(ValidateActionstepMatter))]
        public async Task<ActionstepMatterValidationResult> ValidateActionstepMatter([ActivityTrigger] ValidateActionstepMatterCommand validateActionstepMatterCommand)
        {
            if (validateActionstepMatterCommand is null) throw new ArgumentNullException(nameof(validateActionstepMatterCommand));
            return await _mediator.Send(validateActionstepMatterCommand);
        }
    }
}
