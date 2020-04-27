using System.Collections.Generic;

namespace WCA.Web.Areas.API.Account
{
    public class AccountModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsLoggedIn { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
