using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WCA.Core.Features.Conveyancing.PolicyRequest;
using WCA.Domain.Conveyancing;
using WCA.FirstTitle.Client;
using Xunit;

namespace WCA.UnitTests.FirstTitle
{
    //public class GetMappingToFirstTitleRequestTests
    //{
    //    private readonly IMapper _mapper;
    //    public GetMappingToFirstTitleRequestTests()
    //    {
    //        // TODO use DI here to ensure we get the same AutoMapper configuration as WCA.Core
    //        var configuration = new MapperConfiguration(cfg => { cfg.AddProfile(new WCA.Core.AutoMapper.CommandProfile()); });
    //        //configuration.AssertConfigurationIsValid();
    //        _mapper = new Mapper(configuration);
    //    }

    //    [Theory]
    //    [ClassData(typeof(FirstTitleTestRequestCollection))]
    //    void TestRequestMapping(
    //        SendFirstTitlePolicyRequestQuery sendFirstTitlePolicyRequestQuery,
    //        string titleRef,
    //        string streetNo,
    //        string streetName,
    //        StreetType streetType,
    //        string city,
    //        string postCode,
    //        string country,
    //        decimal insuredAmount,
    //        DateTime estimatedSettlementDate,
    //        NameTitleValue ownerTitle,
    //        string ownerFirstName,
    //        string ownerLastName,
    //        string ownerEmail)
    //    {
    //        var mappedResponse = _mapper.Map<TitleInsuranceRequest>(sendFirstTitlePolicyRequestQuery);
    //        Assert.Equal(titleRef, mappedResponse.Application.FinancialSegment.FirstOrDefault()?.Asset.RealEstate.Location.Title.TorrensTitleRef);
    //        var address = mappedResponse.Application.FinancialSegment.FirstOrDefault()?.Asset.RealEstate.Location.Item as Address;
    //        Assert.NotNull(address);
    //        var streetAddress = address.Item as StreetAddress;
    //        Assert.NotNull(streetAddress);
    //        Assert.Equal(streetNo, streetAddress.StreetNo);
    //        Assert.Equal(streetName, streetAddress.Street.Value);
    //        Assert.Equal(streetType, streetAddress.Street.Type);
    //        Assert.Equal(city, address.City);
    //        Assert.Equal(postCode, address.Postcode);
    //        Assert.Equal(country, address.Country.Value);
    //        var rfi = mappedResponse.TitleInsuranceSegment.RequestForInsurance;
    //        Assert.Equal(insuredAmount, rfi.InsuredAmount.Value);
    //        Assert.Equal(estimatedSettlementDate, rfi.EstimatedSettlementDate.Date.Date1);
    //        var ownerUniqueID = rfi.InsuredParty.FirstOrDefault()?.RelatedID;
    //        Assert.NotNull(ownerUniqueID);
    //        var ownerRelatedParty = mappedResponse.RelatedPartySegment.FirstOrDefault(party => ownerUniqueID == party?.Identifier?.FirstOrDefault()?.UniqueID);
    //        Assert.NotNull(ownerRelatedParty);
    //        var ownerPerson = ownerRelatedParty.Item as Person;
    //        Assert.Equal(ownerTitle, ownerPerson.PersonName.NameTitle.Value);
    //        Assert.Equal(ownerFirstName, ownerPerson.PersonName.FirstName);
    //        Assert.Equal(ownerLastName, ownerPerson.PersonName.Surname);
    //        var ownerAddress = (ownerPerson.ContactDetails.Items.FirstOrDefault(item => typeof(AddressDetails) == item.GetType()) as AddressDetails)?.Item as Address;
    //        Assert.NotNull(ownerAddress);
    //        var ownerStreetAddress = ownerAddress.Item as StreetAddress;
    //        Assert.NotNull(ownerStreetAddress);
    //        Assert.Equal(streetNo, ownerStreetAddress.StreetNo);
    //        Assert.Equal(streetName, ownerStreetAddress.Street.Value);
    //        Assert.Equal(streetType, ownerStreetAddress.Street.Type);
    //        Assert.Equal(city, ownerAddress.City);
    //        Assert.Equal(postCode, ownerAddress.Postcode);
    //        Assert.Equal(country, ownerAddress.Country.Value);
    //        var email = (ownerPerson.ContactDetails.Items.FirstOrDefault(item => typeof(Email) == item.GetType()) as Email)?.Value;
    //        Assert.Equal(ownerEmail, email);
    //    }
    //}

    //public class FirstTitleTestRequestCollection : IEnumerable<object[]>
    //{
    //    private readonly List<object[]> _data = new List<object[]>
    //    {
    //        new object[]
    //        {
    //            new SendFirstTitlePolicyRequestQuery()
    //            {
    //                FirstTitlePolicyRequest = new FirstTitlePolicyRequestFromActionstepResponse()
    //                {
    //                    ActionstepData = new ConveyancingMatter()
    //                    {
    //                        PurchasePrice = 1234.56m,
    //                        PropertyDetails = new PropertyDetails()
    //                        {
    //                            TitleReference = "123 / 45678",
    //                        },
    //                        SettlementDate = new NodaTime.LocalDate(2019, 12, 14),
    //                        // PropertyAddresses and Buyers also need to be set, but they are read-only properties
    //                        // so we have to add them in the constructor, below
    //                    }
    //                }
    //            },
    //            "123 / 45678", // TitleRef
    //            "1", // StreetNo
    //            "Fake", // StreetName
    //            StreetType.Street, // StreetType
    //            "Fakesville", // City
    //            "1234", // PostCode
    //            "Straya", // Country
    //            1234.56m, // InsuredAmount
    //            new DateTime(2019, 12, 14), // EsimatedSettlementDate
    //            NameTitleValue.Mrs, // OwnerTitle
    //            "Test", // OwnerFirstName
    //            "Person", // OwnerLastName
    //            "test@example.com", // OwnerEmail
    //        }
    //    };
    //    private readonly Domain.Conveyancing.Party _propertyAddress = new Domain.Conveyancing.Party()
    //    {
    //        AddressLine1 = "1 Fake Street",
    //        City = "Fakesville",
    //        PostCode = "1234",
    //        Country = "Straya"
    //    };
    //    private readonly Domain.Conveyancing.Party _buyer = new Domain.Conveyancing.Party()
    //    {
    //        IdentityType = IdentityType.Individual,
    //        Title = "Mrs",
    //        FirstName = "Test",
    //        LastName = "Person",
    //        AddressLine1 = "1 Fake Street",
    //        City = "Fakesville",
    //        PostCode = "1234",
    //        Country = "Straya",
    //        EmailAddress = "test@example.com"
    //    };

    //    public FirstTitleTestRequestCollection()
    //    {
    //        foreach (var datum in _data)
    //        {
    //            var sendFirstTitlePolicyRequest = datum[0] as SendFirstTitlePolicyRequestQuery;
    //            sendFirstTitlePolicyRequest.FirstTitlePolicyRequest.ActionstepData.PropertyAddresses.Add(_propertyAddress);
    //            sendFirstTitlePolicyRequest.FirstTitlePolicyRequest.ActionstepData.Buyers.Add(_buyer);
    //        }
    //    }
    
    //    public IEnumerator<object[]> GetEnumerator()
    //    {
    //        return _data.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }
    //}
}
