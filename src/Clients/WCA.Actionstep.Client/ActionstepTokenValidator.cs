using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace WCA.Actionstep.Client
{
    public class ActionstepTokenValidator : ISecurityTokenValidator
    {
        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get => int.MaxValue; set { return; } }

        public bool CanReadToken(string securityToken)
        {
            return true;
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            validatedToken = null;
            return new ClaimsPrincipal();
        }
    }
}
