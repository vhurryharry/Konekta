using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.CQRS;

namespace WCA.Core.Features.GlobalX.Sync
{
    public class SetLatestGlobalXTransactionIdCommand : ICommand
    {
        /// <summary>
        /// The org key to update settings for
        /// </summary>
        public string ActionstepOrgKey { get; }

        public int LatestTransactionId { get; }

        public SetLatestGlobalXTransactionIdCommand(
            string actionstepOrgKey,
            int latestTransactionId)
        {
            ActionstepOrgKey = actionstepOrgKey;
            LatestTransactionId = latestTransactionId;
        }

        public class Validator : AbstractValidator<SetLatestGlobalXTransactionIdCommand>
        {
            public Validator()
            {
                RuleFor(c => c.ActionstepOrgKey).NotEmpty();
                RuleFor(c => c.LatestTransactionId);
            }
        }

        public class SetLatestGlobalXTransactionIdCommandHandler : IRequestHandler<SetLatestGlobalXTransactionIdCommand>
        {
            private readonly Validator _validator;
            private readonly WCADbContext _wCADbContext;

            public SetLatestGlobalXTransactionIdCommandHandler(
                WCADbContext wCADbContext,
                Validator validator)
            {
                _validator = validator;
                _wCADbContext = wCADbContext;
            }

            public async Task<Unit> Handle(SetLatestGlobalXTransactionIdCommand request, CancellationToken cancellationToken)
            {
                if (request is null) throw new ArgumentNullException(nameof(request));
                _validator.ValidateAndThrow(request);

                var settings = await _wCADbContext.GlobalXOrgSettings
                    .SingleAsync(g => g.ActionstepOrgKey == request.ActionstepOrgKey);

                settings.LatestTransactionId = request.LatestTransactionId;

                await _wCADbContext.SaveChangesAsync();

                return new Unit();
            }
        }
    }
}
