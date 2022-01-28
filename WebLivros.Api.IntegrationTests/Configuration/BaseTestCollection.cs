using Xunit;

namespace WebLivros.Api.IntegrationTests.Configuration
{
    [CollectionDefinition("Base Collection")]
    public class BaseTestCollection : ICollectionFixture<BaseTestFixture>
    {
    }
}
