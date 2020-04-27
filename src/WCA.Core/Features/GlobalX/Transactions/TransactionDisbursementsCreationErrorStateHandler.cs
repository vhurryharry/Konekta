using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.GlobalX;
using static WCA.Core.Features.GlobalX.Transactions.TransactionDisbursementsCreationError;

namespace WCA.Core.Features.GlobalX.Transactions
{
    public class TransactionDisbursementsCreationErrorStateHandler : INotificationHandler<TransactionDisbursementsCreationError>
    {
        private readonly WCADbContext _wCADbContext;

        public TransactionDisbursementsCreationErrorStateHandler(
            WCADbContext wCADbContext)
        {
            _wCADbContext = wCADbContext;
        }

        public async Task Handle(TransactionDisbursementsCreationError notification, CancellationToken cancellationToken)
        {
            if (notification is null) throw new ArgumentNullException(nameof(notification));

            var currentState = _wCADbContext.GlobalXTransactionStates.Find(notification.TransactionId);

            var transactionProcessingStatus = notification.ErrorType switch
            {
                TransactionErrorType.MatterIdNotFoundInActionstep => TransactionProcessingStatus.MatterIdNotFoundInActionstep,
                TransactionErrorType.MatterIdBelowMinimum => TransactionProcessingStatus.MatterIdBelowMinimum,
                TransactionErrorType.MatterIdUnableToParseAsInt => TransactionProcessingStatus.MatterIdUnableToParseAsInt,
                _ => TransactionProcessingStatus.UnknownError,
            };

            currentState.UpdateStatus(transactionProcessingStatus, notification.ErrorMessage);

            await _wCADbContext.SaveChangesAsync();
        }
    }
}
