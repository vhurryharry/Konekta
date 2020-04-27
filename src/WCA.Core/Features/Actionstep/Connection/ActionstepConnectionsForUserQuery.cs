using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using static WCA.Core.Features.Actionstep.Connection.ActionstepConnectionsForUserQuery;

namespace WCA.Core.Features.Actionstep.Connection
{
    public class ActionstepConnectionsForUserQuery : IAuthenticatedQuery<List<ActionstepConnectionsResponse>>
    {
        public WCAUser AuthenticatedUser { get; set; }

        public class ActionstepConnectionsResponse
        {
            public int Id { get; set; }
            public string Key { get; set; }
            public string Status { get; set; }
            public DateTime Expiration { get; set; }
        }

        public class ActionstepConnectionsForUserQueryHandler : IRequestHandler<ActionstepConnectionsForUserQuery, List<ActionstepConnectionsResponse>>
        {
            private WCADbContext _dbContext;

            public ActionstepConnectionsForUserQueryHandler(WCADbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task<List<ActionstepConnectionsResponse>> Handle(ActionstepConnectionsForUserQuery request, CancellationToken cancellationToken)
            {
                return _dbContext.ActionstepCredentials
                    .Include(a => a.ActionstepOrg)
                    .Where(a => a.Owner == request.AuthenticatedUser)
                    .Select(a => new ActionstepConnectionsResponse
                    {
                        Id = a.Id,
                        Key = a.ActionstepOrg.Key,
                        Status = a.RefreshTokenIsValidAndNotExpired() ? "Active" : "Expired",
                        Expiration = a.RefreshTokenExpiryUtc
                    })
                    .ToListAsync();
            }
        }
    }
}
