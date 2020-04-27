using System.IO;
using System.Text;
using System.Xml.Serialization;
using WCA.PEXA.Client;
using Xunit;

namespace WCA.UnitTests.PEXAClient
{
    public class PEXASerializationTests
    {
        private const string _sampleXmlWithEnums = @"<?xml version=""1.0"" encoding=""utf-16""?>
<WorkspaceCreationRequest xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://api.pexa.net.au/schema/2/"">
  <LandTitleDetails>
    <LandTitle />
    <ParentTitle>No</ParentTitle>
  </LandTitleDetails>
  <RequestLandTitleData>Yes</RequestLandTitleData>
  <Jurisdiction>NSW</Jurisdiction>
  <Role>Mortgagee on Title</Role>
  <FinancialSettlement>Yes</FinancialSettlement>
  <ParticipantSettlementAcceptanceStatus>Accepted</ParticipantSettlementAcceptanceStatus>
  <PartyDetails>
    <Party>
      <Anonymous12>
        <RepresentingParty>Yes</RepresentingParty>
        <PartyType>Individual</PartyType>
        <PartyRole>Incoming Caveator</PartyRole>
      </Anonymous12>
    </Party>
  </PartyDetails>
</WorkspaceCreationRequest>";

        [Fact]
        public void CanSerializeWithEnums()
        {
            var xmlSerializer = new XmlSerializer(typeof(WorkspaceCreationRequest));

            var stringBuilder = new StringBuilder();

            using (var stringWriter = new StringWriter(stringBuilder))
            {
                var workspaceCreationRequest = new WorkspaceCreationRequest();
                workspaceCreationRequest.Role = PexaRole.Mortgagee_on_Title;
                xmlSerializer.Serialize(stringWriter, workspaceCreationRequest);
            }

            // Assert.Equal(_sampleXmlWithEnums, stringBuilder.ToString());
        }

        [Fact]
        public void CanDeserializeWithEnums()
        {
            var xmlSerializer = new XmlSerializer(typeof(WorkspaceCreationRequest));

            WorkspaceCreationRequest deserializedWorkspaceCreationRequestUnderTest;

            using (var stringReader = new StringReader(_sampleXmlWithEnums))
            {
#pragma warning disable CA5369 // Use XmlReader For Deserialize: It's okay in this unit test as we control the XML input so there is no danger of user supplied XML.
                deserializedWorkspaceCreationRequestUnderTest = (WorkspaceCreationRequest)xmlSerializer.Deserialize(stringReader);
#pragma warning restore CA5369 // Use XmlReader For Deserialize
            }

            //Assert.NotNull(deserializedWorkspaceCreationRequestUnderTest);
            //Assert.Equal(WorkspaceCreationRequestRole.Mortgagee_on_Title, deserializedWorkspaceCreationRequestUnderTest.Role);
            //Assert.Equal(PartyRole2.Incoming_Caveator, deserializedWorkspaceCreationRequestUnderTest.PartyDetails.Party[0].PartyRole);
        }
    }
}
