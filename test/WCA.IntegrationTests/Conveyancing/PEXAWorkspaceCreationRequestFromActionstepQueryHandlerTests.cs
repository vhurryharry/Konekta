using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using WCA.Core.Features.Conveyancing.WorkspaceCreation;
using WCA.Data;
using WCA.IntegrationTests.TestInfrastructure;
using Xunit;

namespace WCA.IntegrationTests.Conveyancing
{
    [Collection(WebContainerCollection.WebContainerCollectionName)]
    public class PEXAWorkspaceCreationRequestFromActionstepQueryHandlerTests
    {
        private readonly WebContainerFixture _containerFixture;

        public PEXAWorkspaceCreationRequestFromActionstepQueryHandlerTests(WebContainerFixture containerFixture)
        {
            _containerFixture = containerFixture;
        }

        [Fact]
        public async void HandleMappingTest()
        {
            await _containerFixture.ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();
                var db = sp.GetService<WCADbContext>();

                var user = db.Users.First();
                var query = new PEXAWorkspaceCreationRequestFromActionstepQuery
                {
                    AuthenticatedUser = user,
                    MatterId = 1,
                    ActionstepOrg = "testOrg1"
                };

                return Task.CompletedTask;
            });
        }
    }
}
