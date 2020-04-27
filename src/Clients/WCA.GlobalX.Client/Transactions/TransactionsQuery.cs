using FluentValidation;
using NodaTime;

namespace WCA.GlobalX.Client.Transactions
{
    public class TransactionsQuery : IQuery
    {
        /// <summary>
        /// The transaction datetime start range.
        /// </summary>
        public OffsetDateTime? PeriodStart { get; set; }

        /// <summary>
        /// The transaction datetime end of range.
        /// </summary>
        public OffsetDateTime? PeriodEnd { get; set; }

        /// <summary>
        /// The transaction ID start range.
        /// </summary>
        public int? TransId { get; set; }

        /// <summary>
        /// The matter reference of the transaction.
        /// </summary>
        public string MatterReference { get; set; }

        /// <summary>
        /// Indicates what transactions should be included i.e. if user is master or parent include all child billing, or only own transactions.
        /// Available values : justMe, allChildren
        /// Default value : justMe
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// The UserId whose authentication to use
        /// </summary>
        public string UserId { get; set; }

        public class Validator : AbstractValidator<TransactionsQuery>
        {
            public Validator()
            {
                RuleFor(q => q.UserId).NotEmpty();
            }
        }
    }

    public enum UserType
    {
        JustMe,
        AllChildren
    }
}
