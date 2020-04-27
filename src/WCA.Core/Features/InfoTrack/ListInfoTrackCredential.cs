using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.InfoTrack;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.InfoTrack
{
    public class ListInfoTrackCredential
    {
        public class ListInfoTrackCredentialQuery : IRequest<List<ListInfoTrackCredentialResponse>>
        {
            public WCAUser AuthenticatedUser { get; set; }
        }

        public class ListInfoTrackCredentialResponse
        {
            public string Username { get; set; }
            public string OrgKey { get; set; }
        }

        public class Handler : IRequestHandler<ListInfoTrackCredentialQuery, List<ListInfoTrackCredentialResponse>>
        {
            private readonly IInfoTrackCredentialRepository _infoTrackCredentialRepository;
            private readonly WCADbContext _wCADbContext;

            public Handler(
                IInfoTrackCredentialRepository infoTrackCredentialRepository,
                WCADbContext wCADbContext
            )
            {
                _infoTrackCredentialRepository = infoTrackCredentialRepository;
                _wCADbContext = wCADbContext;
            }

            public async Task<List<ListInfoTrackCredentialResponse>> Handle(ListInfoTrackCredentialQuery message, CancellationToken cancellationToken)
            {
                var result = new List<ListInfoTrackCredentialResponse>();
                var actionstepOrgsList = _wCADbContext.ActionstepCredentials
                            .Include(a => a.ActionstepOrg)
                            .AsNoTracking()
                            .Where(ac => ac.Owner == message.AuthenticatedUser);

                foreach (var org in actionstepOrgsList)
                {
                    var secret = await _infoTrackCredentialRepository.FindCredential(org.ActionstepOrg.Key);
                    if (secret != null)
                    {
                        result.Add(new ListInfoTrackCredentialResponse
                        {
                            Username = secret.Username,
                            OrgKey = secret.ActionstepOrgKey
                        });
                    }                    
                }

                return result;
            }
        }
    }
}
