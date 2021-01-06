using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WCA.Web.Areas.API
{
    [Route("api/[controller]/[action]")]
    public class StripeController : Controller
    {
        private AppSettings appSettings;

        public StripeController(IOptions<AppSettings> appSettings)
        {
            if (appSettings is null)
            {
                throw new System.ArgumentNullException(nameof(appSettings));
            }

            this.appSettings = appSettings.Value;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PublicKey()
        {
            return new ObjectResult(new
            {
                publicKey = appSettings.WCACoreSettings.StripeSettings.ApiPublicKey
            });
        }
    }
}