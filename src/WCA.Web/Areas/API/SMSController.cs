using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WCA.Core;
using WCA.Core.Services;

namespace WCA.Web.Areas.API
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class SMSController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly ITelemetryLogger _telemetryLogger;

        public SMSController(
            IOptions<AppSettings> appSettingsAccessor,
            ITelemetryLogger telemetryLogger)
        {
            if (appSettingsAccessor is null)
            {
                throw new System.ArgumentNullException(nameof(appSettingsAccessor));
            }

            _appSettings = appSettingsAccessor.Value;
            _telemetryLogger = telemetryLogger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task Get()
        {
            await LogTheRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task Post()
        {
            await LogTheRequest();
        }

        private async Task LogTheRequest()
        {
            var payload = string.Empty;

            using (var stream = new StreamReader(HttpContext.Request.Body))
            {
                payload = await stream.ReadToEndAsync();
            }

            _telemetryLogger.TrackEvent(
                "SMSControllerMessageReceived",
                new Dictionary<string, string>()
                    {
                        { "Payload", payload },
                        { "QueryString", HttpContext.Request.QueryString.ToString() }
                    });
        }
    }
}
