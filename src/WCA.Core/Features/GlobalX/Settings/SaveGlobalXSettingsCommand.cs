using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.CQRS;
using WCA.Domain.GlobalX;

namespace WCA.Core.Features.GlobalX.Sync
{
    public class SaveGlobalXSettingsCommand : ICommand
    {
        /// <summary>
        /// The org key to update settings for
        /// </summary>
        public string ActionstepOrgKey { get; set; }

        public string GlobalXAdminId { get; set; }
        public string ActionstepSyncUserId { get; set; }
        public int MinimumMatterIdToSync { get; set; }
        public bool? TransactionSyncEnabled { get; set; }
        public int? TaxCodeIdNoGST { get; set; }
        public int? TaxCodeIdWithGST { get; set; }

        public bool? DocumentSyncEnabled { get; set; }
        public Instant? LastDocumentSync { get; set; }
        public int? LatestTransactionId { get; set; }

        public class Validator : AbstractValidator<SaveGlobalXSettingsCommand>
        {
            public Validator()
            {
                RuleFor(c => c.ActionstepOrgKey).NotEmpty();
                RuleFor(c => c.GlobalXAdminId).NotEmpty();
            }
        }

        public class SetAdminUserCommandHandler : IRequestHandler<SaveGlobalXSettingsCommand>
        {
            private readonly Validator _validator;
            private readonly WCADbContext _wCADbContext;

            public SetAdminUserCommandHandler(
                WCADbContext wCADbContext,
                Validator validator)
            {
                _validator = validator;
                _wCADbContext = wCADbContext;
            }

            public async Task<Unit> Handle(SaveGlobalXSettingsCommand request, CancellationToken cancellationToken)
            {
                if (request is null) throw new ArgumentNullException(nameof(request));
                _validator.ValidateAndThrow(request);

                var settings = await _wCADbContext.GlobalXOrgSettings
                    .SingleOrDefaultAsync(g => g.ActionstepOrgKey == request.ActionstepOrgKey);

                if (settings is null)
                {
                    settings = new GlobalXOrgSettings();
                    settings.ActionstepOrgKey = request.ActionstepOrgKey;
                    _wCADbContext.GlobalXOrgSettings.Add(settings);
                }

                settings.GlobalXAdminId = request.GlobalXAdminId;
                settings.ActionstepSyncUserId = request.ActionstepSyncUserId;
                settings.MinimumMatterIdToSync = request.MinimumMatterIdToSync;

                if (request.TransactionSyncEnabled.HasValue) settings.TransactionSyncEnabled = request.TransactionSyncEnabled.Value;
                if (request.LatestTransactionId.HasValue) settings.LatestTransactionId = request.LatestTransactionId.Value;
                if (request.TaxCodeIdWithGST.HasValue) settings.TaxCodeIdWithGST = request.TaxCodeIdWithGST.Value;
                if (request.TaxCodeIdNoGST.HasValue) settings.TaxCodeIdNoGST = request.TaxCodeIdNoGST.Value;
                if (request.DocumentSyncEnabled.HasValue) settings.DocumentSyncEnabled = request.DocumentSyncEnabled.Value;
                if (request.LastDocumentSync.HasValue) settings.LastDocumentSyncUtc = request.LastDocumentSync.Value.ToDateTimeUtc();

                await _wCADbContext.SaveChangesAsync();

                return new Unit();
            }
        }
    }
}
