using System;

namespace WCA.Domain.Actionstep
{
    public class InvalidActionstepMatterAggregateIdException : WCAException
    {
        private InvalidActionstepMatterAggregateIdException()
        {
        }
        public InvalidActionstepMatterAggregateIdException(string aggregateId)
            : base($"Unable to parse Actionstep Matter aggregate ID: '{aggregateId}'. See inner exception for more details")
        { }

        public InvalidActionstepMatterAggregateIdException(string aggregateId, Exception innerException)
            : base($"Unable to parse Actionstep Matter aggregate ID: '{aggregateId}'. See inner exception for more details", innerException)
        { }
    }
}
