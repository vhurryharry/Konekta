using NodaTime;
using System.Collections.Generic;

namespace WCA.Domain.Conveyancing
{
    public class Party
    {
        public string Name { get; set; }
        public IdentityType IdentityType { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string PreferredName { get; set; }
        public string Employer { get; set; }
        public string CompanyName { get; set; }

        public string Occupation { get; set; }
        public LocalDate DateOfBirth { get; set; }
        public bool IsDeceased { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public Gender Gender { get; set; }

        public string EmailAddress { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string StateProvince { get; set; }

        public List<PhoneContact> PhoneContacts { get; } = new List<PhoneContact>();
    }

    public enum IdentityType
    {
        Company,
        Individual
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum MaritalStatus
    {
#pragma warning disable CA1720 // Identifier contains type name
        Single,
#pragma warning restore CA1720 // Identifier contains type name
        Married
    }

    public enum PhoneType
    {
        Business,
        Mobile,
        Home,
        DirectDial,
        HomeFax,
        Assistant,
        Business2,
        Home2,
        Mobile2,
        Other
    }
}
