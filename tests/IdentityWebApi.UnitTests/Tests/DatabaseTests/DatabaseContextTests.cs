using IdentityWebApi.UnitTests.Infrastructure;

using NUnit.Framework;

using System.Threading.Tasks;

namespace IdentityWebApi.UnitTests.Tests.DatabaseTests;

public class DatabaseContextTests : SqliteConfiguration
{
    [Test]
    public async Task ShouldConnectToDbSuccessfully()
    {
        Assert.True(await this.DatabaseContext.Database.CanConnectAsync());
    }
}
