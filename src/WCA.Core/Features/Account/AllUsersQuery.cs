using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Actionstep
{
    public class AllUsersQuery : IQuery<IEnumerable<WCAUser>>
    {
        public class AllUsersQueryHandler : IRequestHandler<AllUsersQuery, IEnumerable<WCAUser>>
        {
            private UserManager<WCAUser> _userManager;

            public AllUsersQueryHandler(UserManager<WCAUser> userManager)
            {
                _userManager = userManager;
            }

            public Task<IEnumerable<WCAUser>> Handle(AllUsersQuery request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_userManager.Users.AsEnumerable());
            }
        }
    }
}
