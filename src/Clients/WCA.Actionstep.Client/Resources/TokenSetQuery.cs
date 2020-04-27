using System;

namespace WCA.Actionstep.Client.Resources
{
    public class TokenSetQuery
    {
        public string UserId { get; }
        public string OrgKey { get; }

        public TokenSetQuery(string userId, string orgKey)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("Must be supplied", nameof(userId));
            if (string.IsNullOrEmpty(orgKey)) throw new ArgumentException("Must be supplied", nameof(orgKey));

            UserId = userId;
            OrgKey = orgKey;
        }

        public TokenSetQuery(TokenSet fromTokenSet)
        {
            if (fromTokenSet is null) throw new ArgumentNullException(nameof(fromTokenSet));
            if (string.IsNullOrEmpty(fromTokenSet .UserId)) throw new ArgumentException($"The value for {nameof(TokenSet.UserId)} must be supplied", nameof(fromTokenSet));
            if (string.IsNullOrEmpty(fromTokenSet.OrgKey)) throw new ArgumentException($"The value for {nameof(TokenSet.OrgKey)} must be supplied", nameof(fromTokenSet));

            UserId = fromTokenSet.UserId;
            OrgKey = fromTokenSet.OrgKey;
        }
    }
}