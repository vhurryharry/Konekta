using System;

namespace WCA.Domain.Abstractions
{
    public interface IEntityWithGuid
    {
        Guid Id { get; set; }
    }
}
