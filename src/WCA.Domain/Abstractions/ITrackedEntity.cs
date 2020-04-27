using System;
using WCA.Domain.Models.Account;

namespace WCA.Domain.Abstractions
{
    public interface ITrackedEntity
    {
        DateTime DateCreatedUtc { get; set; }

        WCAUser CreatedBy { get; set; }

        DateTime LastUpdatedUtc { get; set; }

        WCAUser UpdatedBy { get; set; }
    }
}
