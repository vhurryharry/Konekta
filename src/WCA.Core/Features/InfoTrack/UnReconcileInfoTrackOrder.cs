using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.InfoTrack
{
    public class UnReconcileInfoTrackOrder
    {
        public class UnReconcileInfoTrackOrderCommand : ICommand
        {
            public WCAUser AuthenticatedUser { get; set; }
            public int InfoTrackOrderId { get; set; }
        }

        public class Validator : AbstractValidator<UnReconcileInfoTrackOrderCommand>
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
                RuleFor(c => c.InfoTrackOrderId).GreaterThan(0);
            }
        }

        public class Handler : AsyncRequestHandler<UnReconcileInfoTrackOrderCommand>
        {
            private readonly Validator validator;
            private readonly WCADbContext wCADbContext;
            private readonly IMediator mediator;

            public Handler(
                Validator validator,
                WCADbContext wCADbContext,
                IMediator mediator)
            {
                this.validator = validator;
                this.wCADbContext = wCADbContext;
                this.mediator = mediator;
            }

            protected override async Task Handle(UnReconcileInfoTrackOrderCommand command, CancellationToken cancellationToken)
            {
                ValidationResult result = validator.Validate(command);
                if (!result.IsValid)
                {
                    throw new ValidationException("Invalid input.", result.Errors);
                }

                // Only unreconcile orders where the the authenticated user has a valid refresh token
                // for the org that this order relates to. This implies that they have valid
                // credentials / permissions for that org.
                var orderToReconcile = wCADbContext.InfoTrackOrders
                    .Where(order => order.InfoTrackOrderId == command.InfoTrackOrderId)
                    .Where(order => wCADbContext.ActionstepCredentials
                            .Where(actionstepCredential =>
                                actionstepCredential.RefreshToken != null &&
                                actionstepCredential.RefreshToken.Length > 0 &&
                                actionstepCredential.RefreshTokenExpiryUtc > DateTime.UtcNow)
                            .Any(actionstepCredential =>
                                actionstepCredential.Owner == command.AuthenticatedUser &&
                                actionstepCredential.ActionstepOrg == order.ActionstepOrg))
                    .Single();

                orderToReconcile.Reconciled = false;
                orderToReconcile.LastUpdatedUtc = DateTime.UtcNow;
                orderToReconcile.UpdatedBy = command.AuthenticatedUser;
                await wCADbContext.SaveChangesAsync();
            }
        }
    }
}
