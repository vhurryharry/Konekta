using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using WCA.Web.Extensions;
using WCA.Web.FeatureFlags;

namespace WCA.Web.Areas.API
{
    [Route("api/feature-flags")]
    public class FeatureFlagsController : Controller
    {
        private readonly IFeatureManager _featureManager;

        public FeatureFlagsController(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        [HttpGet]
        [Route("{actionstepOrg?}")]
        [ProducesDefaultResponseType(typeof(IEnumerable<FeatureFlag>))]
        [ProducesErrorResponseType(typeof(UnauthorizedResult))]
        public IEnumerable<FeatureFlag> EnabledFeatures(string actionstepOrg)
        {
            // Required by org key feature flag filter
            HttpContext.SetCurrentOrgKey(actionstepOrg);

            return Enum.GetValues(typeof(FeatureFlag))
                .OfType<FeatureFlag>()
                .Where(f => _featureManager.IsEnabled(f.ToString()));
        }
    }
}