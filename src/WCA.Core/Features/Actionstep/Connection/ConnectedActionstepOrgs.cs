using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Actionstep.Connection
{
    public class ConnectedActionstepOrgs
    {
        public class ConnectedActionstepOrgsQuery : IAuthenticatedQuery<ConnectedActionstepOrgsResponse[]>
        {
            public WCAUser AuthenticatedUser { get; set; }
        }

        public class ConnectedActionstepOrgsResponse
        {
            public string ActionstepOrgTitle { get; set; }
            public string ActionstepOrgKey { get; set; }
            public int ActionstepCredentialId { get; set; }
            public bool IsValid { get; set; }
            public DateTime LastRefreshedUtc { get; set; }
            public DateTime? ExpiredAtUtc { get; set; }
            public DateTime? RevokedAtUtc { get; set; }
        }

        public class Handler : IRequestHandler<ConnectedActionstepOrgsQuery, ConnectedActionstepOrgsResponse[]>
        {
            private readonly WCADbContext _wCADbContext;

            public Handler(WCADbContext wCADbContext)
            {
                _wCADbContext = wCADbContext;
            }

            public Task<ConnectedActionstepOrgsResponse[]> Handle(ConnectedActionstepOrgsQuery message, CancellationToken token)
            {
                var utcNow = DateTime.UtcNow;

                var actionstepOrgsList = _wCADbContext.ActionstepCredentials
                    .AsNoTracking()
                    .Where(ac => ac.Owner == message.AuthenticatedUser)
                    .Select(ac => new ConnectedActionstepOrgsResponse
                    {
                        ActionstepOrgKey = ac.ActionstepOrg.Key,
                        ActionstepOrgTitle = string.IsNullOrEmpty(ac.ActionstepOrg.Title) ? ac.ActionstepOrg.Key : ac.ActionstepOrg.Title,
                        ActionstepCredentialId = ac.Id,
                        LastRefreshedUtc = ac.ReceivedAtUtc,
                        ExpiredAtUtc = ac.RefreshTokenExpiryUtc < utcNow ? ac.RefreshTokenExpiryUtc : (DateTime?)null,
                        RevokedAtUtc = ac.RevokedAtUtc,
                        IsValid = ac.RefreshTokenIsValidAndNotExpired()
                    })
                    .ToArray();

                return Task.FromResult(actionstepOrgsList);
            }
        }
    }
}