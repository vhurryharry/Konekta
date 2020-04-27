using System;
using System.Collections.Generic;
using System.Text;

namespace WCA.Domain.Abstractions
{
    public interface IMatch
    {
        string RealPropertyIdentifier { get; set; }
        string MatchContext { get; set; }
    }
}
