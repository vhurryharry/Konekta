
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WCA.FirstTitle.Client;

namespace WCA.Core.Features.FirstTitle.Connection
{
    public class FirstTitleCredentialsQueryHandler: IRequestHandler<FirstTitleCredentialsQuery, FirstTitleCredential>
    {
        private readonly FirstTitleCredentialsQuery.Validator _validator;
        private readonly IFirstTitleCredentialRepository _firstTitleCredentialRepository;

        public FirstTitleCredentialsQueryHandler(
            IFirstTitleCredentialRepository firstTitleCredentialRepository,
            FirstTitleCredentialsQuery.Validator validator)
        {
            _firstTitleCredentialRepository = firstTitleCredentialRepository;
            _validator = validator;
        }

        public async Task<FirstTitleCredential> Handle(FirstTitleCredentialsQuery query, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(query);

            return await _firstTitleCredentialRepository.FindCredential(query.AuthenticatedUser);
        }
    }
}
