using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using WCA.Actionstep.Client;
using WCA.Web.FeatureFlags;

namespace WCA.Web.Areas.API
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UIDefinitionsController : Controller
    {
        private readonly IActionstepService _actionstepService;
        private IFeatureManager _featureManager;

        public UIDefinitionsController(IActionstepService actionstepService, IFeatureManager featureManager)
        {
            _actionstepService = actionstepService;
            _featureManager = featureManager;
        }

        [HttpGet]
        public UISettings Get()
        {
            // Only user feature flags should be returned because we aren't setting
            // an org key in the context
            var featureFlags = Enum.GetValues(typeof(FeatureFlag))
                .OfType<FeatureFlag>()
                .Where(f => _featureManager.IsEnabled(f.ToString()));

            return new UISettings() {
                BuildNumber = UIDefinitions.BuildNumber,
                YearWcaIncorporated = UIDefinitions.YearWcaIncorporated,
                IsDevEnvironment = UIDefinitions.IsDevEnvironment,
                BackToActionstepURL = _actionstepService.LaunchPadUri.AbsoluteUri,
                UserFeatureFlags = featureFlags
            };
        }
    }

    public class UISettings
    {
        public string BuildNumber { get; set; }
        public int YearWcaIncorporated { get; set; }
        public bool IsDevEnvironment { get; set; }

#pragma warning disable CA1056 // Uri properties should not be strings
        public string BackToActionstepURL { get; set; }
        public IEnumerable<FeatureFlag> UserFeatureFlags { get; internal set; }
#pragma warning restore CA1056 // Uri properties should not be strings
    }
}
