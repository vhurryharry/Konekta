using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WCA.Core.Features.Actionstep;
using WCA.Core.Features.Actionstep.Connection;
using WCA.Core.Features.InfoTrack;
using WCA.Core.Features.Integrations;
using WCA.Domain.Models.Account;
using static WCA.Core.Features.Integrations.IntegrationsQuery;

namespace WCA.Web.Areas.API.Integrations
{
    [Route("api/[controller]")]
    public class IntegrationsController : Controller
    {
        private readonly UserManager<WCAUser> _userManager;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public IntegrationsController(
            UserManager<WCAUser> userManager,
            IMediator mediator,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _mediator = mediator;
            _logger = loggerFactory.CreateLogger<IntegrationsController>();
        }

        [HttpPost]
        [Route("infotrack/connect")]
        public async Task<IActionResult> ConnectToInfoTrack([FromBody] StoreInfoTrackCredentialsForOrg.StoreInfoTrackCredentialsForOrgCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var currentUser = await _userManager.GetUserAsync(User);
            command.AuthenticatedUser = currentUser;

            try
            {
                var credentialIsValid = await _mediator.Send(new CheckInfoTrackCredentials.CheckInfoTrackCredentialsCommand()
                {
                    AuthenticatedUser = currentUser,
                    InfoTrackUsername = command.InfoTrackUsername,
                    InfoTrackPassword = command.InfoTrackPassword,
                });

                if (!credentialIsValid)
                {
                    return BadRequest("The InfoTrack username and password supplied are not valid.");
                }

                await _mediator.Send(command);
                return Ok();
            }
            catch (ValidationException)
            {
                return BadRequest("There was a problem validating the form submitted, please check your input and try again.");
            }
        }

        [HttpGet]
        [Route("infotrack")]
        public async Task<List<ListInfoTrackCredential.ListInfoTrackCredentialResponse>> InfoTrack()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            return await _mediator.Send(new ListInfoTrackCredential.ListInfoTrackCredentialQuery { AuthenticatedUser = currentUser });
        }

        [HttpGet]
        [Route("actionsteporgs")]
        public async Task<ConnectedActionstepOrgs.ConnectedActionstepOrgsResponse[]> ActionstepOrgs()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            return await _mediator.Send(new ConnectedActionstepOrgs.ConnectedActionstepOrgsQuery { AuthenticatedUser = currentUser });
        }

        [HttpPost]
        [Route("actionstep/disconnect")]
        public async Task DisconnectFromActionstepOrg([FromBody]DisconnectActionstepOrgCommand disconnectActionstepOrgCommand)
        {
            if (disconnectActionstepOrgCommand is null)
            {
                throw new ArgumentNullException(nameof(disconnectActionstepOrgCommand));
            }

            var currentUser = await _userManager.GetUserAsync(User);

            await _mediator.Send(new DisconnectFromActionstepOrg.DisconnectFromActionstepOrgCommand
            {
                ActionstepOrgKey = disconnectActionstepOrgCommand.ActionstepOrgKey,
                AuthenticatedUser = currentUser
            });
        }

        [HttpGet]
        [Route("actionstep/credentials")]
        public async Task<List<ActionstepConnectionsForUserQuery.ActionstepConnectionsResponse>> ActionstepCredentials()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var response = await _mediator.Send(new ActionstepConnectionsForUserQuery
            {
                AuthenticatedUser = currentUser
            });

            return response;

        }

        [HttpPost]
        [Route("actionstep/refresh-credentials")]
        public async Task<RefreshActionstepTokenResponseViewModel> ActionstepRefreshCredentials(
            [FromBody]RefreshActionstepCredentials.RefreshActionstepCredentialsCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.AuthenticatedUser = await _userManager.GetUserAsync(User);
            var response = await _mediator.Send(command);

            return new RefreshActionstepTokenResponseViewModel(Instant.FromDateTimeUtc(response.RefreshTokenExpiresAt.ToDateTimeUtc()));
        }

        [HttpGet]
        [Route("integration-links/{actionstepOrg}/{matterId}")]
        public async Task<IEnumerable<Integration>> GetIntegrationLinks(string actionstepOrg, int matterId)
        {
            return await _mediator.Send(new IntegrationsQuery()
            {
                AuthenticatedUser = await _userManager.GetUserAsync(User),
                ActionstepOrg = actionstepOrg,
                MatterId = matterId
            });
        }

        public class DisconnectActionstepOrgCommand
        {
            public string ActionstepOrgKey { get; set; }
        }
    }
}