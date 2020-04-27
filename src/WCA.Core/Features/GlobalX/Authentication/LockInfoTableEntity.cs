using Microsoft.WindowsAzure.Storage.Table;
using System;
using WCA.GlobalX.Client.Authentication;

namespace WCA.Core.Features.GlobalX.Authentication
{
    public class LockInfoTableEntity : TableEntity
    {
        public new static string RowKey { get; } = "GlobalXApiTokenLockInfo";

        public Guid LockId { get; set; }

        public DateTime ExpiresAtUtc { get; set; }

        public LockInfoTableEntity()
        { }

        public LockInfoTableEntity(GlobalXApiTokenLockInfo globalXApiTokenLockInfo)
        {
            if (globalXApiTokenLockInfo is null) throw new ArgumentNullException(nameof(globalXApiTokenLockInfo));

            PartitionKey = globalXApiTokenLockInfo.UserId;
            base.RowKey = RowKey;

            LockId = globalXApiTokenLockInfo.LockId;
            ExpiresAtUtc = globalXApiTokenLockInfo.ExpiresAt.ToDateTimeUtc();
        }
    }
}
