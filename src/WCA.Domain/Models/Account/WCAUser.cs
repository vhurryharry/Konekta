using Microsoft.AspNetCore.Identity;

namespace WCA.Domain.Models.Account
{
    public class WCAUser : IdentityUser
    {
        public static string AllUsersId { get => "AllUsersId"; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}