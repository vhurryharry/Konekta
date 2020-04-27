using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Actionstep;
using WCA.Domain.CQRS;

namespace WCA.Core.Features.Actionstep
{
    public class ActionstepOrgsQuery : IQuery<IEnumerable<ActionstepOrg>>
    {
        public class ActionstepOrgsQueryHandler : IRequestHandler<ActionstepOrgsQuery, IEnumerable<ActionstepOrg>>
        {
            private WCADbContext _dbContext;

            public ActionstepOrgsQueryHandler(WCADbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task<IEnumerable<ActionstepOrg>> Handle(ActionstepOrgsQuery request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_dbContext.ActionstepOrgs
                    .AsNoTracking()
                    .AsEnumerable());
            }
        }
    }
}
