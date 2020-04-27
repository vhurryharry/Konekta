using FluentValidation;
using Newtonsoft.Json;
using NodaTime;
using System.Collections.Generic;
using WCA.GlobalX.Client.Serialisation;

namespace WCA.GlobalX.Client.Documents
{
    public class DocumentsRequest
    {
        /// <summary>
        /// The UserId whose authentication to use
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The order number that the documents belong to
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// Retrieves order with the specified Order ID prefix
        /// </summary>
        public string OrderIdPrefix { get; set; }

        /// <summary>
        /// The matter reference that the documents belong to
        /// </summary>
        public string MatterReference { get; set; }

        /// <summary>
        /// The product identifier that the documents belong to
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// The name of the documents (partial-matching and case-insensitive)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The start of the date that the documents were created
        /// </summary>
        [JsonConverter(typeof(OffsetDateTimeConverter))]
        public OffsetDateTime? Before { get; set; }

        /// <summary>
        /// The end of the date that the documents were created
        /// </summary>
        [JsonConverter(typeof(OffsetDateTimeConverter))]
        public OffsetDateTime? After { get; set; }

        /// <summary>
        /// The list of status of the documents to be retrieved.
        /// </summary>
        public List<DocumentStatus> Statuses { get; } = new List<DocumentStatus>();

        public class Validator : AbstractValidator<DocumentsRequest>
        {
            public Validator()
            {
                RuleFor(q => q.UserId).NotEmpty();

                RuleFor(q => q.After)
                    .Must((q, after) => after.Value.ToInstant() > q.Before.Value.ToInstant())
                    .When(q => q.After.HasValue && q.Before.HasValue)
                    .WithMessage($"{nameof(After)} must be after {nameof(Before)}");
            }
        }
    }
}