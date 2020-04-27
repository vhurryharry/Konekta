using System;
using System.Diagnostics.CodeAnalysis;

namespace WCA.Domain.Actionstep
{
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Default / seed data")]
    public static class ActionstepDefaults
    {
        public static string AllOrgsKey { get => "AllOrgsKey"; }

        public static ActionstepOrg[] ActionstepOrgs
        {
            get
            {
                return new[]
                {
                    new ActionstepOrg() { Key = AllOrgsKey, Title = "All Orgs", DateCreatedUtc = DateTime.MinValue, LastUpdatedUtc = DateTime.MinValue },
                };
            }
        }
    }
}