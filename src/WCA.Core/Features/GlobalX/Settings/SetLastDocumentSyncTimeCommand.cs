using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.CQRS;

namespace WCA.Core.Features.GlobalX.Sync
{
    public class SetLastDocumentSyncTimeCommand : ICommand
    {
        /// <summary>
        /// The org key to update settings for
        /// </summary>
        public string ActionstepOrgKey { get; }

        public Instant LastDocumentSync { get; }

        public SetLastDocumentSyncTimeCommand(
            string actionstepOrgKey,
            Instant lastDocumentSync)
        {
            ActionstepOrgKey = actionstepOrgKey;
            LastDocumentSync = lastDocumentSync;
        }

        public class Validator : AbstractValidator<SetLastDocumentSyncTimeCommand>
        {
            public Validator()
            {
                RuleFor(c => c.ActionstepOrgKey).NotEmpty();
            }
        }

        public class SetLastDocumentSyncTimeCommandHandler : IRequestHandler<SetLastDocumentSyncTimeCommand>
        {
            private readonly Validator _validator;
            private readonly WCADbContext _wCADbContext;

            public SetLastDocumentSyncTimeCommandHandler(
                WCADbContext wCADbContext,
                Validator validator)
            {
                _validator = validator;
                _wCADbContext = wCADbContext;
            }

            public async Task<Unit> Handle(SetLastDocumentSyncTimeCommand request, CancellationToken cancellationToken)
            {
                if (request is null) throw new ArgumentNullException(nameof(request));
                _validator.ValidateAndThrow(request);

                var settings = await _wCADbContext.GlobalXOrgSettings
                    .SingleAsync(g => g.ActionstepOrgKey == request.ActionstepOrgKey);

                settings.LastDocumentSyncUtc = request.LastDocumentSync.ToDateTimeUtc();

                await _wCADbContext.SaveChangesAsync();

                return new Unit();
            }
        }
    }
}
