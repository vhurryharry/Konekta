using System;
using WCA.Domain.Abstractions;
using WCA.Domain.Models.Account;

namespace WCA.Domain.Actionstep
{
    /// <summary>
    /// Substititions allow for one Actionstep user to be substituted for another
    /// when making Actionstep API calls.
    ///
    /// For example if a user leaves, this allows for another user to take over
    /// their files. This is needed because any outstanding tasks will fail unless
    /// another useres credentials are supplied.
    ///
    /// A more specific exapmle is if there are oustanding InfoTrack orders. When
    /// they come back 
    /// </summary>
    public class ActionstepCredentialSubstitution : EntityBase, IEntityWithId
    {
        public int Id { get; set; }
        public WCAUser ForOwner { get; set; }
        public WCAUser SubstituteWithOwner { get; set; }
    }
}