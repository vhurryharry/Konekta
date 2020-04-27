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

namespace WCA.Core.Features.Actionstep
{
    public class DisconnectFromActionstepOrg
    {
        public class DisconnectFromActionstepOrgCommand : IAuthenticatedCommand
        {
            public string ActionstepOrgKey { get; set; }
            public WCAUser AuthenticatedUser { get; set; }
        }

        public class ValidatorCollection : AbstractValidator<DisconnectFromActionstepOrgCommand>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.ActionstepOrgKey).NotEmpty();
                RuleFor(c => c.AuthenticatedUser).NotEmpty();
            }
        }

        public class Handler : AsyncRequestHandler<DisconnectFromActionstepOrgCommand>
        {
            private readonly ValidatorCollection _validator;
            private readonly ITokenSetRepository _tokenSetRepository;

            public Handler(
                ValidatorCollection validator,
                ITokenSetRepository tokenSetRepository)
            {
                _validator = validator;
                _tokenSetRepository = tokenSetRepository;
            }

            protected override async Task Handle(DisconnectFromActionstepOrgCommand message, CancellationToken token)
            {
                if (message is null) throw new ArgumentNullException(nameof(message));

                ValidationResult result = _validator.Validate(message);
                if (!result.IsValid)
                {
                    throw new ValidationException("Unable to disconnect from Actionstep organisaion, the command message was invalid.", result.Errors);
                }

                await _tokenSetRepository.Remove(new TokenSetQuery(message.AuthenticatedUser?.Id, message.ActionstepOrgKey));
             }
        }
    }
}
