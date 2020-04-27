using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Actionstep;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class StorePexaWorkspaceInfoIntoDBCommandHandler : IRequestHandler<StorePexaWorkspaceInfoIntoDBCommand, bool>
    {
        private readonly WCADbContext _wCADbContext;
        private readonly StorePexaWorkspaceInfoIntoDBCommand.ValidatorCollection _validator;

        public StorePexaWorkspaceInfoIntoDBCommandHandler(
            StorePexaWorkspaceInfoIntoDBCommand.ValidatorCollection validator,
            WCADbContext wCADbContext)
        {
            _validator = validator;
            _wCADbContext = wCADbContext;
        }

        public async Task<bool> Handle(StorePexaWorkspaceInfoIntoDBCommand command, CancellationToken cancellationToken)
        {
            if (command is null) throw new System.ArgumentNullException(nameof(command));

            await _validator.ValidateAndThrowAsync(command);

            var existingWorkspace = _wCADbContext.PexaWorkspaces.FirstOrDefault(p => p.ActionstepOrg == command.ActionstepOrg && p.MatterId == command.MatterId);

            if (existingWorkspace != default(PexaWorkspace))
            {
                _wCADbContext.PexaWorkspaces.Remove(existingWorkspace);
            }

            _wCADbContext.PexaWorkspaces.Add(new PexaWorkspace()
            {
                WorkspaceId = command.WorkspaceId,
                WorkspaceUri = command.WorkspaceUri,
                ActionstepOrg = command.ActionstepOrg,
                MatterId = command.MatterId
            });

            await _wCADbContext.SaveChangesAsync();

            return true;
        }
    }
}
