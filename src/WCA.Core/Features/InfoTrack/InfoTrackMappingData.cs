using Newtonsoft.Json;
using System.Collections.Generic;

namespace WCA.Core.Features.InfoTrack
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class InfoTrackMappingData
    {
        public string ClientReference { get; set; }
        public string RetailerReference { get; set; }
        public Lawyerdetail LawyerDetail { get; set; } = new Lawyerdetail();
        public List<PropertyDetail> PropertyDetails { get; } = new List<PropertyDetail>();
        public List<Individual> Individuals { get; } = new List<Individual>();
        public List<Organisation> Organisations { get; } = new List<Organisation>();
        public string MatterType { get; set; }
        public string EntryPoint { get; set; }
        public string State { get; set; }
        public List<Courtdetail> CourtDetails { get; } = new List<Courtdetail>();

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Lawyerdetail
        {
            public Organisation Organisation { get; set; } = new Organisation();
            public List<Contactdetail> ContactDetails { get; } = new List<Contactdetail>();
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Organisation
        {
            public string Name { get; set; }
            public string AcnOrAbn { get; set; }
            public string Acn { get; set; }
            public string Abn { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Contactdetail
        {
            public Individual Individual { get; set; } = new Individual();
            public Address Address { get; set; } = new Address();
            public Poboxaddress PoBoxAddress { get; set; } = new Poboxaddress();
            public Dxaddress DxAddress { get; set; } = new Dxaddress();
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Fax { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Individual
        {
            public string Title { get; set; }
            public string GivenName { get; set; }
            public string GivenName2 { get; set; }
            public string Surname { get; set; }
            public string Gender { get; set; }
            public string DateOfBirth { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Address
        {
            public string BuildingName { get; set; }
            public string LotNumber { get; set; }
            public string UnitFlatShopNumber { get; set; }
            public string StreetNumber { get; set; }
            public string StreetNumberTo { get; set; }
            public string StreetName { get; set; }
            public string StreetType { get; set; }
            public string Suburb { get; set; }
            public string State { get; set; }
            public string PostCode { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Poboxaddress
        {
            public string PoBoxType { get; set; }
            public string Number { get; set; }
            public string Suburb { get; set; }
            public string State { get; set; }
            public string PostCode { get; set; }
            public string Instructions { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Dxaddress
        {
            public string Number { get; set; }
            public string Exchange { get; set; }
            public string Suburb { get; set; }
            public string State { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class PropertyDetail
        {
            public Address PropertyAddress { get; set; } = new Address();
            public string Locality { get; set; }
            public string Council { get; set; }
            public string County { get; set; }
            public string Parish { get; set; }
            public string NatureOfProperty { get; set; }
            public string DocsPropertyReferences { get; set; }
            public List<PropertyReference> PropertyReferences { get; } = new List<PropertyReference>();
            public List<Lotplan> LotPlans { get; } = new List<Lotplan>();
            public List<Volumefolio> VolumeFolios { get; } = new List<Volumefolio>();
            public string CouncilPropertyNumber { get; set; }
            public string Spi { get; set; }
            public Crownallotment CrownAllotment { get; set; } = new Crownallotment();
            public List<Proprietor> Proprietors { get; } = new List<Proprietor>();
            public List<Vendor> Vendors { get; } = new List<Vendor>();
            public List<Purchaser> Purchasers { get; } = new List<Purchaser>();
            public string SettlementDate { get; set; }
            public string SettlementTime { get; set; }
            public string AreaUnit { get; set; }
            public string Area { get; set; }
            public string Reason { get; set; }
            public string PurchasePrice { get; set; }
            public List<Mapreference> MapReferences { get; } = new List<Mapreference>();
            public List<Agentdetail> AgentDetails { get; } = new List<Agentdetail>();
            public List<Purchaserssolicitor> PurchasersSolicitors { get; } = new List<Purchaserssolicitor>();
            public Ownerscorporation OwnersCorporation { get; set; } = new Ownerscorporation();
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Crownallotment
        {
            public string Allotment { get; set; }
            public string Block { get; set; }
            public string Section { get; set; }
            public string Portion { get; set; }
            public string SubDivision { get; set; }
            public string Parish { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Ownerscorporation
        {
            public Organisation Organisation { get; set; } = new Organisation();
            public List<Contactdetail> ContactDetails { get; } = new List<Contactdetail>();
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class PropertyReference
        {
            public string Reference { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Lotplan
        {
            public string Lot { get; set; }
            public string PlanType { get; set; }
            public string PlanNumber { get; set; }
            public string Section { get; set; }
            public string Block { get; set; }
            public string StageNumber { get; set; }
            public string RedevelopmentNumber { get; set; }
            public string TitleReference { get; set; }

            /// <summary>
            /// For better readability
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            private bool ContainsData(string value)
            {
                return !(string.IsNullOrEmpty(value) || value.StartsWith("select", System.StringComparison.OrdinalIgnoreCase));
            }

            public bool ContainsLotData()
            {
                if (ContainsData(Lot)
                    || ContainsData(PlanType)
                    || ContainsData(PlanNumber)
                    || ContainsData(Section)
                    || ContainsData(Block)
                    || ContainsData(StageNumber)
                    || ContainsData(RedevelopmentNumber))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Volumefolio
        {
            public string Volume { get; set; }
            public string Folio { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Proprietor
        {
            public Individual Individual { get; set; } = new Individual();
            public Organisation Organisation { get; set; } = new Organisation();
            public Address Address { get; set; } = new Address();
            public string Phone { get; set; }
            public string Fax { get; set; }
            public string Email { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Vendor
        {
            public Individual Individual { get; set; } = new Individual();
            public Organisation Organisation { get; set; } = new Organisation();
            public Address Address { get; set; } = new Address();
            public string Phone { get; set; }
            public string Fax { get; set; }
            public string Email { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Purchaser
        {
            public Individual Individual { get; set; } = new Individual();
            public Organisation Organisation { get; set; } = new Organisation();
            public Address Address { get; set; } = new Address();
            public string Phone { get; set; }
            public string Fax { get; set; }
            public string Email { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Mapreference
        {
            public string MapType { get; set; }
            public string Page { get; set; }
            public string Grid { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Agentdetail
        {
            public Organisation Organisation { get; set; } = new Organisation();
            public List<Contactdetail> ContactDetails { get; } = new List<Contactdetail>();
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Purchaserssolicitor
        {
            public Organisation Organisation { get; set; } = new Organisation();
            public List<Contactdetail> ContactDetails { get; } = new List<Contactdetail>();
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Courtdetail
        {
            public List<Fillingparty> FillingParties { get; } = new List<Fillingparty>();
            public List<Againstparty> AgainstParties { get; } = new List<Againstparty>();
            public string RegistryCourt { get; set; }
            public string RegistryLocation { get; set; }
            public string LawyerPcn { get; set; }
            public string ClaimAmount { get; set; }
            public string SolicitorFeeIncGST { get; set; }
            public string FilingCosts { get; set; }
            public string Interest { get; set; }
            public string Description { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Fillingparty
        {
            public Individual Individual { get; set; } = new Individual();
            public Organisation Organisation { get; set; } = new Organisation();
            public Address Address { get; set; } = new Address();
            public Poboxaddress PoBoxAddress { get; set; } = new Poboxaddress();
            public string Phone { get; set; }
            public string Fax { get; set; }
            public string Email { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class Againstparty
        {
            public Individual Individual { get; set; } = new Individual();
            public Organisation Organisation { get; set; } = new Organisation();
            public Address Address { get; set; } = new Address();
            public Poboxaddress PoBoxAddress { get; set; } = new Poboxaddress();
            public string Phone { get; set; }
            public string Fax { get; set; }
            public string Email { get; set; }
        }
    }
}