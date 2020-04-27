using System;
using System.Collections.Generic;
using WCA.Core.Helpers;
using WCA.Domain.Conveyancing;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;

namespace WCA.Core.Features.Conveyancing.PolicyRequest
{
    public class FirstTitlePolicyRequestFromActionstepQuery : IAuthenticatedQuery<FirstTitlePolicyRequestFromActionstepResponse>
    {
        public WCAUser AuthenticatedUser { get; set; }
        public int MatterId { get; set; }
        public string OrgKey { get; set; }
    }

    public class FirstTitlePolicyRequestFromActionstepResponse
    {
        public FTActionstepMatter ActionstepData { get; set; }
        public RequestPolicyOptions RequestPolicyOptions { get; set; }
    }

    public class FTActionstepMatter
    {
        public FTTitle Title { get; set; }
        public FTParty SourceProperty { get; set; }
        public List<FTParty> Buyers { get; } = new List<FTParty>();
        public decimal? PurchasePrice { get; set; }
        public DateTime SettlementDate { get; set; }
        public PropertyTitleType? PropertyTitleType { get; set; }
    }

    public class FTTitle
    {
        public TitleInfoType TitleInfoType { get; set; }
        public string TitleReference { get; set; }
        public string TitleVolume { get; set; }
        public string TitleFolio { get; set; }
        public string LegalDescription { get; set; }
    }

    public enum TitleInfoType
    {
        Reference,
        VolumeFolio
    }

    public enum PropertyTitleType
    {
        Strata,
        Community
    }

    public class FTParty
    {
        public string Name { get; set; }
        public IdentityType IdentityType { get; set; }

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string PreferredName { get; set; }

        public string CompanyName { get; set; }
        public string ABN { get; set; }
        public string ACN { get; set; }
        public string ABRN { get; set; }

        public string EmailAddress { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string StateProvince { get; set; }

        public string StreetNo { get; set; }
        public string StreetType { get; set; }
        public string StreetName { get; set; }

        public string BuildingName { get; set; }
        public string FloorNo { get; set; }
        public string UnitNo { get; set; }

        public Party ToASParty()
        {
            return new Party()
            {
                Name = Name,
                IdentityType = IdentityType,
                Title = Title,
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
                Suffix = Suffix,
                PreferredName = PreferredName,
                CompanyName = CompanyName,
                EmailAddress = EmailAddress,
                AddressLine1 = AddressLine1,
                AddressLine2 = AddressLine2,
                City = City,
                Country = Country,
                PostCode = PostCode,
                StateProvince = StateProvince
            };
        }

        public static FTParty FromASParty(Party party)
        {
            var (streetNo, streetSuffix, roadTypeCode, streetName) = AddressHelper.ParseRoadInfo(party.AddressLine1, party.AddressLine2);
            var (state, addressLine) = AddressHelper.ParseState(party.AddressLine1, party.AddressLine2);

            return new FTParty()
            {
                Name = party.Name,
                IdentityType = party.IdentityType,

                Title = party.Title,
                FirstName = party.FirstName,
                MiddleName = party.MiddleName,
                LastName = party.LastName,
                Suffix = party.Suffix,
                PreferredName = party.PreferredName,

                CompanyName = party.CompanyName,
                EmailAddress = party.EmailAddress,

                AddressLine1 = party.AddressLine1,
                AddressLine2 = party.AddressLine2,
                City = party.City,
                Country = party.Country,
                PostCode = party.PostCode,
                StateProvince = string.IsNullOrEmpty(party.StateProvince) ? state : party.StateProvince,

                StreetNo = streetNo,
                StreetType = roadTypeCode,
                StreetName = streetName
            };
        }
    }

    public class RiskInformation
    {
        public bool? UnapprovedWorks { get; set; } = null;
        public bool? EncroachingStructures { get; set; } = null;
        public bool? StructureOverSewerageAndDrainage { get; set; } = null;
        public bool? SewerAndDrainageConnectionWithoutAnEasement { get; set; } = null;
        public bool? IncompleteOrInaccurateSewerAndDrainageDiagram { get; set; } = null;
        public string KnownRisk { get; set; } = "";
    }

    public class RequestPolicyOptions
    {
        public bool RequestPolicyDocument { get; set; } = false;
        public bool RequestKnownRiskPolicies { get; set; } = false;
        public bool RequestPoliciesForVacantLand { get; set; } = false;
        public RiskInformation? RiskInformation { get; set; }
    }

}
