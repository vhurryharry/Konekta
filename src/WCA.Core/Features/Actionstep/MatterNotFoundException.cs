using System;
using WCA.Domain;

namespace WCA.Core.Features.Actionstep
{
    public class MatterNotFoundException : WCAException
    {
        private const string _defaultMessage = "Could not find the specified matter";

        /// <summary>
        /// Gets or sets the actionstep org key that was attempted to be accessed.
        /// </summary>
        /// <value>
        /// The actionstep org key.
        /// </value>
        public string ActionstepOrgKey { get; }

        /// <summary>
        /// Gets the matter that was attempted to be accessed.
        /// </summary>
        /// <value>
        /// The matter that was attempted to be accessed.
        /// </value>
        public int MatterId { get; }

        public MatterNotFoundException()
        {
        }

        public MatterNotFoundException(string message) : base(message)
        {
        }

        public MatterNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MatterNotFoundException(
            string actionstepOrgKey,
            int matterId,
            Exception innerException
            ) : base(_defaultMessage, innerException)
        {
            ActionstepOrgKey = actionstepOrgKey;
            MatterId = matterId;
        }

    }
}
