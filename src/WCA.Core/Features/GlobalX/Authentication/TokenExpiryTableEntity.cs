using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace WCA.Core.Features.GlobalX.Authentication
{
    public class TokenExpiryTableEntity : TableEntity
    {
        public new static string RowKey { get; } = "GlobalXApiTokenExpiryInfo";
        public string UserId { get => PartitionKey; }
        public DateTimeOffset RefreshTokenExpiryUtc { get; set; }
        public DateTimeOffset AccessTokenExpiryUtc { get; set; }
        public DateTimeOffset? RevokedAtUtc { get; set; }

        public TokenExpiryTableEntity()
        { }

        public TokenExpiryTableEntity(DateTime refreshTokenExpiryUtc, DateTime accessTokenExpiryUtc, string userId, DateTimeOffset? revokedAtUtc)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("Must be supplied", nameof(userId));

            PartitionKey = userId;
            base.RowKey = RowKey;
            RefreshTokenExpiryUtc = refreshTokenExpiryUtc;
            AccessTokenExpiryUtc = accessTokenExpiryUtc;
            RevokedAtUtc = revokedAtUtc;
        }
    }
}
