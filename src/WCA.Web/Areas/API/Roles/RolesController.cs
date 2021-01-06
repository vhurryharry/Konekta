using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WCA.Domain.Models.Account;

namespace WCA.Web.Areas.API.Roles
{
    [Route("api/roles")]
    [Authorize(Roles = "GlobalAdministrator")]
    public class RolesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<WCAUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(
            IMediator mediator,
            UserManager<WCAUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _mediator = mediator;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IEnumerable<IdentityRole>> Get(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return _roleManager.Roles;
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                return new[] { role };
            }
        }

        [HttpGet]
        [Route("roles-for-user")]
        public async Task<IEnumerable<IdentityRole>> GetRolesForUser(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var rolesForUser = new List<IdentityRole>();

            foreach (var roleForUser in await _userManager.GetRolesAsync(user))
            {
                rolesForUser.Add(await _roleManager.FindByNameAsync(roleForUser));
            }

            return rolesForUser;
        }

        [HttpPost]
        [Route("add-role-to-user")]
        public async Task AddRoleToUser([FromBody]AddRoleToUser addRoleToUser)
        {
            if (addRoleToUser == null)
            {
                throw new ArgumentNullException(nameof(addRoleToUser));
            }

            if (string.IsNullOrEmpty(addRoleToUser.Email))
            {
                throw new ArgumentNullException(nameof(addRoleToUser), "Email must be supplied");
            }

            if (string.IsNullOrEmpty(addRoleToUser.RoleName))
            {
                throw new ArgumentNullException(nameof(addRoleToUser), "RoleName must be supplied");
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            await _userManager.AddToRoleAsync(user, addRoleToUser.RoleName);
        }
    }
}