using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Core.Helpers;
using WCA.Core.Services;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using static WCA.Core.Features.InfoTrack.SendMappingsToInfoTrack.SendMappingsToInfoTrackCommand;

namespace WCA.Core.Features.InfoTrack
{
    public class GetMappingDataFromActionstep
    {
        public class GetMappingDataFromActionstepQuery : IQuery<SendMappingsToInfoTrack.SendMappingsToInfoTrackCommand>
        {
            public string ActionstepOrgKey { get; set; }
            public int MatterId { get; set; }
            public WCAUser AuthenticatedUser { get; set; }
        }

        public class Validator : AbstractValidator<GetMappingDataFromActionstepQuery>
        {
            public Validator()
            {
                RuleFor(c => c.MatterId).NotEmpty();
                RuleFor(c => c.ActionstepOrgKey).NotEmpty();
                RuleFor(c => c.AuthenticatedUser).NotNull();
            }
        }

        public class Handler : IRequestHandler<GetMappingDataFromActionstepQuery, SendMappingsToInfoTrack.SendMappingsToInfoTrackCommand>
        {
            private readonly ActionstepSettings actionstepSettings;
            private readonly Validator validator;
            private readonly IActionstepService actionstepService;
            private readonly IMediator mediator;
            private readonly IInfoTrackCredentialRepository infoTrackCredentialRepository;
            private readonly ITelemetryLogger telemetryLogger;

            public Handler(
                Validator validator,
                IActionstepService actionstepService,
                IOptions<WCACoreSettings> settingsAccessor,
                IMediator mediator,
                IInfoTrackCredentialRepository infoTrackCredentialRepository,
                ITelemetryLogger telemetryLogger)
            {
                if (settingsAccessor is null) throw new ArgumentNullException(nameof(settingsAccessor));

                this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
                this.actionstepService = actionstepService ?? throw new ArgumentNullException(nameof(actionstepService));
                this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
                this.infoTrackCredentialRepository = infoTrackCredentialRepository ?? throw new ArgumentNullException(nameof(infoTrackCredentialRepository));
                this.telemetryLogger = telemetryLogger ?? throw new ArgumentNullException(nameof(telemetryLogger));
                actionstepSettings = settingsAccessor.Value?.ActionstepSettings;
            }

