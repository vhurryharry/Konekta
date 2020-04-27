using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.InfoTrack
{
    public class GetInfoTrackOrderHistoryPreview
    {
        public class GetInfoTrackOrderHistoryPreviewQuery : IRequest<List<InfoTrackOrderResult>>
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

            public string Keyword { get; set; }
        }

        public class Validator : AbstractValidator<GetInfoTrackOrderHistoryPreviewQuery>
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
            }
        }

        public class Handler : IRequestHandler<GetInfoTrackOrderHistoryPreviewQuery, List<InfoTrackOrderResult>>
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

            public async Task<List<InfoTrackOrderResult>> Handle(GetInfoTrackOrderHistoryPreviewQuery message, CancellationToken token)
            {
                ValidationResult result = validator.Validate(message);
                if (!result.IsValid)
                {
                    throw new ValidationException("Invalid input.", result.Errors);
                }

                // TODO: Return all orders for any matter who has an order in the filter criteria

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

                if (!String.IsNullOrEmpty(message.Keyword))
                {
                    query = query.Where(o => EF.Functions.FreeText(o.InfoTrackDescription, message.Keyword) ||
                                EF.Functions.FreeText(o.InfoTrackClientReference, message.Keyword) || 
                                o.ActionstepMatterId.ToString().Contains(message.Keyword) ||
                                o.InfoTrackOrderId.ToString().Contains(message.Keyword) ||
                                o.InfoTrackParentOrderId.ToString().Contains(message.Keyword));
                }

                var results = await query
                    .OrderBy(order => order.InfoTrackDateOrderedUtc)
                    .ToArrayAsync();

                return mapper.Map<List<InfoTrackOrderResult>>(results);
            }
        }
    }
}
