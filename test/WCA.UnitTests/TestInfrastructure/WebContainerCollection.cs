using Xunit;

namespace WCA.UnitTests.TestInfrastructure
{
    [CollectionDefinition(WebContainerCollectionName)]
    public class WebContainerCollection : ICollectionFixture<WebContainerFixture>
    {
        public const string WebContainerCollectionName = "Web Container Collection";

        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