            public async Task<SendMappingsToInfoTrack.SendMappingsToInfoTrackCommand> Handle(GetMappingDataFromActionstepQuery message, CancellationToken token)
            {
                if (message is null) throw new ArgumentNullException(nameof(message));

                telemetryLogger.TrackTrace("GetMappingDataFromActionstep handler called", WCASeverityLevel.Information, null);
                try
                {
                    ValidationResult validationResult = validator.Validate(message);
                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException("Invalid input.", validationResult.Errors);
                    }

                    // If we get this far, we know that the user has permisisons to the Actionstep org.
                    // We must only get InfoTrack credentials if the user 
                    var infoTrackCredentials = await infoTrackCredentialRepository.FindCredential(message.ActionstepOrgKey);
                    if (infoTrackCredentials == null)
                    {
                        throw new InfoTrackCredentialsNotFoundException(message.ActionstepOrgKey, message.AuthenticatedUser);
                    }

                    var tokenSetQuery = new TokenSetQuery(message.AuthenticatedUser?.Id, message.ActionstepOrgKey);

                    // Response 1 Get Action Participants
                    var allParticipants = await actionstepService.Handle<ActionParticipantsResponse>(
                        new GenericActionstepRequest(
                            tokenSetQuery,
                            $"/rest/actionparticipants?action={message.MatterId}&include=participantType,participant",
                            HttpMethod.Get));

                    // Response 2 Data collection info
                    var dataCollectionRecordValues = await actionstepService.Handle<dynamic>(
                        new GenericActionstepRequest(
                            tokenSetQuery,
                            "/rest/datacollectionrecordvalues" +
                                $"?action={message.MatterId}" +
                                $"&dataCollectionRecord[dataCollection][name_in]=property,convdet,keydates" +
                                $"&dataCollectionField[name_in]=titleref,lotno,planno,plantype,lotno2,planno2,plantype2,smtdateonly,smttime,purprice" +
                                $"&include=dataCollectionField,dataCollection",
                            HttpMethod.Get));

                    // Prepare result types for population
                    var queryResult = new SendMappingsToInfoTrack.SendMappingsToInfoTrackCommand()
                    {
                        AuthenticatedUser = message.AuthenticatedUser,
                        InfoTrackCredentials = infoTrackCredentials,
                        InfoTrackMappingData = new InfoTrackMappingData()
                        {
                            ClientReference = message.MatterId.ToString(CultureInfo.InvariantCulture),
                            RetailerReference = $"WCA_{message.ActionstepOrgKey}|{message.AuthenticatedUser.Id}"
                        }
                    };

                    // Read responses and populate result object
                    if (allParticipants != null)
                    {
                        foreach (var propertyAddressParticipant in FindParticipantsByName(allParticipants, "Property_Address"))
                        {
                            (string streetNumber, string streetName) =
                                AddressHelper.ParseStreetNumber(
                                    propertyAddressParticipant.physicalAddressLine1,
                                    propertyAddressParticipant.physicalAddressLine2);

                            queryResult.InfoTrackMappingData.PropertyDetails.Add(new InfoTrackMappingData.PropertyDetail()
                            {
                                PropertyAddress = new InfoTrackMappingData.Address()
                                {
                                    StreetNumber = streetNumber,
                                    StreetName = streetName,
                                    Suburb = propertyAddressParticipant.physicalCity,
                                    State = propertyAddressParticipant.physicalStateProvince,
                                    PostCode = propertyAddressParticipant.physicalPostCode
                                }
                            });

                            // We currently want this to be empty as it affects the InfoTrack entrypoint.
                            // This might change in the near future.
                            //queryResult.State = propertyAddressParticipant.physicalStateProvince;

                            // If there are multiple participants in the Property_Address field, we only want the first one
                            break;
                        }

                        //var conveyancerTypeId = allParticipants.linked.participanttypes.FirstOrDefault(t => t.name == "Conveyancer")?.id;
                        var organisationNameSet = false;
                        foreach (var conveyancerParticipant in FindParticipantsByName(allParticipants, "Conveyancer"))
                        {
                            if (!organisationNameSet)
                            {
                                queryResult.InfoTrackMappingData.LawyerDetail.Organisation.Name = conveyancerParticipant.companyName;
                            }

                            (string addressStreetNumber, string addressStreetName) =
                                AddressHelper.ParseStreetNumber(
                                    conveyancerParticipant.physicalAddressLine1,
                                    conveyancerParticipant.physicalAddressLine2);

                            (string mailingStreetNumber, string mailingStreetName) =
                                AddressHelper.ParseStreetNumber(
                                    conveyancerParticipant.mailingAddressLine1,
                                    conveyancerParticipant.mailingAddressLine2);

                            queryResult.InfoTrackMappingData.LawyerDetail.ContactDetails.Add(new InfoTrackMappingData.Contactdetail()
                            {
                                Email = conveyancerParticipant.email,
                                Phone = GetBestPhoneForParticipant(conveyancerParticipant),
                                Fax = conveyancerParticipant.fax,
                                Individual = new InfoTrackMappingData.Individual()
                                {
                                    Title = conveyancerParticipant.salutation,
                                    GivenName = conveyancerParticipant.firstName,
                                    GivenName2 = conveyancerParticipant.middleName,
                                    Surname = conveyancerParticipant.lastName,
                                    Gender = conveyancerParticipant.gender,
                                    DateOfBirth = ParseDateForInfoTrack(conveyancerParticipant.birthTimestamp),
                                },
                                Address = new InfoTrackMappingData.Address()
                                {
                                    StreetNumber = addressStreetNumber,
                                    StreetName = addressStreetName,
                                    Suburb = conveyancerParticipant.physicalCity,
                                    State = conveyancerParticipant.physicalStateProvince,
                                    PostCode = conveyancerParticipant.physicalPostCode,
                                },
                                PoBoxAddress = new InfoTrackMappingData.Poboxaddress()
                                {
                                    PoBoxType = mailingStreetName,
                                    Number = mailingStreetNumber,
                                    Suburb = conveyancerParticipant.mailingCity,
                                    State = conveyancerParticipant.mailingStateProvince,
                                    PostCode = conveyancerParticipant.mailingPostCode
                                }
                            });
                        }

                        foreach (var buyerParticipant in FindParticipantsByName(allParticipants, "Buyer"))
                        {
                            (string addressStreetNumber, string addressStreetName) =
                                AddressHelper.ParseStreetNumber(
                                    buyerParticipant.physicalAddressLine1,
                                    buyerParticipant.physicalAddressLine2);

                            if (queryResult.InfoTrackMappingData.PropertyDetails.Count < 1)
                            {
                                queryResult.InfoTrackMappingData.PropertyDetails.Add(new InfoTrackMappingData.PropertyDetail());
                            }

                            queryResult.InfoTrackMappingData.PropertyDetails[0].Purchasers.Add(new InfoTrackMappingData.Purchaser()
                            {
                                Organisation = new InfoTrackMappingData.Organisation()
                                {
                                    Name = buyerParticipant.companyName,
                                    AcnOrAbn = buyerParticipant.taxNumber,
                                    Abn = buyerParticipant.taxNumber
                                },
                                Email = buyerParticipant.email,
                                Phone = GetBestPhoneForParticipant(buyerParticipant),
                                Fax = buyerParticipant.fax,
                                Individual = new InfoTrackMappingData.Individual()
                                {
                                    Title = buyerParticipant.salutation,
                                    GivenName = buyerParticipant.firstName,
                                    GivenName2 = buyerParticipant.middleName,
                                    Surname = buyerParticipant.lastName,
                                    Gender = buyerParticipant.gender,
                                    DateOfBirth = ParseDateForInfoTrack(buyerParticipant.birthTimestamp)
                                },
                                Address = new InfoTrackMappingData.Address()
                                {
                                    StreetNumber = addressStreetNumber,
                                    StreetName = addressStreetName,
                                    Suburb = buyerParticipant.physicalCity,
                                    State = buyerParticipant.physicalStateProvince,
                                    PostCode = buyerParticipant.physicalPostCode
                                }
                            });
                        }

                        foreach (var sellerParticipant in FindParticipantsByName(allParticipants, "Seller"))
                        {
                            (string addressStreetNumber, string addressStreetName) =
                                AddressHelper.ParseStreetNumber(
                                    sellerParticipant.physicalAddressLine1,
                                    sellerParticipant.physicalAddressLine2);

                            if (queryResult.InfoTrackMappingData.PropertyDetails.Count < 1)
                            {
                                queryResult.InfoTrackMappingData.PropertyDetails.Add(new InfoTrackMappingData.PropertyDetail());
                            }

                            queryResult.InfoTrackMappingData.PropertyDetails[0].Vendors.Add(new InfoTrackMappingData.Vendor()
                            {
                                Organisation = new InfoTrackMappingData.Organisation()
                                {
                                    Name = sellerParticipant.companyName,
                                    AcnOrAbn = sellerParticipant.taxNumber,
                                    Abn = sellerParticipant.taxNumber
                                },
                                Email = sellerParticipant.email,
                                Phone = GetBestPhoneForParticipant(sellerParticipant),
                                Fax = sellerParticipant.fax,
                                Individual = new InfoTrackMappingData.Individual()
                                {
                                    Title = sellerParticipant.salutation,
                                    GivenName = sellerParticipant.firstName,
                                    GivenName2 = sellerParticipant.middleName,
                                    Surname = sellerParticipant.lastName,
                                    Gender = sellerParticipant.gender,
                                    DateOfBirth = ParseDateForInfoTrack(sellerParticipant.birthTimestamp),
                                },
                                Address = new InfoTrackMappingData.Address()
                                {
                                    StreetNumber = addressStreetNumber,
                                    StreetName = addressStreetName,
                                    Suburb = sellerParticipant.physicalCity,
                                    State = sellerParticipant.physicalStateProvince,
                                    PostCode = sellerParticipant.physicalPostCode
                                }
                            });
                        }
                    }

                    if (queryResult.InfoTrackMappingData.PropertyDetails.Count < 1)
                    {
                        queryResult.InfoTrackMappingData.PropertyDetails.Add(new InfoTrackMappingData.PropertyDetail());
                    }

                    if (dataCollectionRecordValues != null && dataCollectionRecordValues.datacollectionrecordvalues != null)
                    {
                        int? propertyDataCollectionId = ((IEnumerable<dynamic>)dataCollectionRecordValues?.linked?.datacollections)?.FirstOrDefault(d => d.name == "property")?.id;
                        if (propertyDataCollectionId.HasValue)
                        {
                            // Get Title Reference and copy it to PropertyReferences
                            var titleReference = ReadDataCollectionRecordValueString("titleref", propertyDataCollectionId.Value, dataCollectionRecordValues);
                            queryResult.InfoTrackMappingData.PropertyDetails[0].PropertyReferences.Add(
                                new InfoTrackMappingData.PropertyReference()
                                {
                                    Reference = titleReference
                                });

                            // Lot details and copy them to the first property
                            queryResult.InfoTrackMappingData.PropertyDetails[0].PropertyAddress.LotNumber = ReadDataCollectionRecordValueString("lotno", propertyDataCollectionId.Value, dataCollectionRecordValues);
                            queryResult.InfoTrackMappingData.PropertyDetails[0].LotPlans.Add(new InfoTrackMappingData.Lotplan()
                            {
                                Lot = queryResult.InfoTrackMappingData.PropertyDetails[0].PropertyAddress.LotNumber,
                                PlanNumber = ReadDataCollectionRecordValueString("planno", propertyDataCollectionId.Value, dataCollectionRecordValues),
                                PlanType = ReadDataCollectionRecordValueString("plantype", propertyDataCollectionId.Value, dataCollectionRecordValues),
                                TitleReference = titleReference
                            });

                            var lot2 = new InfoTrackMappingData.Lotplan()
                            {
                                Lot = ReadDataCollectionRecordValueString("lotno2", propertyDataCollectionId.Value, dataCollectionRecordValues),
                                PlanNumber = ReadDataCollectionRecordValueString("planno2", propertyDataCollectionId.Value, dataCollectionRecordValues),
                                PlanType = ReadDataCollectionRecordValueString("plantype2", propertyDataCollectionId.Value, dataCollectionRecordValues),
                                TitleReference = titleReference
                            };

                            if (lot2.ContainsLotData())
                            {
                                queryResult.InfoTrackMappingData.PropertyDetails[0].LotPlans.Add(lot2);
                            }
                        }

                        int? convdetDataCollectionId = ((IEnumerable<dynamic>)dataCollectionRecordValues?.linked?.datacollections)?.FirstOrDefault(d => d.name == "convdet")?.id;
                        if (convdetDataCollectionId.HasValue)
                        {
                            queryResult.InfoTrackMappingData.PropertyDetails[0].SettlementTime = ReadDataCollectionRecordValueString("smttime", convdetDataCollectionId.Value, dataCollectionRecordValues);
                            queryResult.InfoTrackMappingData.PropertyDetails[0].PurchasePrice = ReadDataCollectionRecordValueString("purprice", convdetDataCollectionId.Value, dataCollectionRecordValues);
                        }

                        int? keydatesDataCollectionId = ((IEnumerable<dynamic>)dataCollectionRecordValues?.linked?.datacollections)?.FirstOrDefault(d => d.name == "keydates")?.id;
                        if (keydatesDataCollectionId.HasValue)
                        {
                            queryResult.InfoTrackMappingData.PropertyDetails[0].SettlementDate = ParseDateForInfoTrack(ReadDataCollectionRecordValueString("smtdateonly", keydatesDataCollectionId.Value, dataCollectionRecordValues));
                        }
                    }

                    return queryResult;
                }
                catch (Exception ex)
                {
                    telemetryLogger.TrackTrace(
                        "Unexpected exception getting InfoTrack mapping URL",
                        WCASeverityLevel.Error,
                        new Dictionary<string, string>()
                        {
                            { "Actionstep Org", message.ActionstepOrgKey },
                            { "Matter", message.MatterId.ToString(CultureInfo.InvariantCulture) },
                            { "User", message.AuthenticatedUser.Id },
                            { "Exception Message", ex.Message },
                            { "Stack Trace", ex.StackTrace }
                        });

                    throw;
                }
            }

