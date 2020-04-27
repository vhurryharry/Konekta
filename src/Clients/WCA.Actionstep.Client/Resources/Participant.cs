using System;
using Newtonsoft.Json;
using NodaTime;
using WCA.Actionstep.Client.Converters;
using WCA.Actionstep.Client.Resources.Responses;

namespace WCA.Actionstep.Client.Resources
{
    public class Participant
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        [JsonConverter(typeof(ActionstepBooleanConverter))]
        public bool IsCompany { get; set; }
        public string CompanyName { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string PreferredName { get; set; }
        public string PhysicalAddressLine1 { get; set; }
        public string PhysicalAddressLine2 { get; set; }
        public string PhysicalCity { get; set; }
        public string PhysicalStateProvince { get; set; }
        public string PhysicalPostCode { get; set; }
        public string MailingAddressLine1 { get; set; }
        public string MailingAddressLine2 { get; set; }
        public string MailingCity { get; set; }
        public string MailingStateProvince { get; set; }
        public string MailingPostCode { get; set; }
        public string Phone1Label { get; set; }
        public int? Phone1Country { get; set; }
        public int? Phone1Area { get; set; }
        public string Phone1Number { get; set; }
        public string Phone1Notes { get; set; }
        public string Phone2Label { get; set; }
        public int? Phone2Country { get; set; }
        public int? Phone2Area { get; set; }
        public string Phone2Number { get; set; }
        public string Phone2Notes { get; set; }
        public string Phone3Label { get; set; }
        public int? Phone3Country { get; set; }
        public int? Phone3Area { get; set; }
        public string Phone3Number { get; set; }
        public string Phone3Notes { get; set; }
        public string Phone4Label { get; set; }
        public int? Phone4Country { get; set; }
        public int? Phone4Area { get; set; }
        public string Phone4Number { get; set; }
        public string Phone4Notes { get; set; }
        public string Fax { get; set; }
        public string Sms { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Occupation { get; set; }
        public string TaxNumber { get; set; }
        public LocalDate? BirthTimestamp { get; set; }
        public LocalDate? DeathTimestamp { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public OffsetDateTime ModifiedTimestamp { get; set; }
        public OffsetDateTime CreatedTimestamp { get; set; }
        public ParticipantLinks Links { get; set; } = new ParticipantLinks();

        public class ParticipantLinks
        {
            public string PhysicalCountry { get; set; }
            public string MailingCountry { get; set; }
            public string Division { get; set; }
        }
    }
}
