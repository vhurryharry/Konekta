using System;
using System.Collections.Generic;
using System.Linq;
using WCA.Core.Features.Conveyancing.PolicyRequest;
using WCA.Core.Helpers;
using WCA.Domain.Conveyancing;
using WCA.FirstTitle.Client;

namespace WCA.Core.Features.Conveyancing.Services
{
    public class WCAToFirstTitleMapper: IWCAToFirstTitleMapper
    {
        public TitleInsuranceRequest MapToFirstTitleInsuranceRequest(FTActionstepMatter wCAMatter, RequestPolicyOptions options)
        {
            var result = new TitleInsuranceRequest()
            {
                Identifier = new Identifier() { UniqueID = Guid.NewGuid().ToString() },
                Application = new Application()
                {
                    Identifier = new Identifier[] { new Identifier() { UniqueID = Guid.NewGuid().ToString() } }
                },
                TitleInsuranceSegment = new TitleInsuranceSegment()
            };

            var financialSegments = new ValueItem[1];
            financialSegments[0] = new ValueItem()
            {
                Identifier = new Identifier[] { new Identifier() { UniqueID = Guid.NewGuid().ToString() } },
                Asset = new Asset()
                {
                    RealEstate = new RealEstate()
                    {
                        Location = new Location()
                    }
                }
            };

            if (wCAMatter.Title.TitleInfoType == TitleInfoType.Reference) {
                financialSegments[0].Asset.RealEstate.Location.Title = new Title()
                {
                    TorrensTitleRef = wCAMatter.Title.TitleReference,
                    LegalDescription = wCAMatter.Title.LegalDescription,
                    TitleType = TitleTitleType.Torrens,
                    TenureType = TitleTenureType.Freehold
                };
            } else
            {
                financialSegments[0].Asset.RealEstate.Location.Title = new Title()
                {
                    TorrensVolume = wCAMatter.Title.TitleVolume,
                    TorrensFolio = wCAMatter.Title.TitleFolio,
                    LegalDescription = wCAMatter.Title.LegalDescription,
                    TitleType = TitleTitleType.Torrens,
                    TenureType = TitleTenureType.Freehold
                };
            }

            // PropertyAddress
            // - StreetNo
            // - StreetName
            // - StreetType
            // - City
            // - PostCode
            // - Country
            var sourcePropertyAddress = wCAMatter.SourceProperty;
            if (!(sourcePropertyAddress is null))
            {
                financialSegments[0].Asset.RealEstate.Location.Item = new Address();

                ConvertAddress(sourcePropertyAddress, financialSegments[0].Asset.RealEstate.Location.Item as Address);
            }
            else
            {
                // [EtitleException: Column 'StreetAddress' does not allow nulls.].
                // Todo: Handle this exception
                financialSegments[0].Asset.RealEstate.Location.Item = new Address();
            }

            // Financial Segment
            result.Application.FinancialSegment = financialSegments;

            // owner could be a person, or a company, or there can be multiple owners listed
            // (e.g. a company and a contact person)
            // we currently assume that the order the parties are presented in
            // wCAMatter.Buyers matches the order of the buyers in
            // destination.TitleInsuranceSegment.RequestForInsurance.InsuredParty, and that this matches the order
            // of the entries in destination.RelatedPartySegment.
            // TODO handle matching multiple owners in any order
            // (or stop trying to handle a non-null destination object)
            var sourceBuyers = wCAMatter.Buyers;
            var numBuyers = sourceBuyers.Count;

            result.TitleInsuranceSegment.RequestForInsurance = new RequestForInsurance()
            {
                Identifier = new Identifier[] { new Identifier() { UniqueID = Guid.NewGuid().ToString() } },
                InsuredAmount = new InsuredAmount(),
                EstimatedSettlementDate = new EstimatedSettlementDate()
                {
                    Date = new Date()
                    {
                        Date1 = wCAMatter.SettlementDate
                    }
                }
            };

            var insuredParties = new RelatedEntityRef[numBuyers];
            var relatedPartySegments = new RelatedParty[numBuyers];

            // InsuredAmount
            var sourcePurchasePrice = wCAMatter.PurchasePrice;
            if (sourcePurchasePrice is null)
            {
                // [EtitleException: 10027] Insured Amount MUST be greater than $0.
                result.TitleInsuranceSegment.RequestForInsurance.InsuredAmount.ValueSpecified = true;
                result.TitleInsuranceSegment.RequestForInsurance.InsuredAmount.Value = 1;
            }
            else
            {
                result.TitleInsuranceSegment.RequestForInsurance.InsuredAmount.Value = sourcePurchasePrice.Value;
                result.TitleInsuranceSegment.RequestForInsurance.InsuredAmount.ValueSpecified = true;
            }

            for (int i = 0; i < numBuyers; i++)
            {
                var sourceBuyer = sourceBuyers[i];
                var insuredParty = insuredParties[i] = new RelatedEntityRef();
                var ownerUniqueID = insuredParty.RelatedID;
                bool newOwnerID = String.IsNullOrEmpty(insuredParty.RelatedID);
                var relatedParty = relatedPartySegments[i];
                if (!newOwnerID)
                {
                    if (relatedParty.Identifier?.FirstOrDefault()?.UniqueID != ownerUniqueID)
                    {
                        throw new ArgumentException($"Identifier for InsuredParty entry {i} does not match RelatedParty", nameof(wCAMatter));
                    }
                }
                else
                {
                    if (!(relatedParty is null))
                    {
                        throw new ArgumentException($"RelatedParty {i} already exists but InsuredParty does not", nameof(wCAMatter));
                    }
                    ownerUniqueID = insuredParty.RelatedID = Guid.NewGuid().ToString();
                    relatedParty = relatedPartySegments[i] = new RelatedParty()
                    {
                        Identifier = new Identifier[] { new Identifier() { UniqueID = ownerUniqueID } },
                        RelPartyType = RelatedPartyRelPartyType.Owner
                    };
                }

                switch (sourceBuyer.IdentityType)
                {
                    case IdentityType.Company:
                        {
                            // Owner (company)
                            // - CompanyName
                            // - CompanyNumber (ABN/ABRN/ACN) - not currently in Domain.Conveyancing.Party
                            // - Address
                            // - Email

                            relatedParty.Item = new Company();
                            var relatedPartyCompany = relatedParty.Item as Company;

                            relatedPartyCompany.CompanyName = new CompanyName()
                            {
                                BusinessName = sourceBuyer.CompanyName
                            };

                            //  [EtitleException: 10031] ABN/ACN must be populated for an Australian Company if the Company name is populated
                            relatedPartyCompany.CompanyNumber = new CompanyNumber()
                            {
                                ABN = sourceBuyer.ABN,
                                ACN = sourceBuyer.ACN,
                                ABRN = sourceBuyer.ABRN
                            };

                            var contactDetails = relatedPartyCompany.ContactDetails = new ContactDetails();
                            bool hasEmail = !String.IsNullOrEmpty(sourceBuyer.EmailAddress);
                            contactDetails.Items = new object[hasEmail ? 2 : 1];
                            // address and email could be specified in either order (and there could even be multiple addresses / email addresses)
                            // we should handle that. for now, we assume 1 address, then 0-1 email addresses

                            contactDetails.Items[0] = new AddressDetails();
                            var ownerAddressDetails = contactDetails.Items[0] as AddressDetails;
                            ownerAddressDetails.Item = new Address();

                            var ownerAddress = ownerAddressDetails.Item as Address;
                            ConvertAddress(sourceBuyer, ownerAddress);
                            if (hasEmail)
                            {
                                contactDetails.Items[1] = new Email()
                                {
                                    Type = EmailType.Work,
                                    TypeSpecified = true,
                                    PreferredContactMethod = EmailPreferredContactMethod.Yes,
                                    Value = sourceBuyer.EmailAddress
                                };
                            }
                        }
                        break;
                    case IdentityType.Individual:
                        {
                            // Owner (person)
                            // - Title
                            // - FirstName
                            // - Surname
                            // - Address
                            // - Email
                            relatedParty.Item = new Person();
                            var relatedPartyPerson = relatedParty.Item as Person;
                            var rppName = relatedPartyPerson.PersonName = new PersonName();
                            var personTitle = rppName.NameTitle = new NameTitle();
                            personTitle.ValueSpecified = true;
                            try
                            {
                                personTitle.Value = (NameTitleValue)Enum.Parse(typeof(NameTitleValue), sourceBuyer.Title);
                            }
                            catch (Exception ex) when (ex is ArgumentException || ex is OverflowException)
                            {
                                // [EtitleException: 11033] If participant title type is 'Other', the title must be specified and at least 2 characters long.

                                personTitle.Value = NameTitleValue.Other;
                                personTitle.Description = sourceBuyer.Title ?? "Other Title";
                                rppName.OtherTitle = sourceBuyer.Title ?? "Other Title";
                            }
                            rppName.FirstName = sourceBuyer.FirstName;
                            rppName.Surname = sourceBuyer.LastName;

                            var contactDetails = relatedPartyPerson.ContactDetails = new ContactDetails();
                            
                            bool hasEmail = !String.IsNullOrEmpty(sourceBuyer.EmailAddress);

                            // [EtitleException: 11142] Email provided 'Pexa Last', is not valid.
                            // Seems email address is mandatory for Individual
                            contactDetails.Items = new object[2]; ; // new object[hasEmail ? 2 : 1];

                            // address and email could be specified in either order (and there could even be multiple addresses / email addresses)
                            // we should handle that. for now, we assume 1 address, then 0-1 email addresses

                            contactDetails.Items[0] = new AddressDetails();
                            var ownerAddressDetails = contactDetails.Items[0] as AddressDetails;
                            ownerAddressDetails.Item = new Address();
                            var ownerAddress = ownerAddressDetails.Item as Address;
                            ConvertAddress(sourceBuyer, ownerAddress);

                            contactDetails.Items[1] = new Email()
                            {
                                Type = EmailType.Home,
                                TypeSpecified = true,
                                PreferredContactMethod = EmailPreferredContactMethod.Yes,
                                Value = sourceBuyer.EmailAddress ?? "test@gmail.com"
                            };
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Unhandled IdentityType: {sourceBuyer.IdentityType}");
                }
            }

            result.RelatedPartySegment = relatedPartySegments;
            result.TitleInsuranceSegment.RequestForInsurance.InsuredParty = insuredParties;

            var salesBundle = "";
            if (wCAMatter.SettlementDate < DateTime.Now)
            {
                salesBundle += "Existing";
            }

            if (wCAMatter.PropertyTitleType == PropertyTitleType.Strata)
            {
                salesBundle += "Strata";
            } else
            {
                salesBundle += "Home";
            }
            salesBundle += "OwnersGOLD";

            if(options.RequestPolicyDocument)
            {
                result.TitleInsuranceSegment.RequestForInsurance.RequestSupportingDoc = new RequestSupportingDoc()
                {
                    Identifier = new Identifier()
                    {
                        UniqueID = Guid.NewGuid().ToString()
                    },
                    ReturnDeliveryMethod = new ReturnDeliveryMethod[] { new ReturnDeliveryMethod()
                            {
                                Method = ReturnDeliveryMethodMethod.LixiAttachment
                            }
                        },
                    Required = RequestSupportingDocRequired.Yes,
                    DocType = RequestSupportingDocDocType.PolicyDoc
                };
            }

            if(options.RequestKnownRiskPolicies)
            {
                result.Application.FinancialSegment[0].Asset.RealEstate.Location.Title.HasKnownRisk = HasKnownRisk.Yes;
                // [EtitleException: 10118] Known Risks is a required field if requesting a Home Owners Gold Policy Known Risk.
                result.Application.FinancialSegment[0].Asset.RealEstate.Location.Title.KnownRisk = options.RiskInformation.KnownRisk;

                result.Application.FinancialSegment[0].Asset.RealEstate.Location.Title.HasKnownRiskCoverage = HasKnownRiskCoverage.Yes;

                result.Application.FinancialSegment[0].Asset.RealEstate.Location.Title.UnapprovedWorks = 
                    options.RiskInformation.UnapprovedWorks == true ? UnapprovedWorks.Yes : UnapprovedWorks.No;

                result.Application.FinancialSegment[0].Asset.RealEstate.Location.Title.EncroachingStructures =
                    options.RiskInformation.EncroachingStructures == true ? EncroachingStructures.Yes : EncroachingStructures.No;

                result.Application.FinancialSegment[0].Asset.RealEstate.Location.Title.StructureOverSewerageAndDrainage =
                    options.RiskInformation.StructureOverSewerageAndDrainage == true ? StructureOverSewerageAndDrainage.Yes : StructureOverSewerageAndDrainage.No;

                result.Application.FinancialSegment[0].Asset.RealEstate.Location.Title.SewerAndDrainageConnectionWithoutAnEasement =
                    options.RiskInformation.SewerAndDrainageConnectionWithoutAnEasement == true ? SewerAndDrainageConnectionWithoutAnEasement.Yes : SewerAndDrainageConnectionWithoutAnEasement.No;

                result.Application.FinancialSegment[0].Asset.RealEstate.Location.Title.IncompleteOrInaccurateSewerAndDrainageDiagram =
                    options.RiskInformation.IncompleteOrInaccurateSewerAndDrainageDiagram == true ? IncompleteOrInaccurateSewerAndDrainageDiagram.Yes : IncompleteOrInaccurateSewerAndDrainageDiagram.No;

                salesBundle += "KnownRisk";
            }

            if(options.RequestPoliciesForVacantLand)
            {
                result.Application.FinancialSegment[0].Asset.RealEstate.Item = new Residential()
                {
                    Type = ResidentialType.VacantLand
                };
            }

            result.TitleInsuranceSegment.RequestForInsurance.SalesBundle = salesBundle;

            return result;
        }

        private static void ConvertAddress(FTParty sourceAddress, Address destinationAddress)
        {
            destinationAddress.City = sourceAddress.City ?? "TestCity";
            destinationAddress.Postcode = sourceAddress.PostCode ?? "61100"; // Test PostCode
            var country = destinationAddress.Country = new Country();
            country.Value = string.IsNullOrEmpty(sourceAddress.Country) ? "Australia" : sourceAddress.Country;

            var state = destinationAddress.State = new State()
            {
                NameSpecified = true
            };

            try
            {
                state.Name = (StateName)Enum.Parse(typeof(StateName), sourceAddress.StateProvince);
            }
            catch
            {
                state.Name = StateName.Other;
                state.OtherDescription = "Unknown State";
            }

            // FirstTitle addressd can be one of StreetAddress, POBoxAddress, DXBoxAddress and NonStdAddress)
            // We currently only try to produce a StreetAddress
            // TODO handle other formats
            destinationAddress.Item = new StreetAddress();
            var streetAddress = destinationAddress.Item as StreetAddress;

            // These fields are optional
            streetAddress.BuildingName = sourceAddress.BuildingName;
            streetAddress.FloorNo = sourceAddress.FloorNo;
            streetAddress.UnitNo = sourceAddress.UnitNo;

            streetAddress.StreetNo = sourceAddress.StreetNo;
            var street = streetAddress.Street = new Street();
            street.Value = sourceAddress.StreetName;
            // If sourceRoadTypeCode is not set, or does not map to a valid FirstTitle.Client.StreetType,
            // set street.Type "Road"

            // [EtitleException: 10006] Error whilst validating Security: Address Line 1 must be populated.
            street.TypeSpecified = true;
            try
            {
                street.Type = (StreetType)Enum.Parse(typeof(StreetType), AddressHelper.RoadTypeCodes[sourceAddress.StreetType]);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is OverflowException || ex is KeyNotFoundException)
            {
                // Ignored
                street.Type = StreetType.Road;
            }
        }
    }
}
