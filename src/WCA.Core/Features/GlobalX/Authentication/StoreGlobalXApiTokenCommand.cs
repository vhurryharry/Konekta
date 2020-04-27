using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.GlobalX.Client;
using WCA.GlobalX.Client.Authentication;

namespace WCA.Core.Features.GlobalX.Authentication
{
    public class StoreGlobalXApiTokenCommand : IAuthenticatedCommand
    {
        public WCAUser AuthenticatedUser { get; set; }
        public GlobalXApiToken GlobalXApiToken { get; set; }
        public bool OverrideAndClearLock { get; set; }

        public class Validator : AbstractValidator<StoreGlobalXApiTokenCommand>
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.GlobalXApiToken).NotEmpty();
            }
        }

        public class StoreGlobalXApiTokenCommandHandler : AsyncRequestHandler<StoreGlobalXApiTokenCommand>
        {
            private readonly Validator _validator;
            private readonly IGlobalXApiTokenRepository _globalXApiTokenRepository;

            public StoreGlobalXApiTokenCommandHandler(
                IGlobalXApiTokenRepository globalXApiTokenRepository,
                Validator validator)
            {
                _globalXApiTokenRepository = globalXApiTokenRepository;
                _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            }

            protected override async Task Handle(StoreGlobalXApiTokenCommand command, CancellationToken token)
            {
                if (command is null) throw new ArgumentNullException(nameof(command));
                await _validator.ValidateAndThrowAsync(command);
                await _globalXApiTokenRepository.AddOrUpdateGlobalXApiToken(command.GlobalXApiToken, null, overrideAndClearLock: true);
            }
        }
    }
}
