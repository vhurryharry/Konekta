using System.Reflection;
using Xunit.Sdk;

namespace WCA.UnitTests.TestInfrastructure
{
    public class ResetDatabaseAttribute : BeforeAfterTestAttribute
    {
        private readonly WebContainerFixture webContainerFixture;

        public ResetDatabaseAttribute(WebContainerFixture webContainerFixture)
        {
            this.webContainerFixture = webContainerFixture;
        }

        public override void Before(MethodInfo methodUnderTest)
        {
            webContainerFixture.ResetCheckpoint();
        }
    }
}
