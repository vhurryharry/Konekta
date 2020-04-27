using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Actionstep.Connection
{
    public class RefreshActionstepCredentials
    {
        public class RefreshActionstepCredentialsCommand : IAuthenticatedCommand<TokenSet>
        {
            public RefreshActionstepCredentialsCommand()
            {
            }

            public RefreshActionstepCredentialsCommand(WCAUser authenticatedUser, int actionstepCredentialIdToRefresh, bool forceRefreshIfNotExpired = false)
            {
                AuthenticatedUser = authenticatedUser;
                ActionstepCredentialIdToRefresh = actionstepCredentialIdToRefresh;
                ForceRefreshIfNotExpired = forceRefreshIfNotExpired;
            }

            public WCAUser AuthenticatedUser { get; set; }
            public int ActionstepCredentialIdToRefresh { get; set; }
            public bool ForceRefreshIfNotExpired { get; set; } = false;
        }

        public class ValidatorCollection : AbstractValidator<RefreshActionstepCredentialsCommand>
        {
            public ValidatorCollection()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.ActionstepCredentialIdToRefresh).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<RefreshActionstepCredentialsCommand, TokenSet>
        {
            private readonly ValidatorCollection _validator;
            private readonly IActionstepService _actionstepService;
            private readonly ITokenSetRepository _tokenSetRepository;

            public Handler(
                ValidatorCollection validator,
                IActionstepService actionstepService,
                ITokenSetRepository tokenSetRepository)
            {
                _validator = validator;
                _actionstepService = actionstepService;
                _tokenSetRepository = tokenSetRepository;
            }

            public async Task<TokenSet> Handle(RefreshActionstepCredentialsCommand message, CancellationToken token)
            {
                if (message is null) throw new ArgumentNullException(nameof(message));

                ValidationResult result = _validator.Validate(message);
                if (!result.IsValid)
                {
                    throw new ValidationException("Invalid input.", result.Errors);
                }

                var tokenToRefresh = await _tokenSetRepository.GetTokenSetById(message.ActionstepCredentialIdToRefresh.ToString(CultureInfo.InvariantCulture));

                if (tokenToRefresh == null)
                {
                    throw new InvalidOperationException($"No Actionstep credentials found with the id {message.ActionstepCredentialIdToRefresh}. No credentials to be refreshed.");
                }

                return await _actionstepService.RefreshAccessTokenIfExpired(tokenToRefresh, forceRefresh: true);
            }
        }
    }
}
