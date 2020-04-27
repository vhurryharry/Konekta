using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.CQRS;
using WCA.Domain.GlobalX;

namespace WCA.Core.Features.GlobalX.Settings
{
    public class GlobalXOrgSettingsQuery : IQuery<IEnumerable<GlobalXOrgSettings>>
    {
        /// <summary>
        /// Filter whether transaction sync is enabled for the org.
        /// </summary>
        public bool? TransactionSyncEnabled { get; set; }

        /// <summary>
        /// Filter whether transaction sync is enabled for the org.
        /// </summary>
        public bool? DocumentSyncEnabled { get; set; }

        public class Validator : AbstractValidator<GlobalXOrgSettingsQuery>
        {
            public Validator()
            { }
        }

        public class GlobalXSyncOrgsQueryHandler : IRequestHandler<GlobalXOrgSettingsQuery, IEnumerable<GlobalXOrgSettings>>
        {
            private readonly Validator _validator;
            private readonly WCADbContext _wCADbContext;

            public GlobalXSyncOrgsQueryHandler(
                WCADbContext wCADbContext,
                Validator validator)
            {
                _validator = validator;
                _wCADbContext = wCADbContext;
            }

            public Task<IEnumerable<GlobalXOrgSettings>> Handle(GlobalXOrgSettingsQuery request, CancellationToken cancellationToken)
            {
                if (request is null) throw new ArgumentNullException(nameof(request));
                _validator.ValidateAndThrow(request);

                var query = _wCADbContext.GlobalXOrgSettings
                    .AsNoTracking();

                if (request.TransactionSyncEnabled.HasValue)
                    query = query.Where(g => g.TransactionSyncEnabled == request.TransactionSyncEnabled.Value);

                if (request.DocumentSyncEnabled.HasValue)
                    query = query.Where(g => g.DocumentSyncEnabled == request.DocumentSyncEnabled.Value);

                return Task.FromResult(
                    query
                        .Include(s => s.GlobalXAdmin)
                        .Include(s => s.ActionstepSyncUser)
                        .ToList() // Ensures no conflicts with other WCADbContext operations.
                        .AsEnumerable());
            }
        }

        public class GlobalXSyncOrg
        {
            public string AdminUserId { get; set; }
            public string ActionstepOrgKey { get; set; }
        }
    }
}
