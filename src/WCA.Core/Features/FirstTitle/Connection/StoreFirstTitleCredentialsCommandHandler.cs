using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace WCA.Core.Features.FirstTitle.Connection
{
    public class StoreFirstTitleCredentialsCommandHandler : AsyncRequestHandler<StoreFirstTitleCredentialsCommand>
    {
        private readonly IFirstTitleCredentialRepository _firstTitleCredentialRepository;

        public StoreFirstTitleCredentialsCommandHandler(
            IFirstTitleCredentialRepository firstTitleCredentialRepository)
        {
            _firstTitleCredentialRepository = firstTitleCredentialRepository;
        }

        protected override async Task Handle(StoreFirstTitleCredentialsCommand command, CancellationToken token)
        {
            await _firstTitleCredentialRepository.SaveOrUpdateCredential(command);
        }
    }
}
