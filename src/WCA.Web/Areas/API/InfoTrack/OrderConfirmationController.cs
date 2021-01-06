using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WCA.Core.Features.InfoTrack;

namespace WCA.Web.Areas.API.InfoTrack
{
    [Route("api/infotrack")]
    public class OrderConfirmationController : Controller
    {
        private readonly IMediator mediator;
        private readonly CloudStorageAccount cloudStorageAccount;
        private readonly ILogger logger;
        private readonly AppSettings appSettings;

        public OrderConfirmationController(
            IMediator mediator,
            ILoggerFactory loggerFactory,
            IOptions<AppSettings> appSettingsAccessor,
            CloudStorageAccount cloudStorageAccount)
        {
            if (appSettingsAccessor is null)
            {
                throw new ArgumentNullException(nameof(appSettingsAccessor));
            }

            this.mediator = mediator;
            this.cloudStorageAccount = cloudStorageAccount;
            logger = loggerFactory.CreateLogger<OrderConfirmationController>();
            appSettings = appSettingsAccessor.Value;
        }

        /// <summary>
        /// Orders the specified payload j object.
        /// </summary>
        /// <param name="command">
        ///     The payload as a <see cref="JObject"/>. This value isn't used directly, instead its purpose
        ///     is to validate that the body is in fact valid JSON. We'll store the raw string instead, to
        ///     provide more flexibility during processing.
        /// </param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Invalid credentails. Authentication Failed.
        /// or
        /// The authorization header isn't Basic.
        /// </exception>
        [Route("orderconfirmation")]
        [IgnoreAntiforgeryToken]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Order([FromBody]SaveIncomingInfoTrackOrderUpdate.SaveIncomingInfoTrackOrderUpdateCommand command)
        {
            var authHeader = Request.Headers["Authorization"].ToString() ?? string.Empty;
            if (!authHeader.StartsWith("Basic", StringComparison.InvariantCultureIgnoreCase))
            {
                return new ObjectResult(
                    new ErrorViewModel("Only Basic authentication is supported at this endpoint."))
                    { StatusCode = (int)HttpStatusCode.Unauthorized };
            }

            var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
            int seperatorIndex = usernamePassword.IndexOf(':', StringComparison.InvariantCultureIgnoreCase);
            var username = usernamePassword.Substring(0, seperatorIndex);
            var password = usernamePassword.Substring(seperatorIndex + 1);

            if (appSettings.WCACoreSettings.InfoTrackSettings.WCAUserName != username ||
                appSettings.WCACoreSettings.InfoTrackSettings.WCAPassword != password)
            {
                return new ObjectResult(
                    new ErrorViewModel("The user name or password supplied were incorrect."))
                    { StatusCode = (int)HttpStatusCode.Unauthorized };
            }

            await mediator.Send(command);

            return new ObjectResult(new { Result = "Order info successfully saved." });
        }
    }
}
