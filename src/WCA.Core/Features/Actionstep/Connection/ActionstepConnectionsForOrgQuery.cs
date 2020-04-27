using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.CQRS;
using static WCA.Core.Features.Actionstep.Connection.ActionstepConnectionsForOrgQuery;

namespace WCA.Core.Features.Actionstep.Connection
{
    public class ActionstepConnectionsForOrgQuery : IQuery<IEnumerable<ActionstepConnection>>
    {
        public string ActionstepOrgKey { get; set; }

        public class ActionstepConnection
        {
            public int Id { get; set; }
            public string OrgKey { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string UserId { get; set; }
            public string Status { get; set; }
            public DateTime Expiration { get; set; }
        }

        public class ActionstepConnectionsForUserQueryHandler : IRequestHandler<ActionstepConnectionsForOrgQuery, IEnumerable<ActionstepConnection>>
        {
            private WCADbContext _dbContext;

            public ActionstepConnectionsForUserQueryHandler(WCADbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task<IEnumerable<ActionstepConnection>> Handle(ActionstepConnectionsForOrgQuery request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_dbContext.ActionstepCredentials
                    .Include(a => a.ActionstepOrg)
                    .Include(a => a.Owner)
                    .Where(a => a.ActionstepOrg.Key == request.ActionstepOrgKey)
                    .Select(a => new ActionstepConnection
                    {
                        Id = a.Id,
                        OrgKey = a.ActionstepOrg.Key,
                        Email = a.Owner.Email,
                        FirstName = a.Owner.FirstName,
                        LastName = a.Owner.LastName,
                        UserId = a.Owner.Id,
                        Status = a.RefreshTokenIsValidAndNotExpired() ? "Active" : "Expired",
                        Expiration = a.RefreshTokenExpiryUtc
                    })
                    .AsEnumerable());
            }
        }
    }
}
