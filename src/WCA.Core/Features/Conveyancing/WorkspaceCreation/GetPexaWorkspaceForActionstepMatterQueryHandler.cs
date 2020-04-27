using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Actionstep;
using WCA.Domain.Pexa;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    class GetPexaWorkspaceForActionstepMatterQueryHandler : IRequestHandler<GetPexaWorkspaceForActionstepMatterQuery, PexaWorkspaceInfo>
    {
        private readonly WCADbContext _wCADbContext;
        private readonly GetPexaWorkspaceForActionstepMatterQuery.ValidatorCollection _validator;

        public GetPexaWorkspaceForActionstepMatterQueryHandler(
            GetPexaWorkspaceForActionstepMatterQuery.ValidatorCollection validator,
            WCADbContext wCADbContext)
        {
            _validator = validator;
            _wCADbContext = wCADbContext;
        }

        public async Task<PexaWorkspaceInfo> Handle(GetPexaWorkspaceForActionstepMatterQuery message, CancellationToken token)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            await _validator.ValidateAndThrowAsync(message);

            if ((message.ActionstepOrg == "btrcdemo" && message.MatterId == 30) || (message.ActionstepOrg == "trial181078920" && message.MatterId == 23)
                || (message.ActionstepOrg == "ktademo" && message.MatterId == 8))
                return null;

            var existingWorkspace = _wCADbContext.PexaWorkspaces.FirstOrDefault(p => p.ActionstepOrg == message.ActionstepOrg && p.MatterId == message.MatterId);

            if (existingWorkspace == default(PexaWorkspace))
            {
                return null;
            }

            return new PexaWorkspaceInfo()
            {
                PexaWorkspaceId = existingWorkspace.WorkspaceId,
                PexaWorkspaceUri = existingWorkspace.WorkspaceUri
            };
        }
    }
}
