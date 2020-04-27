using AutoMapper;
using MediatR;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Core.Features.Actionstep.Connection;
using WCA.Core.Features.Conveyancing.Services;
using WCA.Core.Helpers;
using WCA.Domain.Actionstep;
using WCA.Domain.Conveyancing;
using WCA.PEXA.Client;

namespace WCA.Core.Features.Conveyancing.WorkspaceCreation
{
    public class PEXAWorkspaceCreationRequestFromActionstepQueryHandler : IRequestHandler<PEXAWorkspaceCreationRequestFromActionstepQuery, PEXAWorkspaceCreationRequestWithActionstepResponse>
    {
        private readonly IActionstepService _actionstepService;
        private readonly IMapper _mapper;
        private readonly IActionstepToWCAMapper _actionstepToWCAMapper;

        public PEXAWorkspaceCreationRequestFromActionstepQueryHandler(
            IActionstepService actionstepService,
            IMapper mapper,
            IActionstepToWCAMapper actionstepToWCAMapper)
        {
            _actionstepService = actionstepService;
            _mapper = mapper;
            _actionstepToWCAMapper = actionstepToWCAMapper;
        }

        public Task<PEXAWorkspaceCreationRequestWithActionstepResponse> Handle(PEXAWorkspaceCreationRequestFromActionstepQuery request, CancellationToken cancellationToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            // Get Actionstep matter info
            var tokenSetQuery = new TokenSetQuery(request.AuthenticatedUser?.Id, request.ActionstepOrg);

            try
            {
                // New token refresh handling means these can run in parallel.
                var actionResponseTask = _actionstepService.Handle<GetActionResponse>(new GetActionRequest
                {
                    ActionId = request.MatterId,
                    TokenSetQuery = tokenSetQuery
                });

                var actionParticipantsResponseTask = _actionstepService.Handle<ListActionParticipantsResponse>(new ListActionParticipantsRequest
                {
                    ActionstepId = request.MatterId,
                    TokenSetQuery = tokenSetQuery
                });

                var dataCollectionRecordValuesResponseTask = _actionstepService.Handle<ListDataCollectionRecordValuesResponse>(new ListDataCollectionRecordValuesRequest
                {
                    ActionstepId = request.MatterId,
                    TokenSetQuery = tokenSetQuery,
                    DataCollectionRecordNames = { "property", "convdet", "keydates" },
                    DataCollectionFieldNames = { "titleref", "lotno", "planno", "plantype", "smtdateonly", "smttime", "purprice", "ConveyType" }
                });

                Task.WaitAll(actionResponseTask, actionParticipantsResponseTask, dataCollectionRecordValuesResponseTask);

                // Transform Actionstep matter info into generic WCA Conveyancing Matter type
                // using specific configuration for this client (to account for Actionstep
                // action type configuration).
                var wCAConveyancingMatter = _actionstepToWCAMapper.MapFromActionstepTypes(
                    actionResponseTask.Result,
                    actionParticipantsResponseTask.Result,
                    dataCollectionRecordValuesResponseTask.Result);

                PEXAWorkspaceCreationRequestWithActionstepResponse pexaWorkspaceRequestWithActionstepData = new PEXAWorkspaceCreationRequestWithActionstepResponse
                {
                    CreatePexaWorkspaceCommand = new CreatePexaWorkspaceCommand()
                    {
                        PexaWorkspaceCreationRequest = new WorkspaceCreationRequest(),
                        OrgKey = request.ActionstepOrg,
                        MatterId = request.MatterId
                    },
                    PexaRoleSpecified = true,
                    ActionstepData = wCAConveyancingMatter
                };


                var landTitle = _mapper.Map<PropertyDetails, WorkspaceCreationRequestTypeLandTitleDetailsLandTitle>(wCAConveyancingMatter.PropertyDetails);

                pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.ParticipantSettlementAcceptanceStatus = "Accepted";
                pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.LandTitleDetails = new WorkspaceCreationRequestTypeLandTitleDetails();
                pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.LandTitleDetails.LandTitle.Add(landTitle);

                var actionTypeAlias = String.Empty;
                switch (wCAConveyancingMatter.ActionType)
                {
                    case "Conveyancing - NSW":
                        actionTypeAlias = "NSW";
                        break;
                    case "Conveyancing  - Queensland":
                        actionTypeAlias = "QLD";
                        break;
                    case "Conveyancing - Victoria":
                        actionTypeAlias = "VIC";
                        break;
                    default:
                        actionTypeAlias = String.Empty;
                        break;
                }

                if (!string.IsNullOrEmpty(actionTypeAlias))
                {
                    pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.Jurisdiction = actionTypeAlias;
                }

                if (wCAConveyancingMatter.ConveyancingType == ConveyancingType.Purchase)
                    pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.Role = PexaRole.Incoming_Proprietor;
                else if (wCAConveyancingMatter.ConveyancingType == ConveyancingType.Sale)
                    pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.Role = PexaRole.Proprietor_on_Title;
                else
                    pexaWorkspaceRequestWithActionstepData.PexaRoleSpecified = false;

                if (!string.IsNullOrEmpty(request.ActionstepOrg) && request.MatterId > 0)
                {
                    pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.SubscriberReference = ActionstepMatter.ConstructId(request.ActionstepOrg, request.MatterId);
                }

                if (wCAConveyancingMatter.SettlementDate != null)
                {
                    pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.SettlementDate = wCAConveyancingMatter.SettlementDate.ToDateTimeUnspecified();
                }

                if (pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.SettlementDate == DateTime.MinValue)
                {
                    pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.SettlementDate = null;
                }

                if (pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.Role == PexaRole.Consentor || pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.Role == PexaRole.CT_Controller)
                {
                    pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.LandTitleDetails.ParentTitle = "No";
                    pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.RequestLandTitleData = "No";
                    pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.FinancialSettlement = "Yes";
                }
                else
                {
                    pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.LandTitleDetails.ParentTitle = "No";
                    pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.RequestLandTitleData = "Yes";
                    pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.FinancialSettlement = "Yes";
                }

                pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.PartyDetails = new Collection<WorkspaceCreationRequestTypePartyDetailsParty>();

                foreach (var buyer in wCAConveyancingMatter.Buyers)
                {
                    var buyerPartyDetails = MapPartyDetails(buyer, "Incoming Proprietor");
                    pexaWorkspaceRequestWithActionstepData.CreatePexaWorkspaceCommand.PexaWorkspaceCreationRequest.PartyDetails.Add(buyerPartyDetails);
                }

                return Task.FromResult(pexaWorkspaceRequestWithActionstepData);
            }
            catch (AggregateException ex)
            {
                if(ex.InnerExceptions.All(e => e is InvalidTokenSetException || e is InvalidCredentialsForActionstepApiCallException))
                {
                    throw ex.InnerExceptions.FirstOrDefault();
                }

                throw ex;
            }
        }

