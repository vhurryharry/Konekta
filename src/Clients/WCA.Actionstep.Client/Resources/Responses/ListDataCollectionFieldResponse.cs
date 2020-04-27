using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public class ListDataCollectionFieldResponse : IActionstepResponse
    {
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<DataCollectionField>  DataCollectionFields { get; set; }

        public Linked Linked { get; set; }
    }
}
