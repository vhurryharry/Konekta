using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System;
using System.Linq;
using System.Security.Claims;

namespace WCA.Web.FeatureFlags
{
    [FilterAlias(UserFilterKey)]
    public class UserFilter : IFeatureFilter
    {
        private const string UserFilterKey = "User";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Evaluate(FeatureFilterEvaluationContext context)
        {
            if (context is null)
            {
                return false;
            }

            var nameIdentifiers = _httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.Name);
            if (nameIdentifiers is null)
            {
                return false;
            }

            var singleEmail = context.Parameters.GetValue<string>(nameof(UserFilterParameters.UserEmails));
            if (!string.IsNullOrEmpty(singleEmail))
            {
                if (nameIdentifiers.Any(n => singleEmail.Equals(n.Value, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            var enabledEmails = context.Parameters.Get<UserFilterParameters>();
            if (enabledEmails is null || enabledEmails.UserEmails is null)
            {
                return false;
            }

            if (enabledEmails.UserEmails.Length < 1)
            {
                return false;
            }

 
            if (nameIdentifiers.Any(n => enabledEmails.UserEmails.Any(u => u.Equals(n.Value, StringComparison.OrdinalIgnoreCase))))
            {
                return true;
            }

            return false;
        }
    }
}
