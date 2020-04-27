using System;
using System.Collections.Generic;
using System.Text;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public class ListActionstepUsersResponse : ActionstepResponseBase<ActionstepUser>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public List<ActionstepUser> Users { get; set; } = new List<ActionstepUser>();
    }
}
