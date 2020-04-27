using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using WCA.Domain.Abstractions;

namespace WCA.Domain.Actionstep
{
    public class ActionstepOrg : EntityBase
    {
        public string Key { get; set; }

        [Display(Description ="The friendly name of the organisation as shown in Actionstep")]
        public string Title { get; set; }

        public ICollection<ActionstepCredential> Credentials { get; } = new Collection<ActionstepCredential>();
    }
}