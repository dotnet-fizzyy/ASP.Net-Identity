using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Mapping;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Infrastructure.Network.Services;
using DY.Auth.Identity.Api.Startup.ApplicationSettings;
using DY.Auth.Identity.Api.UnitTests.Shared.Mocks;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.UnitTests.Tests.Services;

/// <summary>
/// Unit tests for <see cref="RegionVerificationService"/>.
/// </summary>
[TestFixture]
public class RegionVerificationServiceTests
{
    private const string FakeIpAddress = "127.0.0.1";

    private static readonly AppSettings AppSettings = new ()
    {
        IpStackSettings = new ()
        {
            AccessKey = "TestAccessKey",
        },
    };

    private readonly IMapper mapper = GetMapper();

    /// <summary>
    /// Tests <see cref="RegionVerificationService.GetIpAddressDetailsAsync"/> with successful execution.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Category("Positive")]
    public async Task GetIpAddressDetailsAsync_ShouldExecuteSuccessfully()
    {
        // Arrange
        var clientFactoryMock = new HttpClientFactoryBuilder()
            .WithClient(InternalApi.RegionVerification)
            .WithResponseFromFile(HttpStatusCode.OK, Path.Combine("RegionVerificationService", "GetIpAddressDetailsAsync.Success.json"))
            .GetResult();

        var service = this.GetService(clientFactoryMock.Object);

        // Act
        var result = await service.GetIpAddressDetailsAsync(FakeIpAddress);

        // Assert
        ClassicAssert.NotNull(result);

        clientFactoryMock.Verify();
    }

    /// <summary>
    /// Tests <see cref="RegionVerificationService.GetIpAddressDetailsAsync"/> with <see cref="HttpRequestException"/> exception throwing on bad response.
    /// </summary>
    [Test]
    [Category("Negative")]
    public void GetIpAddressDetailsAsync_ShouldThrowExceptionOnBadResponse()
    {
        // Arrange
        var clientFactoryMock = new HttpClientFactoryBuilder()
            .WithClient(InternalApi.RegionVerification)
            .WithResponse(HttpStatusCode.Unauthorized)
            .GetResult();

        var service = this.GetService(clientFactoryMock.Object);

        // Act & Assert
        ClassicAssert.ThrowsAsync<HttpRequestException>(() => service.GetIpAddressDetailsAsync(FakeIpAddress));
    }

    private static IMapper GetMapper() =>
        new MapperConfiguration(opt => opt.AddProfile(new RegionVerificationServiceProfile()))
            .CreateMapper();

    private RegionVerificationService GetService(IHttpClientFactory httpClientFactory) =>
        new (
            httpClientFactory,
            this.mapper,
            AppSettings);
}
