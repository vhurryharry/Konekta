using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.GlobalX;

namespace WCA.Core.Features.GlobalX.Transactions
{
    public class TransactionAddedStateHandler : INotificationHandler<TransactionAdded>
    {
        private readonly WCADbContext _wCADbContext;

        public TransactionAddedStateHandler(
            WCADbContext wCADbContext)
        {
            _wCADbContext = wCADbContext;
        }

        public async Task Handle(TransactionAdded notification, CancellationToken cancellationToken)
        {
            if (notification is null) throw new ArgumentNullException(nameof(notification));
            if (notification.Transaction is null) throw new ArgumentException("Transaction must be supplied", nameof(notification));

            var transactionState = new GlobalXTransactionState();
            transactionState.ActionstepOrgKey = notification.ActionstepOrgKey;

            transactionState.TransactionId = notification.Transaction.TransactionId;
            transactionState.OrderId = notification.Transaction.OrderId;
            transactionState.MatterId = notification.Transaction.Matter;
            transactionState.TransactionDateTimeUtc = notification.Transaction.TransactionDateTime.UtcDateTime;
            transactionState.SearchReference = notification.Transaction.SearchReference;
            transactionState.WholesalePrice = notification.Transaction.WholesalePrice;
            transactionState.WholesaleGst = notification.Transaction.WholesaleGst;
            transactionState.RetailPrice = notification.Transaction.RetailPrice;
            transactionState.RetailGst = notification.Transaction.RetailGst;
            transactionState.CreditForTransactionId = notification.Transaction.CreditFor;
            transactionState.ItemNumber = notification.Transaction.ItemNumber;
            transactionState.MatterBasedInvoiced = notification.Transaction.MatterBasedInvoiced;
            transactionState.GlobalXUserId = notification.Transaction.User?.UserId;
            transactionState.GlobalXCustomerRef = notification.Transaction.User?.CustomerRef;
            transactionState.ProductCode = notification.Transaction.Product?.ProductCode;
            transactionState.ProductDescription = notification.Transaction.Product?.ProductDescription;
            transactionState.ProductSubGroup = notification.Transaction.Product?.ProductSubGroup;

            _wCADbContext.GlobalXTransactionStates.Add(transactionState);

            await _wCADbContext.SaveChangesAsync();
        }
    }
}
