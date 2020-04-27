using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using WCA.Core;
using WCA.FirstTitle.Client;
using Xunit;

namespace WCA.IntegrationTests.FirstTitle
{
    public class FirstTitleConnectivityTests
    {
        /*
        private readonly IConfiguration _configuration;
        private readonly string _authUsername;
        private readonly string _authPassword;

        public FirstTitleConnectivityTests()
        {
            // xunit does not set up config for us, so we have to do it manually
            var assemblyLocation = typeof(FirstTitleConnectivityTests).Assembly.Location;
            var basePath = Path.GetDirectoryName(assemblyLocation);
            _configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("WorkCloudApplications")
                .Build();

            var wCACoreSettings = _configuration.GetSection("WCACoreSettings").Get<WCACoreSettings>();

            // Ensure tests are only run against the staging environment
            if (wCACoreSettings.FirstTitleSettings.Environment != FirstTitleEnvironment.Staging)
            {
                throw new Exception("First Title tests should only be run against the staging environment");
            }

            _authUsername = wCACoreSettings.FirstTitleSettings.AuthUsername;
            _authPassword = wCACoreSettings.FirstTitleSettings.AuthPassword;
        }

        [Fact]
        public async void CanConnectToFirstTitleAPI()
        {
            using (var httpClient = new HttpClient())
            using (IFirstTitleClient firstTitleClient = new FirstTitleClient(httpClient, _configuration, new NullLogger<FirstTitleClient>()))
            {
                Assert.True(await firstTitleClient.CheckCredentials(_authUsername, _authPassword));
            }
        }

        [Theory]
        [ClassData(typeof(FirstTitleTestDataCollection))]
        public async void CanRequestInsurance(TitleInsuranceRequest titleInsuranceRequest)
        {
            using (var httpClient = new HttpClient())
            using (IFirstTitleClient firstTitleClient = new FirstTitleClient(httpClient, _configuration, new NullLogger<FirstTitleClient>()))
            {
                var response = await firstTitleClient.DoRequestForInsurance(titleInsuranceRequest);

                var responseStatus = response.Message.FirstOrDefault()?.MessageBody.FirstOrDefault()?.Status.FirstOrDefault()?.Name;
                var errorMessage = response.Message.FirstOrDefault()?.MessageBody.FirstOrDefault()?.MessageAnnotation.FirstOrDefault()?.Value;
                if (!(errorMessage is null))
                {
                    // If this is not the first time that this test data has been sent to First Title
                    // then they will already have a record of a policy.
                    // In that case, the request will fail with an error message of the format
                    // "[EtitleException: 20331] Property address '1 Test St Testing NSW 2000 Australia' already exists."
                    Assert.Equal(StatusName.Failed, responseStatus);
                    Assert.StartsWith("[EtitleException: 20331]", errorMessage, StringComparison.InvariantCulture);
                } else
                {
                    // If there was no error message, then the request should have succeeded
                    Assert.Equal(StatusName.Succeeded, responseStatus);
                }
            }
        }
        */
    }

    public class FirstTitleTestDataCollection : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] {
                // This test case is copied from the First Title API documentation (r1.4), section 3.1
                new TitleInsuranceRequest
                {
                    ProductionData = TitleInsuranceRequestProductionData.No,
                    Identifier = new Identifier { UniqueID = "UniqueRequestID" },
                    Date = new Date { Date1 = new DateTime(2013, 2, 13) },
                    Time = new Time { Time1 = new DateTime(2013, 2, 13, 12, 16, 26) },
                    Application = new Application
                    {
                        FinancialSegment = new ValueItem[]
                        {
                            new ValueItem
                            {
                                Identifier = new Identifier[] {
                                    new Identifier { UniqueID = "N/A" }
                                },
                                Asset = new Asset
                                {
                                    RealEstate = new RealEstate
                                    {
                                        Location = new Location
                                        {
                                            Title = new Title
                                            {
                                                TorrensVolume = "123",
                                                TorrensFolio = "45678",
                                                TitleType = TitleTitleType.Torrens,
                                                TenureType = TitleTenureType.Freehold,
                                                LegalDescription = "Lot 1 of Plan 2 in Subdivision 3"
                                            },
                                            Item = new Address
                                            {
                                                City = "Testing",
                                                State = new State
                                                {
                                                    Name = StateName.NSW,
                                                    NameSpecified = true // Note: Do not forget to include this (and similar <X>Specified properties in other objects)! That leads to things being omitted by the SOAP XML serializer, cryptic error messages bring returned from First Title, and much wailing and gnashing of teeth
                                                },
                                                Postcode = "2000",
                                                Country = new Country { Value = "AU" },
                                                Item = new StreetAddress
                                                {
                                                    UnitNo = "",
                                                    StreetNo = "1",
                                                    Street = new Street {
                                                        Type = StreetType.Street,
                                                        TypeSpecified = true,
                                                        Value = "Test"
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    TitleInsuranceSegment = new TitleInsuranceSegment
                    {
                        RequestForInsurance = new RequestForInsurance
                        {
                            TransactionServiceType = RequestForInsuranceTransactionServiceType.NewRFI,
                            Identifier = new Identifier[]
                            {
                                new Identifier { UniqueID = "YourReference" }
                            },
                            SalesBundle = "HomeOwnersGOLD",
                            InsuredAmount = new InsuredAmount
                            {
                                Value = 456123.00m,
                                ValueSpecified = true
                            },
                            InsuredParty = new RelatedEntityRef[]
                            {
                                new RelatedEntityRef { RelatedID = "UniqueOwnerID" }
                            },
                            EstimatedSettlementDate = new EstimatedSettlementDate {
                                Date = new Date { Date1 = new DateTime(2019, 12, 14) }
                            }
                        }
                    },
                    RelatedPartySegment = new RelatedParty[]
                    {
                        new RelatedParty
                        {
                            RelPartyType = RelatedPartyRelPartyType.Owner,
                            Identifier = new Identifier[]
                            {
                                new Identifier { UniqueID = "UniqueOwnerID" }
                            },
                            Item = new Person
                            {
                                PersonName = new PersonName
                                {
                                    NameTitle = new NameTitle
                                    {
                                        Value = NameTitleValue.Mrs,
                                        ValueSpecified = true
                                    },
                                    FirstName = "Test",
                                    Surname = "Person"
                                },
                                ContactDetails = new ContactDetails
                                {
                                    Items = new object[]
                                    {
                                        new AddressDetails
                                        {
                                            Item = new Address
                                            {
                                                City = "Testville",
                                                State = new State
                                                {
                                                    Name = StateName.NSW,
                                                    NameSpecified = true,
                                                },
                                                Postcode = "2000",
                                                Country = new Country { Value = "Australia" },
                                                Item = new StreetAddress
                                                {
                                                    StreetNo = "2",
                                                    Street = new Street
                                                    {
                                                        Type = StreetType.Road,
                                                        TypeSpecified = true,
                                                        Value = "Tester"
                                                    }
                                                }
                                            }
                                        },
                                        new Email { Value = "test@etitle.com.au" }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
