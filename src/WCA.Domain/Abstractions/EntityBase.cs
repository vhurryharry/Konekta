using System;
using WCA.Domain.Models.Account;

namespace WCA.Domain.Abstractions
{
    public abstract class EntityBase : ITrackedEntity
    {
        public DateTime DateCreatedUtc { get; set; }
        public WCAUser CreatedBy { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public WCAUser UpdatedBy { get; set; }
    }
}
