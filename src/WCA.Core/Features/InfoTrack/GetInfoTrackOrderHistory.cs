using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.InfoTrack
{
    public class GetInfoTrackOrderHistory
    {
        public class GetInfoTrackOrderHistoryQuery : IRequest<InfoTrackOrderResult[]>
        {
            public WCAUser AuthenticatedUser { get; set; }
            public DateTime OrderedFromDateUtc { get; set; }
            public DateTime OrderedToDateUtc { get; set; }

            /// <summary>
            /// Optional Actionstep Organisation Key to filter by.
            /// </summary>
            public string OrgKey { get; set; }

            /// <summary>
            /// Optional Actionstep Matter ID (Action ID) to filter by.
            /// </summary>
            public int? MatterId { get; set; }
        }

        public class Validator : AbstractValidator<GetInfoTrackOrderHistoryQuery>
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
            }
        }

        public class Handler : IRequestHandler<GetInfoTrackOrderHistoryQuery, InfoTrackOrderResult[]>
        {
            private readonly Validator validator;
            private readonly WCADbContext wCADbContext;
            private readonly IMediator mediator;
            private readonly IMapper mapper;

            public Handler(
                Validator validator,
                WCADbContext wCADbContext,
                IMediator mediator,
                IMapper mapper)
            {
                this.validator = validator;
                this.wCADbContext = wCADbContext;
                this.mediator = mediator;
                this.mapper = mapper;
            }

            public async Task<InfoTrackOrderResult[]> Handle(GetInfoTrackOrderHistoryQuery message, CancellationToken token)
            {
                ValidationResult result = validator.Validate(message);
                if (!result.IsValid)
                {
                    throw new ValidationException("Invalid input.", result.Errors);
                }

                // Only return orders for orgs for which the authenticated user has a valid
                // refresh token. This implies that they have valid credentials / permissions
                // for that org.

                // TODO: Server side pagination! :)

                var query = wCADbContext.InfoTrackOrders
                    .AsNoTracking()
                    .Include(order => order.ActionstepOrg)
                    .Where(order => order.InfoTrackDateOrderedUtc >= message.OrderedFromDateUtc)
                    .Where(order => order.InfoTrackDateOrderedUtc < message.OrderedToDateUtc)
                    .Where(order => wCADbContext.ActionstepCredentials
                            .Where(actionstepCredential =>
                                actionstepCredential.RefreshToken != null &&
                                actionstepCredential.RefreshToken.Length > 0 &&
                                actionstepCredential.RefreshTokenExpiryUtc > DateTime.UtcNow)
                            .Any(actionstepCredential =>
                                actionstepCredential.Owner == message.AuthenticatedUser &&
                                actionstepCredential.ActionstepOrg == order.ActionstepOrg));

                if (!string.IsNullOrEmpty(message.OrgKey))
                    query = query.Where(order => order.ActionstepOrg.Key == message.OrgKey);

                if (message.MatterId.HasValue)
                    query = query.Where(order => order.ActionstepMatterId == message.MatterId);

                var results = await query
                    .OrderBy(order => order.InfoTrackDateOrderedUtc)
                    .ToArrayAsync();

                return mapper.Map<InfoTrackOrderResult[]>(results);
            }
        }
    }
}
