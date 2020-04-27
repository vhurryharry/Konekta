using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public class SaveActionDocumentResponse : ActionstepResponseBase<ActionDocument>
    {
        [JsonProperty(PropertyName = "actiondocuments")]
        public ActionDocument ActionDocument { get; set; }

    }
}
