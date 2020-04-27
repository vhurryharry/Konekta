using Newtonsoft.Json;
using NodaTime;
using WCA.Actionstep.Client.Converters;

namespace WCA.Actionstep.Client.Resources
{
    public class Disbursement
    {
        public int? Id { get; set; }

        public LocalDate? Date { get; set; }

        /// <summary>
        /// Required to create a new disbursement
        /// </summary>
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        [JsonConverter(typeof(ActionstepBooleanConverter))]
        public bool UnitPriceIncludesTax { get; set; }

        public string UtbmsExpenseCode { get; set; }

        /// <summary>
        /// Use this field to supply an external transaction ID if available. This is stored
        /// by Actionstep internally and not visible in the UI. Use this to retrieve Actionstep
        /// disbursement IDs when creating multiple disbursements.
        /// </summary>
        public string ImportExternalReference { get; set; }

        public OffsetDateTime? EnteredTimestamp { get; set; }
        public DisbursementLink Links { get; set; } = new DisbursementLink();
    }

    public class DisbursementLink
    {
        /// <summary>
        /// Required. The Action ID (Matter ID) that this disbursement is linked to.
        /// </summary>
        [JsonConverter(typeof(IntStringConverter))]
        public int? Action { get; set; }
        public int? EnteredByParticipant { get; set; }
        public string Uom { get; set; }

        [JsonConverter(typeof(IntStringConverter))]
        public int? TaxCode { get; set; }
        public int? IncomeAccount { get; set; }
    }
}