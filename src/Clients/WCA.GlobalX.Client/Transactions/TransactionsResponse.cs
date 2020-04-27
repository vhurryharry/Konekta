using System.Collections.Generic;

namespace WCA.GlobalX.Client.Transactions
{
    public class TransactionsResponse
    {
        public TransactionsQuery TransactionsSearchCriteria { get; set; }
        public List<Transaction> Transactions { get; } = new List<Transaction>();
    }
}