            private string ParseDateForInfoTrack(string inputDate)
            {
                if (DateTime.TryParse(inputDate, out var result))
                    return result.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                return null;
            }

            private static string GetBestPhoneForParticipant(ActionParticipantsResponse.Participant participant)
            {
                // Check phone fields in order of preference.
                // Phone 2 is preferred over Phone 1 because by default Phone 2 is "Mobile" and
                // Phone 1 is "Business". So the idea is that with a default configuration the
                // mobile number will be used first, then business, then others.
                if (!string.IsNullOrEmpty(participant.phone2Number))
                    return $"+{participant.phone2Country} {participant.phone2Area} {participant.phone2Number}";
                else if (!string.IsNullOrEmpty(participant.phone1Number))
                    return $"+{participant.phone1Country} {participant.phone1Area} {participant.phone1Number}";
                else if (!string.IsNullOrEmpty(participant.phone3Number))
                    return $"+{participant.phone3Country} {participant.phone3Area} {participant.phone3Number}";
                else if (!string.IsNullOrEmpty(participant.phone4Number))
                    return $"+{participant.phone4Country} {participant.phone4Area} {participant.phone4Number}";
                else
                    return null;
            }

            private static IEnumerable<ActionParticipantsResponse.Participant> FindParticipantsByName(ActionParticipantsResponse allParticipants, string participantTypeName)
            {
                var participantTypeId = allParticipants.linked.participanttypes.FirstOrDefault(t => t.name == participantTypeName)?.id;
                if (participantTypeId != null && participantTypeId.HasValue)
                {
                    var participantIdAsString = allParticipants.actionparticipants
                        .FirstOrDefault(p => p.links.participantType == participantTypeId.Value.ToString(CultureInfo.InvariantCulture))?.links?.participant;

                    if (!string.IsNullOrEmpty(participantIdAsString))
                    {
                        return allParticipants.linked.participants
                            .Where(p => p.id.ToString(CultureInfo.InvariantCulture) == participantIdAsString);
                    }
                }

                return Enumerable.Empty<ActionParticipantsResponse.Participant>();
            }

