using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using WCA.Core;
using WCA.Core.Features.InfoTrack;
using WCA.Domain.Models.Account;
using WCA.UnitTests.TestInfrastructure;
using Xunit;

namespace WCA.UnitTests.InfoTrack
{
    [Collection(WebContainerCollection.WebContainerCollectionName)]
    public class GetMappingDataFromActionstepTests
    {
        private readonly WebContainerFixture _containerFixture;

        public GetMappingDataFromActionstepTests(WebContainerFixture containerFixture)
        {
            _containerFixture = containerFixture;
        }

        [Fact]
        public async void DetailsMappedCorrectly()
        {
            // Arrange
            WCAUser testUser = await _containerFixture.GetTestUser();

            var query = new GetMappingDataFromActionstep.GetMappingDataFromActionstepQuery()
            {
                ActionstepOrgKey = "wcamaster",
                AuthenticatedUser = testUser,
                MatterId = 7
            };

            var testActionstepService = new TestActionstepService();

            testActionstepService.AddSampleResponse(
                $"/rest/actionparticipants?action={query.MatterId}&include=participantType,participant",
                HttpStatusCode.OK,
                HttpMethod.Get,
                "InfoTrack.GetMappingDataFromActionstepData.Response-1-ActionParticipants.json");

            testActionstepService.AddSampleResponse(
                "/rest/datacollectionrecordvalues" +
                    $"?action={query.MatterId}" +
                    $"&dataCollectionRecord[dataCollection][name_in]=property,convdet,keydates" +
                    $"&dataCollectionField[name_in]=titleref,lotno,planno,plantype,lotno2,planno2,plantype2,smtdateonly,smttime,purprice" +
                    $"&include=dataCollectionField,dataCollection",
                HttpStatusCode.OK,
                HttpMethod.Get,
                "InfoTrack.GetMappingDataFromActionstepData.Response-2-PropertyInfo.json");

            SendMappingsToInfoTrack.SendMappingsToInfoTrackCommand resultUnderTest = null;

            await _containerFixture.ExecuteScopeAsync(async sp =>
            {
                var mediator = sp.GetService<IMediator>();
                var options = sp.GetService<IOptions<WCACoreSettings>>();

                var handlerUnderTest = new GetMappingDataFromActionstep.Handler(
                    new GetMappingDataFromActionstep.Validator(),
                    testActionstepService,
                    options,
                    mediator,
                    new TestInfoTrackCredentialRepository("wcamaster", "dummyUsername", "dummyPassword"),
                    new TestTelemetryLogger());

                // Act
                resultUnderTest = await handlerUnderTest.Handle(query, new CancellationToken());
            });

            // Assert
            // These values must match the corresponding test data in the JSON files referenced above.
            Assert.NotNull(resultUnderTest);
            Assert.Equal(query.MatterId.ToString(CultureInfo.InvariantCulture), resultUnderTest.InfoTrackMappingData.ClientReference);
            Assert.Equal(testUser, resultUnderTest.AuthenticatedUser);
            Assert.Equal($"WCA_{query.ActionstepOrgKey}|{query.AuthenticatedUser.Id}", resultUnderTest.InfoTrackMappingData.RetailerReference);

            // We currently want this to be empty as it affects the InfoTrack entrypoint.
            // This might change in the near future.
            //Assert.Equal("QLD", resultUnderTest.InfoTrackMappingData.State);
            Assert.Null(resultUnderTest.InfoTrackMappingData.State);

            // Property information
            Assert.Equal("133", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.PropertyAddress?.StreetNumber);
            Assert.Equal("MOORE RESERVE", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.PropertyAddress?.StreetName);
            Assert.Equal("IPSWICH", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.PropertyAddress?.Suburb);
            Assert.Equal("QLD", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.PropertyAddress?.State);
            Assert.Equal("4305", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.PropertyAddress?.PostCode);
            Assert.Equal("437", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.PropertyAddress?.LotNumber);
            Assert.Equal("437", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.LotPlans?[0]?.Lot);
            Assert.Equal("112380", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.LotPlans?[0]?.PlanNumber);
            Assert.Equal("SP", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.LotPlans?[0]?.PlanType);
            Assert.Equal("50248019", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.LotPlans?[0]?.TitleReference);

            // Conveyancer information
            Assert.Equal("Company name", resultUnderTest.InfoTrackMappingData.LawyerDetail?.Organisation?.Name);
            Assert.Equal("test.conveyancer@workcloud.com.au", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Email);
            Assert.Equal("+61 123 456789", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Phone);
            Assert.Equal("555555", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Fax);
            Assert.Equal("Ms", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Individual?.Title);
            Assert.Equal("Firstname", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Individual?.GivenName);
            Assert.Equal("Middlename", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Individual?.GivenName2);
            Assert.Equal("Lastname", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Individual?.Surname);
            Assert.Equal("F", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Individual?.Gender);
            Assert.Equal("05/09/2010", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Individual?.DateOfBirth);
            Assert.Equal("123", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Address?.StreetNumber);
            Assert.Equal("Street name, addr2", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Address?.StreetName);
            Assert.Null(resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Address?.StreetType);
            Assert.Equal("city", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Address?.Suburb);
            Assert.Equal("state", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Address?.State);
            Assert.Equal("2612", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.Address?.PostCode);
            Assert.Equal("Something Ave, aaaddr2", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.PoBoxAddress?.PoBoxType);
            Assert.Equal("999", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.PoBoxAddress?.Number);
            Assert.Equal("mailing city", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.PoBoxAddress?.Suburb);
            Assert.Equal("mailing state", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.PoBoxAddress?.State);
            Assert.Equal("4444", resultUnderTest.InfoTrackMappingData.LawyerDetail?.ContactDetails?[0]?.PoBoxAddress?.PostCode);

            // Buyer
            Assert.Equal("Buyer-CompanyName", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Organisation?.Name);
            Assert.Equal("BuyerTaxNumber", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Organisation?.AcnOrAbn);
            Assert.Equal("BuyerTaxNumber", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Organisation?.Abn);
            Assert.Equal("buyer@test.data", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Email);
            Assert.Equal("+62 131 131313", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Phone);
            Assert.Equal("137412", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Fax);
            Assert.Equal("Buyer-Mrs", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Individual?.Title);
            Assert.Equal("Buyer-First", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Individual?.GivenName);
            Assert.Equal("Buyer-Middle", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Individual?.GivenName2);
            Assert.Equal("Buyer-Last", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Individual?.Surname);
            Assert.Equal("BuyerGender", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Individual?.Gender);
            Assert.Equal("13/01/2009", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Individual?.DateOfBirth);
            Assert.Equal("13", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Address?.StreetNumber);
            Assert.Equal("PhysBuyer Street, PhysBuyer addr line 2", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Address?.StreetName);
            Assert.Null(resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Address?.StreetType);
            Assert.Equal("PhysBuyer City", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Address?.Suburb);
            Assert.Equal("PhysBuyer State", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Address?.State);
            Assert.Equal("1313", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Purchasers?[0]?.Address?.PostCode);

            // Seller
            Assert.Equal("Seller-CompanyName", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Organisation?.Name);
            Assert.Equal("SellerTaxNumber", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Organisation?.AcnOrAbn);
            Assert.Equal("SellerTaxNumber", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Organisation?.Abn);
            Assert.Equal("seller@test.data", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Email);
            Assert.Equal("+63 133 131314", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Phone);
            Assert.Equal("137413", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Fax);
            Assert.Equal("Seller-Mrs", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Individual?.Title);
            Assert.Equal("Seller-First", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Individual?.GivenName);
            Assert.Equal("Seller-Middle", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Individual?.GivenName2);
            Assert.Equal("Seller-Last", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Individual?.Surname);
            Assert.Equal("SellerGender", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Individual?.Gender);
            Assert.Equal("14/01/2009", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Individual?.DateOfBirth);
            Assert.Equal("14", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Address?.StreetNumber);
            Assert.Equal("PhysSeller Street, PhysSeller addr line 2", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Address?.StreetName);
            Assert.Null(resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Address?.StreetType);
            Assert.Equal("PhysSeller City", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Address?.Suburb);
            Assert.Equal("PhysSeller State", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Address?.State);
            Assert.Equal("1314", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.Vendors?[0]?.Address?.PostCode);

            // Settlement data
            Assert.Equal("1200000", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.PurchasePrice);
            Assert.Equal("27/02/2018", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.SettlementDate);
            Assert.Equal("1:00 pm", resultUnderTest.InfoTrackMappingData.PropertyDetails?[0]?.SettlementTime);
        }
    }
}
