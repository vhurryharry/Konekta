using MediatR;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.GlobalX;

namespace WCA.Core.Features.GlobalX.Transactions
{
    public class TransactionDisbursementsCreatedStateHandler : INotificationHandler<TransactionDisbursementsCreated>
    {
        private readonly WCADbContext _wCADbContext;

        public TransactionDisbursementsCreatedStateHandler(
            WCADbContext wCADbContext)
        {
            _wCADbContext = wCADbContext;
        }

        public async Task Handle(TransactionDisbursementsCreated notification, CancellationToken cancellationToken)
        {
            if (notification is null) throw new ArgumentNullException(nameof(notification));

            var currentState = _wCADbContext.GlobalXTransactionStates.Find(notification.TransactionId);

            currentState.MatterId = notification.TransactionDisbursementRelationship.ActionstepMatterId.ToString(CultureInfo.InvariantCulture);
            currentState.GSTFreeDisbursementId = notification.TransactionDisbursementRelationship.GSTFreeDisbursementId;
            currentState.GSTTaxableDisbursementId = notification.TransactionDisbursementRelationship.GSTTaxableDisbursementId;
            currentState.UpdateStatus(TransactionProcessingStatus.ProcessedSuccessfully);

            await _wCADbContext.SaveChangesAsync();
        }
    }
}