            private static string ReadDataCollectionRecordValueString(string fieldName, int dataCollectionId, dynamic dataCollectionRecordValues)
            {
                return ((IEnumerable<dynamic>)dataCollectionRecordValues?.datacollectionrecordvalues)
                    ?.FirstOrDefault(t => ((string)t.id).StartsWith($"{dataCollectionId}--{fieldName}--", StringComparison.Ordinal))
                    ?.stringValue;
            }
        }

        private class ActionParticipantsResponse
        {
            public Links links { get; set; }

            [JsonConverter(typeof(SingleOrArrayConverter<ActionParticipant>))]
            public List<ActionParticipant> actionparticipants { get; set; }
            public Linked linked { get; set; }
            public Meta meta { get; set; }


            public class Links
            {
                [JsonProperty("actionparticipants.action")]
                public Link actionparticipantsaction { get; set; }

                [JsonProperty("actionparticipants.participantType")]

                public Link actionparticipantsparticipantType { get; set; }

                [JsonProperty("actionparticipants.participant")]

                public Link actionparticipantsparticipant { get; set; }

                [JsonProperty("participants.physicalCountry")]

                public Link participantsphysicalCountry { get; set; }

                [JsonProperty("participants.mailingCountry")]

                public Link participantsmailingCountry { get; set; }

