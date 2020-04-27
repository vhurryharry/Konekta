using MediatR;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;

namespace WCA.Core.Features.GlobalX.Transactions
{
    public class TransactionMatterIdUpdatedStateHandler : INotificationHandler<TransactionMatterIdUpdated>
    {
        private readonly WCADbContext _wCADbContext;

        public TransactionMatterIdUpdatedStateHandler(
            WCADbContext wCADbContext)
        {
            _wCADbContext = wCADbContext;
        }

        public async Task Handle(TransactionMatterIdUpdated notification, CancellationToken cancellationToken)
        {
            if (notification is null) throw new ArgumentNullException(nameof(notification));

            var currentState = await _wCADbContext.GlobalXTransactionStates.FindAsync(notification.TransactionId);

            currentState.MatterId = notification.NewMatterId.ToString(CultureInfo.InvariantCulture);

            await _wCADbContext.SaveChangesAsync();
        }
    }
}
