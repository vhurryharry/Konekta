using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Domain.CQRS;

using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Actionstep.Connection
{
    public class AddOrUpdateActionstepCredential
    {
        public class AddOrUpdateActionstepCredentialCommand : IAuthenticatedCommand
        {
            public TokenSet TokenSet { get; set; }
            public WCAUser AuthenticatedUser { get; set; }
            public AddOrUpdateActionstepCredentialCommand(TokenSet tokenSet, WCAUser authenticatedUser)
            {
                TokenSet = tokenSet;
                AuthenticatedUser = authenticatedUser;
            }
        }

        public class ValidatorCollection : AbstractValidator<AddOrUpdateActionstepCredentialCommand>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.TokenSet).NotEmpty();
                RuleFor(c => c.TokenSet.UserId).NotEmpty();
                RuleFor(c => c.AuthenticatedUser).NotEmpty();
            }
        }

        public class Handler : AsyncRequestHandler<AddOrUpdateActionstepCredentialCommand>
        {
            private readonly ValidatorCollection _validatorCollection;
            private readonly ITokenSetRepository _tokenSetRepository;

            public Handler(ValidatorCollection validator, ITokenSetRepository tokenSetRepository)
            {
                _validatorCollection = validator;
                _tokenSetRepository = tokenSetRepository;
            }

            protected override async Task Handle(AddOrUpdateActionstepCredentialCommand message, CancellationToken token)
            {
                if (message is null) throw new ArgumentNullException(nameof(message));

                ValidationResult result = _validatorCollection.Validate(message);
                if (!result.IsValid)
                {
                    throw new ValidationException("Unable to save Actionstep credentials, the command message was invalid.", result.Errors);
                }

                if (message.AuthenticatedUser?.Id != message.TokenSet.UserId)
                {
                    throw new InvalidCredentialsForActionstepApiCallException("Unable to save credentials. TokenSet User ID doesn't match authenticated User ID.", message.TokenSet?.OrgKey);
                }

                await _tokenSetRepository.AddOrUpdateTokenSet(message.TokenSet);
            }
        }
    }
}