                [JsonProperty("participants.division")]

                public Link participantsdivision { get; set; }
            }

            public class Link
            {
                public string href { get; set; }
                public string type { get; set; }
            }

            public class Linked
            {
                [JsonConverter(typeof(SingleOrArrayConverter<Participanttype>))]
                public List<Participanttype> participanttypes { get; set; }

                [JsonConverter(typeof(SingleOrArrayConverter<Participant>))]
                public List<Participant> participants { get; set; }
            }

            public class Participanttype
            {
                public int id { get; set; }
                public string name { get; set; }
                public string displayName { get; set; }
                public string description { get; set; }
                public string isBaseParticipantType { get; set; }
                public string companyFlag { get; set; }
                public string taxNumberAlias { get; set; }
            }

            public class Participant
            {
                public int id { get; set; }
                public string displayName { get; set; }
                public string isCompany { get; set; }
                public string companyName { get; set; }
                public string salutation { get; set; }
                public string firstName { get; set; }
                public string middleName { get; set; }
                public string lastName { get; set; }
                public string suffix { get; set; }
                public string preferredName { get; set; }
                public string physicalAddressLine1 { get; set; }
                public string physicalAddressLine2 { get; set; }
                public string physicalCity { get; set; }
                public string physicalStateProvince { get; set; }
                public string physicalPostCode { get; set; }
                public string mailingAddressLine1 { get; set; }
                public string mailingAddressLine2 { get; set; }
                public string mailingCity { get; set; }
                public string mailingStateProvince { get; set; }
                public string mailingPostCode { get; set; }
                public string phone1Label { get; set; }
                public int? phone1Country { get; set; }
                public int? phone1Area { get; set; }
                public string phone1Number { get; set; }
                public string phone1Notes { get; set; }
                public string phone2Label { get; set; }
                public int? phone2Country { get; set; }
                public int? phone2Area { get; set; }
                public string phone2Number { get; set; }
                public string phone2Notes { get; set; }
                public string phone3Label { get; set; }
                public int? phone3Country { get; set; }
                public int? phone3Area { get; set; }
                public string phone3Number { get; set; }
                public string phone3Notes { get; set; }
                public string phone4Label { get; set; }
                public int? phone4Country { get; set; }
                public int? phone4Area { get; set; }
                public string phone4Number { get; set; }
                public string phone4Notes { get; set; }
                public string fax { get; set; }
                public string sms { get; set; }
                public string email { get; set; }
                public string website { get; set; }
                public string occupation { get; set; }
                public string taxNumber { get; set; }
                public string birthTimestamp { get; set; }
                public string deathTimestamp { get; set; }
                public string maritalStatus { get; set; }
                public string gender { get; set; }
                public DateTime modifiedTimestamp { get; set; }
                public DateTime createdTimestamp { get; set; }
                public ParticipantLinks links { get; set; }
            }

