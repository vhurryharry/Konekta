using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System;
using System.Linq;
using WCA.Web.Extensions;

namespace WCA.Web.FeatureFlags
{
    [FilterAlias(OrgKeyFilterKey)]
    public class OrgKeyFilter : IFeatureFilter
    {
        private const string OrgKeyFilterKey = "OrgKey";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrgKeyFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Evaluate(FeatureFilterEvaluationContext context)
        {
            if (context is null)
            {
                return false;
            }

            var currentOrgKey = _httpContextAccessor.HttpContext.GetCurrentOrgKey();
            if (string.IsNullOrEmpty(currentOrgKey))
            {
                return false;
            }

            var singleEnabledOrgKey = context.Parameters.GetValue<string>(nameof(OrgKeyFilterParameters.OrgKeys));
            if (!string.IsNullOrEmpty(singleEnabledOrgKey))
            {
                if (currentOrgKey.Equals(singleEnabledOrgKey, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            var enabledOrgKeys = context.Parameters.Get<OrgKeyFilterParameters>();
            if (enabledOrgKeys is null)
            {
                return false;
            }

            if (enabledOrgKeys.OrgKeys.Length < 1)
            {
                return false;
            }


            if (enabledOrgKeys.OrgKeys.Any(o => o.Equals(currentOrgKey, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return false;
        }
    }
}
