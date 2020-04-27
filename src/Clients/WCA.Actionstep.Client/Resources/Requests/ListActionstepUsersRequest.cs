using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WCA.Actionstep.Client.Resources.Requests
{
    public class ListActionstepUsersRequest : IActionstepRequest
    {
        public HttpMethod HttpMethod => HttpMethod.Get;
        public string RelativeResourcePath
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Filter))
                    return $"rest/users?{Filter}";

                return "rest/users";
            }
        }

        public TokenSetQuery TokenSetQuery { get; set; }

        public object JsonPayload => null;

        public string Filter { get; set; } = null;
    }
}