            public class ParticipantLinks
            {
                public string physicalCountry { get; set; }
                public string mailingCountry { get; set; }
                public string division { get; set; }
            }

            public class Meta
            {
                public Paging paging { get; set; }
                public Debug debug { get; set; }
            }

            public class Paging
            {
                public ActionParticipants actionparticipants { get; set; }
            }

            public class ActionParticipants
            {
                public int recordCount { get; set; }
                public int pageCount { get; set; }
                public int page { get; set; }
                public int pageSize { get; set; }
                public string prevPage { get; set; }
                public string nextPage { get; set; }
            }

            public class Debug
            {
                public string requestTime { get; set; }
                public string mem { get; set; }
                public string server { get; set; }
                public string cb { get; set; }
                public string time { get; set; }
                public string appload { get; set; }
                public string app { get; set; }
                public string db { get; set; }
                public string dbc { get; set; }
                public string qc { get; set; }
                public string uqc { get; set; }
                public string fc { get; set; }
                public string rl { get; set; }
            }

            public class ActionParticipant
            {
                public string id { get; set; }
                public int participantNumber { get; set; }
                public ActionParticipantLinks links { get; set; }
            }

            public class ActionParticipantLinks
            {
                public string action { get; set; }
                public string participantType { get; set; }
                public string participant { get; set; }
            }
        }
    }
}