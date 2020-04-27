using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using WCA.Actionstep.Client.Converters;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public class ListDisbursementsResponse : ActionstepResponseBase<Disbursement>
    {
        [JsonConverter(typeof(SingleOrArrayConverter<Disbursement>))]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<Disbursement> Disbursements { get; set; } = new List<Disbursement>();
    }
}
