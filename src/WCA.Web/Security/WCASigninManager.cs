using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WCA.Actionstep.AspNetCore.Authentication;
using WCA.Domain.Models.Account;

namespace WCA.Web.Security
{
    public class WCASignInManager : SignInManager<WCAUser>
    {
        private readonly IMediator _mediator;
        private readonly UserManager<WCAUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public WCASignInManager(
            UserManager<WCAUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<WCAUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<WCAUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<WCAUser> confirmation,
            IMediator mediator)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _mediator = mediator;
        }

        internal static async Task SignInWithActionstepJwt(TokenValidatedContext context, ILogger logger)
        {
            var email = context.Principal.FindFirstValue(ClaimTypes.Email);
            var actionstepNameIdentifier = context.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            logger.LogDebug("Actionstep JWT SignIn: Attempting to sign in user with Actionstep JWT. User email: '{Email}'", email);

            var userManager = context.HttpContext.RequestServices.GetService<UserManager<WCAUser>>();
            var signInManager = context.HttpContext.RequestServices.GetService<SignInManager<WCAUser>>();
            var userClaimsPrincipalFactory = context.HttpContext.RequestServices.GetService<IUserClaimsPrincipalFactory<WCAUser>>();
            var user = await userManager.FindByEmailAsync(email);

            // Sign in the user by email if present
            if (user == null)
            {
                logger.LogDebug("Actionstep JWT SignIn: No existing identity user, so we will create one. Email: '{Email}'", email);
                user = new WCAUser { UserName = email, Email = email };
                SetFirstAndLastNameIfMissing(user, context.Principal, logger);

                var createUserResult = await userManager.CreateAsync(user);
                logger.LogDebug(
                    "Actionstep JWT SignIn: User created. Email: '{Email}', FirstName: '{FirstName}', LastName: '{LastName}'.",
                    email,
                    user.FirstName,
                    user.LastName);

                if (!createUserResult.Succeeded)
                {
                    throw new ApplicationException("Could not register new user logging in via Actionstep JWT");
                }

            }
            else
            {
                logger.LogDebug("Actionstep JWT SignIn: Existing user found with email: '{Email}'", email);
                if (SetFirstAndLastNameIfMissing(user, context.Principal, logger))
                {
                    await userManager.UpdateAsync(user);
                }
            }

            /// Associate the JWT identifier with this <see cref="WCAUser"/>
            await userManager.AddLoginAsync(user, new UserLoginInfo( ActionstepJwtDefaults.AuthenticationScheme,actionstepNameIdentifier, ActionstepJwtDefaults.DisplayName));

            // Sign in to persist login, set cookie etc.
            await signInManager.SignInAsync(user, isPersistent: true);

            // Update context so the current request will be "authenticated" with this principal as well.
            var userPrincipal = await userClaimsPrincipalFactory.CreateAsync(user);
            context.Principal.AddIdentity((ClaimsIdentity)userPrincipal.Identity);

            logger.LogDebug("Actionstep JWT SignIn: User signed in. Email: '{Email}'.", email);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claimsPrincipal"></param>
        /// <param name="logger"></param>
        /// <returns><see langword="true"/> if the <see cref="WCAUser"/> was updated, otherwise <see langword="false"/>.</returns>
        internal static bool SetFirstAndLastNameIfMissing(WCAUser user, ClaimsPrincipal claimsPrincipal, ILogger logger)
        {
            if (user is null)
            {
                logger.LogDebug("Actionstep JWT SignIn: User is null, cannot set FirstName/LastName.");
                return false;
            }

            if (claimsPrincipal is null)
            {
                logger.LogDebug("Actionstep JWT SignIn: ClaimsPrincipal is null, cannot retrieve FirstName/LastName. Email: '{Email}'.", user.Email);
                return false;
            }

            var modified = false;
            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.FirstName))
            {
                var fullName = claimsPrincipal.FindFirstValue(ActionstepJwtClaimTypes.Name);
                if (!string.IsNullOrEmpty(fullName))
                {
                    var fullNameSplit = fullName.Split(' ');
                    if (fullNameSplit.Length > 1)
                    {
                        user.FirstName = fullNameSplit[0];
                        user.LastName = fullNameSplit[^1];
                        modified = true;
                    }
                    else if (fullNameSplit.Length > 0)
                    {
                        user.FirstName = fullNameSplit[0];
                        modified = true;
                    }
                }

                logger.LogDebug(
                    "Actionstep JWT SignIn: Set FirstName and LastName from Name in JWT. JWT Name: '{Name}', FirstName: '{FirstName}', LastName: '{LastName}'.",
                    fullName,
                    user.FirstName,
                    user.LastName);

            }

            return modified;
        }
    }
}
