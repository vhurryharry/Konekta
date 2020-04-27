using NodaTime;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using WCA.Domain.CQRS;

namespace WCA.Domain.Actionstep
{
    public class ActionstepMatter : AggregateRoot
    {
        private string _orgKey;
        private int _matterId;
        private string _pexaWorkspaceId = null;
        private bool _pexaCreationInProgress = false;

        public override string Id => ConstructId(_orgKey, _matterId);

        public static string ConstructId(string orgKey, int matterId)
        {
            if (string.IsNullOrEmpty(orgKey) || matterId < 1)
                return null;

            return $"{matterId}";
        }

        public static (string orgKey, int matterId) DeconstructId(string id)
        {
            var pattern = new Regex(@"(?<orgKey>.*)_(?<matterId>\d+)");

            try
            {
                var match = pattern.Match(id);
                var orgKey = match.Groups["orgKey"].Value;
                var matterId = int.Parse(match.Groups["matterId"].Value, CultureInfo.InvariantCulture);
                return (orgKey, matterId);
            }
#pragma warning disable CA1031 // Do not catch general exception types - wrapping in application specific exception
            catch (Exception ex)
            {
                throw new InvalidActionstepMatterAggregateIdException(id, ex);
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        public ActionstepMatter()
        { }

        public ActionstepMatter(Instant eventCreatedAt, string orgKey, int matterId, string requestedByUserId)
        {
            ApplyChange(new ActionstepMatterRegistered(eventCreatedAt, orgKey, matterId, requestedByUserId));
        }

        public void Apply(ActionstepMatterRegistered actionstepMatterRegistered)
        {
            _orgKey = actionstepMatterRegistered?.OrgKey;
            _matterId = actionstepMatterRegistered.MatterId;
        }

#pragma warning disable CA1801 // Remove unused parameter: Required for event sourcing
        public void Apply(PexaWorkspaceCreationRequested pexaWorkspaceCreationRequested)
#pragma warning restore CA1801 // Remove unused parameter
        {
            _pexaCreationInProgress = true;
        }

        public void Apply(PexaWorkspaceCreated pexaWorkspaceCreated)
        {
            _pexaCreationInProgress = false;
            _pexaWorkspaceId = pexaWorkspaceCreated?.WorkspaceId;
        }

#pragma warning disable CA1801 // Remove unused parameter: Required for event sourcing
        public void Apply(PexaWorkspaceCreationFailed pexaWorkspaceCreationFailed)
#pragma warning restore CA1801 // Remove unused parameter
        {
            _pexaCreationInProgress = false;
            _pexaWorkspaceId = null;
        }

        public PexaWorkspaceCreationRequested RequestPexaWorkspaceCreation(Instant eventCreatedAt, string requestedbyUserId)
        {
            // Really dodgy for demos. Always created for the given orgkey and matter.
            if (!(_orgKey == "btrcdemo" && _matterId == 30) && !(_orgKey == "trial181078920" && _matterId == 23) && !(_orgKey == "ktademo" && _matterId == 8))
            {
                if (_pexaCreationInProgress)
                {
                    throw new CannotCreatePexaWorkspaceException($"Cannot create PEXA workspace as a workspace creation request is in progress for , matter {_matterId} in Actionstep org '{_orgKey}'.");
                }

                if (!string.IsNullOrEmpty(_pexaWorkspaceId))
                {
                    // throw new CannotCreatePexaWorkspaceException($"Cannot create PEXA workspace as workspace '{_pexaWorkspaceId}' is already associated with matter {_matterId} in Actionstep org '{_orgKey}'.");
                    throw new PexaWorkspaceAlreadyExistsException(_pexaWorkspaceId, $"Cannot create PEXA workspace as workspace '{_pexaWorkspaceId}' is already associated with matter {_matterId} in Actionstep org '{_orgKey}'.");
                }
            }

            var requestedEvent = new PexaWorkspaceCreationRequested(eventCreatedAt, requestedbyUserId);
            ApplyChange(requestedEvent);
            return requestedEvent;
        }

        public void MarkPexaWorkspaceCreated(Instant eventCreatedAt, string workspaceId, PexaWorkspaceCreationRequested pexaWorkspaceCreationRequestInfo)
        {
            if (pexaWorkspaceCreationRequestInfo == null)
            {
                throw new ArgumentNullException(nameof(pexaWorkspaceCreationRequestInfo));
            }

            ApplyChange(new PexaWorkspaceCreated(eventCreatedAt, workspaceId, pexaWorkspaceCreationRequestInfo.CorrelationId, pexaWorkspaceCreationRequestInfo.EventId));
        }

        public void MarkPexaWorkspaceCreationFailed(Instant eventCreatedAt, string message, PexaWorkspaceCreationRequested pexaWorkspaceCreationRequestInfo)
        {
            if (pexaWorkspaceCreationRequestInfo == null)
            {
                throw new ArgumentNullException(nameof(pexaWorkspaceCreationRequestInfo));
            }

            ApplyChange(new PexaWorkspaceCreationFailed(eventCreatedAt, message, pexaWorkspaceCreationRequestInfo.CorrelationId, pexaWorkspaceCreationRequestInfo.EventId));
        }
    }
}
