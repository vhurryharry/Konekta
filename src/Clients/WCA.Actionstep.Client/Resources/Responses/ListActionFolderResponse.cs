using System;
using System.Collections.Generic;
using System.Text;

namespace WCA.Actionstep.Client.Resources.Responses
{
    public class ListActionFolderResponse : ActionstepResponseBase<ActionFolder>
    {
        public List<ActionFolder> ActionFolders { get; } = new List<ActionFolder>();
        public ActionFolder.Link Links { get; set; } = new ActionFolder.Link();
    }
}
