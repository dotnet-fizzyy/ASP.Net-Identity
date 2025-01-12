using DY.Auth.Identity.Api.Core.Interfaces.Infrastructure;
using DY.Auth.Identity.Api.Core.Models;
using DY.Auth.Identity.Api.Presentation.Filters;
using DY.Auth.Identity.Api.Presentation.Models.Response;
using DY.Auth.Identity.Api.Startup.ApplicationSettings;
using DY.Auth.Identity.Api.UnitTests.Shared.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

using Moq;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.UnitTests.Tests.Filters;

[TestFixture]
public class RegionVerificationFilterTests
{
    private const string ProhibitedRegion = "testrg";

    private readonly Mock<ConnectionInfoMock> connectionInfoMock = new ();
    private readonly Mock<HttpContext> httpContextMock = new ();
    private readonly Mock<IRegionVerificationService> regionVerificationServiceMock = new ();

    [TearDown]
    public void TearDown()
    {
        this.connectionInfoMock.Reset();
        this.httpContextMock.Reset();
        this.regionVerificationServiceMock.Reset();
    }

    [Test]
    [Category("Positive")]
    public async Task ShouldValidateRegionAndContinueIfItIsNotProhibited()
    {
        // Arrange
        const string ipAddress = "8.8.8.8";

        this.connectionInfoMock
            .Setup(connectionInfo => connectionInfo.RemoteIpAddress)
            .Returns(IPAddress.Parse(ipAddress))
            .Verifiable();

        this.httpContextMock
            .Setup(context => context.Connection)
            .Returns(this.connectionInfoMock.Object)
            .Verifiable();

        var actContext = GetActionContext(this.httpContextMock.Object);
        var actExecutingContext = GetActionExecutingContext(actContext);

        var actionExecutionDelegate = Mock.Of<ActionExecutionDelegate>();

        var ipAddressDetails = new IpAddressDetails
        {
            RegionName = "testrg2",
            CountryCode = "testrg2",
            CountryName = "CountryName",
            City = "City",
        };

        this.regionVerificationServiceMock
            .Setup(service => service.GetIpAddressDetailsAsync(It.IsAny<string>()))
            .ReturnsAsync(ipAddressDetails)
            .Verifiable();

        var appSettings = new AppSettings
        {
            RegionsVerificationSettings = new RegionsVerificationSettings
            {
                EnableVerification = true,
                ProhibitedRegions = new List<string> { ProhibitedRegion },
            },
        };

        var regionVerificationFilter = new RegionVerificationFilter(this.regionVerificationServiceMock.Object, appSettings);

        // Act
        await regionVerificationFilter.OnActionExecutionAsync(actExecutingContext, actionExecutionDelegate);

        // Assert
        ClassicAssert.Null(actExecutingContext.Result);

        this.regionVerificationServiceMock.Verify();
    }

    [Test]
    [Category("Negative")]
    public async Task ShouldReturnBadRequestIfIpAddressIsNotRecognized()
    {
        // Arrange;
        this.connectionInfoMock
            .Setup(connectionInfo => connectionInfo.RemoteIpAddress)
            .Returns((IPAddress)null)
            .Verifiable();

        this.httpContextMock
            .Setup(context => context.Connection)
            .Returns(this.connectionInfoMock.Object)
            .Verifiable();

        var actContext = GetActionContext(this.httpContextMock.Object);
        var actExecutingContext = GetActionExecutingContext(actContext);

        var actionExecutionDelegate = Mock.Of<ActionExecutionDelegate>();

        var appSettings = new AppSettings
        {
            RegionsVerificationSettings = new RegionsVerificationSettings
            {
                EnableVerification = true,
                ProhibitedRegions = new List<string> { "test" },
            },
        };

        var regionVerificationFilter = new RegionVerificationFilter(this.regionVerificationServiceMock.Object, appSettings);

        // Act
        await regionVerificationFilter.OnActionExecutionAsync(actExecutingContext, actionExecutionDelegate);

        // Assert
        ClassicAssert.AreEqual(typeof(BadRequestObjectResult), actExecutingContext.Result.GetType());
    }

    [Test]
    [Category("Negative")]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(ProhibitedRegion)]
    public async Task ShouldReturnForbiddenIfRegionIsProhibitedOrNotRecognized(string countryCode)
    {
        // Arrange
        const string ipAddress = "8.8.8.8";

        this.connectionInfoMock
            .Setup(connectionInfo => connectionInfo.RemoteIpAddress)
            .Returns(IPAddress.Parse(ipAddress))
            .Verifiable();

        this.httpContextMock
            .Setup(context => context.Connection)
            .Returns(this.connectionInfoMock.Object)
            .Verifiable();

        var actContext = GetActionContext(this.httpContextMock.Object);
        var actExecutingContext = GetActionExecutingContext(actContext);

        var actionExecutionDelegate = Mock.Of<ActionExecutionDelegate>();

        var ipAddressDetails = new IpAddressDetails
        {
            RegionName = "testrg2",
            CountryCode = countryCode,
            CountryName = "CountryName",
            City = "City",
        };

        this.regionVerificationServiceMock
            .Setup(service => service.GetIpAddressDetailsAsync(It.IsAny<string>()))
            .ReturnsAsync(ipAddressDetails)
            .Verifiable();

        var appSettings = new AppSettings
        {
            RegionsVerificationSettings = new RegionsVerificationSettings
            {
                EnableVerification = true,
                ProhibitedRegions = new List<string> { ProhibitedRegion },
            },
        };

        var regionVerificationFilter = new RegionVerificationFilter(this.regionVerificationServiceMock.Object, appSettings);

        // Act
        await regionVerificationFilter.OnActionExecutionAsync(actExecutingContext, actionExecutionDelegate);

        // Assert
        ClassicAssert.AreEqual(typeof(ForbiddenObjectResult), actExecutingContext.Result.GetType());

        this.regionVerificationServiceMock.Verify();
    }

    private static ActionContext GetActionContext(HttpContext httpContext) =>
        new ActionContext(
              httpContext,
              Mock.Of<RouteData>(),
              Mock.Of<ActionDescriptor>(),
              Mock.Of<ModelStateDictionary>());

    private static ActionExecutingContext GetActionExecutingContext(ActionContext actContext) =>
        new ActionExecutingContext(
              actContext,
              new List<IFilterMetadata>(),
              new Dictionary<string, object>(),
              Mock.Of<Controller>());
}
