using FluentValidation;
using MediatR;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Data;
using WCA.Domain.CQRS;
using static WCA.Core.Features.GlobalX.ValidateActionstepMatterCommand;

namespace WCA.Core.Features.GlobalX
{
    public class ValidateActionstepMatterCommand : ICommand<ActionstepMatterValidationResult>
    {
        public string ActionstepUserId { get; set; }
        public string ActionstepOrgKey { get; set; }

        public string MatterId { get; set; }
        public int MinimumMatterIdToSync { get; set; }

        public ValidateActionstepMatterCommand()
        { }

        public class Validator : AbstractValidator<ValidateActionstepMatterCommand>
        {
            public Validator()
            {
                RuleFor(d => d.ActionstepUserId).NotEmpty();
                RuleFor(d => d.ActionstepOrgKey).NotEmpty();
            }
        }

        public class ValidateActionstepMatterCommandHandler : IRequestHandler<ValidateActionstepMatterCommand, ActionstepMatterValidationResult>
        {
            private readonly IActionstepService _actionstepService;
            private readonly WCADbContext _wCADbContext;
            private readonly Validator _validator;

            public ValidateActionstepMatterCommandHandler(
                IActionstepService actionstepService,
                WCADbContext wCADbContext,
                Validator validator)
            {
                _actionstepService = actionstepService;
                _wCADbContext = wCADbContext;
                _validator = validator;
            }

            public async Task<ActionstepMatterValidationResult> Handle(ValidateActionstepMatterCommand request, CancellationToken cancellationToken)
            {
                if (request is null) throw new ArgumentNullException(nameof(request));
                _validator.ValidateAndThrow(request);

                var tokenSetQuery = new TokenSetQuery(request.ActionstepUserId, request.ActionstepOrgKey);
                int? matterId = null;
                bool isMapped = false;

                // First check for mapping to see if this GlobalX Matter reference has been mapped to another Actionstep Matter ID
                var gxMapping = await _wCADbContext.GlobalXMatterMappings.FindAsync(request.ActionstepOrgKey, request.MatterId);
                if (!(gxMapping is null))
                {
                    matterId = gxMapping.ActionstepMatterId;
                    isMapped = true;
                }
                else
                {
                    // No mapping found, so proceed with regular validation against Actionstep
                    if (int.TryParse(request.MatterId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedMatterId))
                    {
                        matterId = parsedMatterId;
                    }
                }

                MatterIdStatus matterIdStatus;
                if (matterId.HasValue)
                {
                    if (matterId < request.MinimumMatterIdToSync)
                    {
                        matterIdStatus = MatterIdStatus.InvalidBelowMinimum;
                    }
                    else if (await MatterExistsInActionstep(tokenSetQuery, matterId.Value))
                    {
                        matterIdStatus = MatterIdStatus.Valid;
                    }
                    else
                    {
                        matterIdStatus = MatterIdStatus.InvalidNotFoundInActionstep;
                    }
                }
                else
                {
                    matterIdStatus = MatterIdStatus.InvalidUnableToParseAsInt;
                }

                return new ActionstepMatterValidationResult(matterIdStatus, matterId, isMapped);
            }

            private async Task<bool> MatterExistsInActionstep(TokenSetQuery tokenSetQuery, int matterId)
            {
                var matterInfo = await _actionstepService.Handle<GetActionResponse>(new GetActionRequest(tokenSetQuery, matterId));

                if (matterInfo is null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public class ActionstepMatterValidationResult
        {
            public MatterIdStatus MatterIdStatus { get; }

            public bool MatterHasBeenMapped { get; }
            public int? MatterId { get; }


            public ActionstepMatterValidationResult(
                MatterIdStatus matterIdStatus,
                int? matterId = null,
                bool matterHasBeenMapped = false)
            {
                MatterIdStatus = matterIdStatus;
                MatterId = matterId;
                MatterHasBeenMapped = matterHasBeenMapped;
            }
        }

        public enum MatterIdStatus
        {
            // Valid
            Valid = 100,

            // Invalid codes
            InvalidBelowMinimum = 200,
            InvalidUnableToParseAsInt = 201,
            InvalidNotFoundInActionstep = 202,
            InvalidUnknownValidationError = 203,
        }
    }
}