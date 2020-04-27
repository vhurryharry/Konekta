using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Domain.Conveyancing;

namespace WCA.Core.Features.Conveyancing.Services
{
    public class ActionstepToWCAMapper : IActionstepToWCAMapper
    {
        public ConveyancingMatter MapFromActionstepTypes(
            GetActionResponse actionResponse, 
            ListActionParticipantsResponse participantsResponse, 
            ListDataCollectionRecordValuesResponse dataCollectionsResponse)
        {
            if (actionResponse is null) throw new ArgumentNullException(nameof(actionResponse));

            var wCAConveyancingMatter = new ConveyancingMatter();

            var action = actionResponse.Action;

            wCAConveyancingMatter.Id = action.Id;
            wCAConveyancingMatter.Name = action.Name;
            wCAConveyancingMatter.ActionType = actionResponse.ActionTypeName;
            wCAConveyancingMatter.FileReference = action.Reference;
            wCAConveyancingMatter.Conveyancers.AddRange(GetParticipants(participantsResponse, "Conveyancer"));
            wCAConveyancingMatter.Buyers.AddRange(GetParticipants(participantsResponse, "Buyer"));
            wCAConveyancingMatter.IncomingBanks.AddRange(GetParticipants(participantsResponse, "Bank_Incoming"));
            wCAConveyancingMatter.OthersideSolicitor.AddRange(GetParticipants(participantsResponse, "Otherside_Solicitor"));
            wCAConveyancingMatter.OthersideSolicitorPrimaryContact.AddRange(GetParticipants(participantsResponse, "Otherside_Solicitor_Primary_Contact"));

            wCAConveyancingMatter.PropertyDetails = new PropertyDetails
            {
                TitleReference = dataCollectionsResponse?["property", "titleref"],
                LotNo = dataCollectionsResponse["property", "lotno"]
            };

            ConveyancingType conveyancingType;
            var isConveyancingTypeParseSuccess = Enum.TryParse<ConveyancingType>(dataCollectionsResponse["convdet", "ConveyType"], out conveyancingType);
            if (!isConveyancingTypeParseSuccess) conveyancingType = ConveyancingType.None;
            wCAConveyancingMatter.ConveyancingType = conveyancingType;

            var stringSettlementDate = dataCollectionsResponse["keydates", "smtdateonly"];
            if (!String.IsNullOrEmpty(stringSettlementDate))
            {
                var pattern = LocalDatePattern.Create("yyyy-MM-dd", CultureInfo.InvariantCulture);
                wCAConveyancingMatter.SettlementDate = pattern.Parse(stringSettlementDate).Value;
            }

            wCAConveyancingMatter.SettlementBookingTime = dataCollectionsResponse["convdet", "smttime"];

            return wCAConveyancingMatter;
        }

        private List<Party> GetParticipants(ListActionParticipantsResponse response, string participantTypeName)
        {
            var listOfParties = new List<Party>();

            if (response is null)
            {
                return listOfParties;
            }

            var participantType = response.Linked.ParticipantTypes.SingleOrDefault(t => t.Name == participantTypeName);
            if (participantType == null) return listOfParties;

            var actionParticipants = response.ActionParticipants.Where(p => p.Links.ParticipantType == participantType.Id.ToString(CultureInfo.InvariantCulture));
            foreach (var actionParticipant in actionParticipants)
            {
                var participant = response.Linked.Participants.SingleOrDefault(p => p.Id.ToString(CultureInfo.InvariantCulture) == actionParticipant.Links.Participant);
                if (participant == null) continue;

                var party = new Party
                {
                    IdentityType = participant.IsCompany ? IdentityType.Company : IdentityType.Individual,
                    FirstName = participant.FirstName,
                    MiddleName = participant.MiddleName,
                    LastName = participant.LastName,
                    PreferredName = participant.PreferredName,
                    CompanyName = participant.CompanyName,
                    DateOfBirth = participant.BirthTimestamp == null ? NodaTime.LocalDate.FromDateTime(DateTime.MinValue) : participant.BirthTimestamp.Value,
                    EmailAddress = participant.Email,
                    IsDeceased = (participant.DeathTimestamp != null),
                    AddressLine1 = participant.PhysicalAddressLine1,
                    AddressLine2 = participant.PhysicalAddressLine2,
                    City = participant.PhysicalCity,
                    PostCode = participant.PhysicalPostCode,
                    StateProvince = participant.PhysicalStateProvince
                };

                listOfParties.Add(party);
            }

            return listOfParties;
        }
    }
}
