using System;
using System.Collections.Generic;
using System.Text;

namespace WCA.Actionstep.Client.Resources
{
    public class ActionParticipant
    {
        public string Id { get; set; }
        public int ParticipantNumber { get; set; }
        public Link Links { get; set; } = new Link();

        public class Link
        {
            public string Action { get; set; }
            public string ParticipantType { get; set; }
            public string Participant { get; set; }
        }
    }
}
