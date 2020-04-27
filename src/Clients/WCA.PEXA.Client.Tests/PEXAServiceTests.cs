using FakeItEasy;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WCA.PEXA.Client.Resources;
using WCA.PEXA.Client.Tests.Infrastructure;
using Xunit;

namespace WCA.PEXA.Client.Tests
{
    public class PEXAServiceTests
    {
        private readonly IConfigurationRoot _config;

        enum TestEnum
        {
            [XmlEnum(Name = "Value With Xml Enum Attribute")]
            ValueWithXmlEnumAttribute,
            ValueWithoutXmlEnumAttribute
        }

        public PEXAServiceTests()
        {
            var config = new ConfigurationBuilder();
            config.AddInMemoryCollection(new Dictionary<string, string>() {
                    { "WCACoreSettings:PEXASettings:Environment", "Test" }
            });

            _config = config.Build();
        }

        [Fact]
        public async Task ShouldCreatePexaWorkspaceResponse()
        {
            using (var _httpClient = new PEXAMockHttpClient(A.Fake<PEXAMockClientHandler>(opt => opt.CallsBaseMethods())))
            {
                var _iPEXAService = new PEXAService(_httpClient, _config);

                var response = await _iPEXAService.Handle<WorkspaceCreationResponse>(
                    new WorkspaceCreationRequestCommand(new WorkspaceCreationRequest(), "testBearerToken"), CancellationToken.None).ConfigureAwait(true);

                Assert.Equal("PEXA190167645", response.WorkspaceId);
                Assert.Equal("In Preparation", response.WorkspaceStatus);
            }
        }

        [Fact]
        public void CanGetXmlEnumName()
        {
            var result = PEXAService.TryGetXmlName(TestEnum.ValueWithXmlEnumAttribute);
            Assert.Equal("Value With Xml Enum Attribute", result);
        }

        [Fact]
        public void GetXmlNameWithoutAttributeGetsEnumValue()
        {
            var result = PEXAService.TryGetXmlName(TestEnum.ValueWithoutXmlEnumAttribute);
            Assert.Equal("ValueWithoutXmlEnumAttribute", result);
        }

        [Fact]
        public void CanGetTestWorkspaceUri()
        {
            var config = new ConfigurationBuilder();
            config.AddInMemoryCollection(new Dictionary<string, string>() {
                    { "WCACoreSettings:PEXASettings:Environment", "Test" }
            });

            var pexaService = new PEXAService(null, config.Build());
            var result = pexaService.GetWorkspaceUri("abc 123", PexaRole.Incoming_Mortgagee);
            Assert.Equal("https://api-tst.pexalabs.com.au/pexa_web/dl/workspaces/abc%20123?role=Incoming%20Mortgagee", result.AbsoluteUri);
        }

        [Fact]
        public void CanGetProductionWorkspaceUri()
        {
            var config = new ConfigurationBuilder();
            config.AddInMemoryCollection(new Dictionary<string, string>() {
                    { "WCACoreSettings:PEXASettings:Environment", "Production" }
            });

            var pexaService = new PEXAService(null, config.Build());
            var result = pexaService.GetWorkspaceUri("abc 123", PexaRole.Incoming_Mortgagee);
            Assert.Equal("https://api.pexa.com.au/pexa_web/dl/workspaces/abc%20123?role=Incoming%20Mortgagee", result.AbsoluteUri);
        }

        [Fact]
        public async Task ShouldCaptureExceptionResponse()
        {
            try
            {
                using (var _httpClient = new PEXAMockHttpClient(A.Fake<PEXAMockClientHandler>(opt => opt.CallsBaseMethods())))
                {
                    var _iPEXAService = new PEXAService(_httpClient, _config);

                    _httpClient.MockHandler.ReturnException = true;
                    var response = await _iPEXAService.Handle<WorkspaceCreationResponse>(
                        new WorkspaceCreationRequestCommand(new WorkspaceCreationRequest(), "testBearerToken"), CancellationToken.None).ConfigureAwait(true);
                }
            }
            catch (PEXAException ex)
            {
                var exceptionResponse = ex.ExceptionResponse;
                Assert.Equal("400001", exceptionResponse.ExceptionList.ElementAt(0).Code);
                Assert.Equal("The message has failed schema validation", exceptionResponse.ExceptionList.ElementAt(0).Message);
                Assert.Equal("OB1409.002", exceptionResponse.ExceptionList.ElementAt(1).Code);
                Assert.Equal("The workgroup cannot be found", exceptionResponse.ExceptionList.ElementAt(1).Message);
            }
        }
    }
}