        private WorkspaceCreationRequestTypePartyDetailsParty MapPartyDetails(Party wcaParty, string role)
        {
            var party = new WorkspaceCreationRequestTypePartyDetailsParty
            {
                RepresentingParty = "Yes",
                PartyType = (wcaParty.IdentityType == IdentityType.Individual) ? "Individual" : "Organisation",
                PartyRole = role,
                FullName = (wcaParty.IdentityType == IdentityType.Individual) ? new FullName
                {
                    GivenName = {
                        new GivenNameOrderType {
                            Order = "1",
                            Value = String.IsNullOrEmpty(wcaParty.FirstName) ? wcaParty.CompanyName : wcaParty.FirstName
                        },
                        new GivenNameOrderType
                        {
                            Order = "2",
                            Value = wcaParty.MiddleName
                        }
                    },
                    FamilyName = String.IsNullOrEmpty(wcaParty.LastName) ? String.Empty : wcaParty.LastName,
                    DateOfBirthValueSpecified = wcaParty.DateOfBirth != null,
                    DateOfBirth = wcaParty.DateOfBirth.ToDateTimeUnspecified()
                } : null
            };

            if (!string.IsNullOrEmpty(wcaParty.CompanyName))
            {
                party.Business = new Business
                {
                    BusinessName = wcaParty.CompanyName,
                    Identification = { 
                        //new Anonymous3 { Identifier = Identifier.ACN, Value = buyer.acn } 
                    },
                    LegalEntityName = wcaParty.CompanyName
                };
            }

            (string roadNumber, string roadSuffix, string roadTypeCode, string roadName) = AddressHelper.ParseRoadInfo(wcaParty.AddressLine1, wcaParty.AddressLine2);
            if (string.IsNullOrEmpty(roadNumber)) roadNumber = "NA";

            party.CurrentAddress = new Address2
            {
                CorrespondenceAddress = new Address2CorrespondenceAddressDetailsType
                {
                    PostalDelivery = new Address2PostalDeliveryDetailsType { PostalDeliveryNumber = wcaParty.PostCode, PostalDeliveryTypeCode = "" },
                    Road = new Address2StreetAddressRoadDetailsType
                    {
                        RoadName = roadName,
                        LotNumber = "",
                        RoadNumber = roadNumber,
                        RoadSuffixCode = roadSuffix,
                        RoadTypeCode = roadTypeCode
                    },
                    LocalityName = String.IsNullOrEmpty(wcaParty.City) ? String.Empty : wcaParty.City,
                    Postcode = String.IsNullOrEmpty(wcaParty.PostCode) ? String.Empty : wcaParty.PostCode,
                    State = String.IsNullOrEmpty(wcaParty.StateProvince) ? String.Empty : wcaParty.StateProvince
                },
                StreetAddress = new Address2StreetAddressDetailsType
                {
                    Road = new Address2StreetAddressRoadDetailsType
                    {
                        RoadName = roadName,
                        LotNumber = "",
                        RoadNumber = roadNumber,
                        RoadSuffixCode = roadSuffix,
                        RoadTypeCode = roadTypeCode
                    },
                    LocalityName = String.IsNullOrEmpty(wcaParty.City) ? String.Empty : wcaParty.City,
                    Postcode = String.IsNullOrEmpty(wcaParty.PostCode) ? String.Empty : wcaParty.PostCode,
                    State = String.IsNullOrEmpty(wcaParty.StateProvince) ? String.Empty : wcaParty.StateProvince,
                    AddressSiteName = String.Empty,
                    SubDwellingUnitType = new Address2StreetAddressSubDwellingUnitTypeDetailsType()
                }
            };

            return party;
        }
    }
}
