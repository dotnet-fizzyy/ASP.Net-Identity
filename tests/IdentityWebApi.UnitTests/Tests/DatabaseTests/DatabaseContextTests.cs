using IdentityWebApi.UnitTests.Infrastructure;

using NUnit.Framework;

using System.Threading.Tasks;

namespace IdentityWebApi.UnitTests.Tests.DatabaseTests;

public class DatabaseContextTests : SqliteConfiguration
{
    [Test]
    public async Task ShouldConnectToDbSuccessfully()
    {
        // Arrange & Act
        var result = await this.DatabaseContext.Database.CanConnectAsync();

        // Assert
        Assert.True(result);
    }
}
