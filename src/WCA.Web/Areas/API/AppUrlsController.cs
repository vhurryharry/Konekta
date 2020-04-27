using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WCA.Core;

namespace WCA.Web.Areas.API
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AppUrlsController: Controller
    {
        private readonly AppSettings _appSettings;

        public AppUrlsController(IOptions<AppSettings> appSettingsAccessor)
        {
            if (appSettingsAccessor is null)
            {
                throw new System.ArgumentNullException(nameof(appSettingsAccessor));
            }

            _appSettings = appSettingsAccessor.Value;
        }

        [HttpGet]
        public AppUrlSettings Get()
        {
            return _appSettings.WCACoreSettings.AppUrlSettings;
        }
    }
}
