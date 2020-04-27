using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WCA.Core.Features.Conveyancing.PolicyRequest;
using WCA.Core.Features.Conveyancing.Services;
using WCA.FirstTitle.Client;
using Xunit;

namespace WCA.UnitTests.FirstTitle
{
    public class GetMappingFromFirstTitleResponseTests
    {
        private readonly FirstTitleToWCAMapper _mapper;
        public GetMappingFromFirstTitleResponseTests()
        {
            _mapper = new FirstTitleToWCAMapper();
        }

        [Theory]
        [ClassData(typeof(FirstTitleTestSuccessfulResponseCollection))]
        public async Task TestSuccessfulResponseMapping(TitleInsuranceResponse titleInsuranceResponse, string policyNumber, decimal premium, decimal gSTOnPremium, decimal stampDuty)
        {
            var mappedResponse = await _mapper.MapFromFirstTitleResponse(titleInsuranceResponse);
            Assert.Equal(policyNumber, mappedResponse.PolicyNumber);
            Assert.Equal(premium, mappedResponse.Price.Premium);
            Assert.Equal(gSTOnPremium, mappedResponse.Price.GSTOnPremium);
            Assert.Equal(stampDuty, mappedResponse.Price.StampDuty);
        }

        [Theory]
        [ClassData(typeof(FirstTitleTestFailedResponseCollection))]
        public async Task TestFailedResponseMapping(TitleInsuranceResponse titleInsuranceResponse, string errorMessage)
        {
            var ex = await Assert.ThrowsAsync<FirstTitlePolicyRequestException>(() => _mapper.MapFromFirstTitleResponse(titleInsuranceResponse));
            Assert.Equal(errorMessage, ex.Message);
        }
    }

    public class FirstTitleTestSuccessfulResponseCollection : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] {
                // This test case is copied from the First Title API documentation (r1.4), section 3.2
                new TitleInsuranceResponse()
                {
                    ProductionData = TitleInsuranceResponseProductionData.Yes,
                    RevisionNumber = new RevisionNumber()
                    {
                        LIXIVersion = "1.0",
                        UserVersion = "1.0",
                        UserType = RevisionNumberUserType.TitleInsurer
                    },
                    Identifier = new Identifier() { UniqueID = "32644899-a413-4797-a7cf-9ba00456735c" },
                    Date = new Date { Date1 = new DateTime(2013, 03, 14) },
                    Time = new Time { Time1 = new DateTime(2013, 03, 14, 13, 35, 40, 498) },
                    Message = new Message[]
                    {
                        new Message()
                        {
                            Identifier = new Identifier() { UniqueID = "dc8c6f59-c0fd-4a68-82f0-e0ad32cd2033"},
                            MessageRelatesTo = new Identifier[]
                            {
                                new Identifier() { UniqueID = "UniqueRequestID" }
                            },
                            MessageBody = new MessageBody[]
                            {
                                new MessageBody()
                                {
                                    Type = MessageBodyType.Information,
                                    Status = new Status[]
                                    {
                                        new Status() {
                                            Name = StatusName.Succeeded,
                                            Date = new Date() { Date1 = new DateTime() }
                                        }
                                    }
                                }
                            },
                            TitleInsuranceResponseSegment = new TitleInsuranceResponseSegment()
                            {
                                Identifier = new Identifier[] {
                                    new Identifier() { UniqueID = "70713b0a-5ba5-4253-91f0-a2585f6ec352" }
                                },
                                Insurer = new Insurer() { BusinessName = "FirstTitle" },
                                Policy = new Policy() { PolicyCode = "GLD13031002050" },
                                Price = new Price[]
                                {
                                    new Price()
                                    {
                                        PriceType = PricePriceType.Premium,
                                        Value = 300.00m,
                                        ValueSpecified = true
                                    },
                                    new Price()
                                    {
                                        PriceType = PricePriceType.GSTOnPremium,
                                        Value = 30.00m,
                                        ValueSpecified = true
                                    },
                                    new Price()
                                    {
                                        PriceType = PricePriceType.StampDuty,
                                        Value = 33.00m,
                                        ValueSpecified = true
                                    },
                                },
                                Date = new Date { Date1 = new DateTime(2013, 03, 14) }
                            }
                        }
                    }
                },
                "GLD13031002050", // PolicyNumber
                300.00m, // Premium
                30.00m, // GSTOnPremium
                33.00m // StampDuty
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

    public class FirstTitleTestFailedResponseCollection : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] {
                // This test case is copied from the First Title API documentation (r1.4), section 3.3
                new TitleInsuranceResponse()
                {
                    ProductionData = TitleInsuranceResponseProductionData.Yes,
                    RevisionNumber = new RevisionNumber()
                    {
                        LIXIVersion = "1.0",
                        UserVersion = "1.0",
                        UserType = RevisionNumberUserType.TitleInsurer
                    },
                    Identifier = new Identifier() { UniqueID = "7d6293a8-a4bc-4a69-ac64-98f3178b5f27" },
                    Date = new Date { Date1 = new DateTime(2013, 03, 14) },
                    Time = new Time { Time1 = new DateTime(2013, 03, 14, 14, 7, 18, 264) },
                    Message = new Message[]
                    {
                        new Message()
                        {
                            Identifier = new Identifier() { UniqueID = "d49788fe-9a93-4adf-a3d3-d09973525c0f"},
                            MessageRelatesTo = new Identifier[]
                            {
                                new Identifier() { UniqueID = "UniqueRequestID" }
                            },
                            MessageBody = new MessageBody[]
                            {
                                new MessageBody()
                                {
                                    Type = MessageBodyType.DataError,
                                     MessageAnnotation = new MessageAnnotation[]
                                     {
                                         new MessageAnnotation()
                                         {
                                             Type = MessageAnnotationType.EndUserMessage,
                                             TypeSpecified = true,
                                             Value = "[EtitleException: 40081] An insured party reference must be supplied."
                                         }
                                     },
                                     Status = new Status[]
                                     {
                                         new Status()
                                         {
                                             Name = StatusName.Failed,
                                             Date = new Date() { Date1 = new DateTime(2013,3,14) }
                                         }
                                     }
                                }
                            },
                            TitleInsuranceResponseSegment = new TitleInsuranceResponseSegment()
                            {
                                Identifier = new Identifier[]
                                {
                                    new Identifier() { UniqueID = "7138cf93-9d8e-4cea-907f-b7d73e33cc64" }
                                },
                                Insurer = new Insurer() { BusinessName = "FirstTitle" },
                                Policy = new Policy()
                                {
                                    PolicyCode = ""
                                },
                                Price = new Price[]
                                {
                                    new Price() { PriceType = PricePriceType.Premium }
                                },
                                Date = new Date() { Date1 = new DateTime(2013, 3, 14) }
                            }
                        }
                    }
                },
                "[EtitleException: 40081] An insured party reference must be supplied."
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
