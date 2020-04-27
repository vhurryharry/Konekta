using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public class UploadFileResponse : IActionstepResponse
    {
        [JsonProperty(PropertyName = "files")]
        public ActionstepFilesResponse File { get; set; }
    }

    public class ActionstepFilesResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
    }
}
