using AutoMapper;
using NodaTime;
using System;

namespace WCA.Core.AutoMapper
{
    public class InstantToDateTimeConverter : ITypeConverter<Instant, DateTime>
    {
        public DateTime Convert(Instant source, DateTime destination, ResolutionContext context)
        {
            return source.ToDateTimeUtc();
        }
    }
}
