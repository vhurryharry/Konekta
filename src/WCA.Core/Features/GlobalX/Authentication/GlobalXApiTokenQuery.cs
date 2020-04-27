using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.GlobalX.Client.Authentication;

namespace WCA.Core.Features.GlobalX.Authentication
{
    public class GlobalXApiTokenQuery : IAuthenticatedQuery<GlobalXApiToken>
    {
        public WCAUser AuthenticatedUser { get; set; }

        public class Validator : AbstractValidator<GlobalXApiTokenQuery>
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
            }
        }

        public class GlobalXApiTokenQueryHandler : IRequestHandler<GlobalXApiTokenQuery, GlobalXApiToken>
        {
            private readonly Validator _validator;
            private readonly IGlobalXApiTokenRepository _globalXApiTokenRepository;

            public GlobalXApiTokenQueryHandler(
                IGlobalXApiTokenRepository globalXApiTokenRepository,
                Validator validator)
            {
                _globalXApiTokenRepository = globalXApiTokenRepository ?? throw new System.ArgumentNullException(nameof(globalXApiTokenRepository));
                _validator = validator ?? throw new System.ArgumentNullException(nameof(validator));
            }

            public async Task<GlobalXApiToken> Handle(GlobalXApiTokenQuery query, CancellationToken cancellationToken)
            {
                if (query is null) throw new System.ArgumentNullException(nameof(query));
                await _validator.ValidateAndThrowAsync(query);

                return await _globalXApiTokenRepository.GetTokenForUser(query.AuthenticatedUser.Id);
            }
        }
    }
}
