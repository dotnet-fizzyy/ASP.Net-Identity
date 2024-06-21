using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Models;
using IdentityWebApi.Presentation.Filters;
using IdentityWebApi.Presentation.Models.Response;
using IdentityWebApi.Startup.ApplicationSettings;
using IdentityWebApi.UnitTests.Shared.Controllers;

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

namespace IdentityWebApi.UnitTests.Tests.Filters;

[TestFixture]
public class RegionVerificationFilterTests
{
    private const string ProhibitedRegion = "testrg";

    [Test]
    [Category("Positive")]
    public async Task ShouldValidateRegionAndContinueIfItIsNotProhibited()
    {
        // Arrange
        const string ipAddress = "8.8.8.8";

        var connectionInfoMock = Mock.Of<ConnectionInfoMock>();
        Mock.Get(connectionInfoMock)
            .Setup(connectionInfo => connectionInfo.RemoteIpAddress)
            .Returns(IPAddress.Parse(ipAddress))
            .Verifiable();

        var httpContext = Mock.Of<HttpContext>();
        Mock.Get(httpContext)
            .Setup(context => context.Connection)
            .Returns(connectionInfoMock)
            .Verifiable();

        var actContext = GetActionContext(httpContext);
        var actExecutingContext = GetActionExecutingContext(actContext);

        var actionExecutionDelegate = Mock.Of<ActionExecutionDelegate>();

        var netService = Mock.Of<INetService>();

        var ipAddressDetails = new IpAddressDetails
        {
            RegionName = "testrg2",
            CountryCode = "testrg2",
            CountryName = "CountryName",
            City = "City",
        };

        Mock.Get(netService)
            .Setup(service => service.GetIpAddressDetails(It.IsAny<string>()))
            .ReturnsAsync(ipAddressDetails)
            .Verifiable();

        var appSettings = new AppSettings
        {
            RegionsVerificationSettings = new RegionsVerificationSettings
            {
                EnableVerification = true,
                ProhibitedRegions = new List<string> { ProhibitedRegion },
            }
        };

        var regionVerificationFilter = new RegionVerificationFilter(netService, appSettings);

        // Act
        await regionVerificationFilter.OnActionExecutionAsync(actExecutingContext, actionExecutionDelegate);

        // Assert
        ClassicAssert.Null(actExecutingContext.Result);

        Mock.Get(netService)
            .Verify(service => service.GetIpAddressDetails(It.IsAny<string>()), Times.Once);
    }

    [Test]
    [Category("Negative")]
    public async Task ShouldReturnBadRequestIfIpAddressIsNotRecognized()
    {
        // Arrange
        var connectionInfoMock = Mock.Of<ConnectionInfoMock>();
        Mock.Get(connectionInfoMock)
            .Setup(connectionInfo => connectionInfo.RemoteIpAddress)
            .Returns((IPAddress)null)
            .Verifiable();

        var httpContext = Mock.Of<HttpContext>();
        Mock.Get(httpContext)
            .Setup(context => context.Connection)
            .Returns(connectionInfoMock)
            .Verifiable();

        var actContext = GetActionContext(httpContext);
        var actExecutingContext = GetActionExecutingContext(actContext);

        var actionExecutionDelegate = Mock.Of<ActionExecutionDelegate>();

        var netService = Mock.Of<INetService>();

        var appSettings = new AppSettings
        {
            RegionsVerificationSettings = new RegionsVerificationSettings
            {
                EnableVerification = true,
                ProhibitedRegions = new List<string> { "test" },
            }
        };

        var regionVerificationFilter = new RegionVerificationFilter(netService, appSettings);

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

        var connectionInfoMock = Mock.Of<ConnectionInfoMock>();
        Mock.Get(connectionInfoMock)
            .Setup(connectionInfo => connectionInfo.RemoteIpAddress)
            .Returns(IPAddress.Parse(ipAddress))
            .Verifiable();

        var httpContext = Mock.Of<HttpContext>();
        Mock.Get(httpContext)
            .Setup(context => context.Connection)
            .Returns(connectionInfoMock)
            .Verifiable();

        var actContext = GetActionContext(httpContext);
        var actExecutingContext = GetActionExecutingContext(actContext);

        var actionExecutionDelegate = Mock.Of<ActionExecutionDelegate>();

        var netService = Mock.Of<INetService>();

        var ipAddressDetails = new IpAddressDetails
        {
            RegionName = "testrg2",
            CountryCode = countryCode,
            CountryName = "CountryName",
            City = "City",
        };

        Mock.Get(netService)
            .Setup(service => service.GetIpAddressDetails(It.IsAny<string>()))
            .ReturnsAsync(ipAddressDetails)
            .Verifiable();

        var appSettings = new AppSettings
        {
            RegionsVerificationSettings = new RegionsVerificationSettings
            {
                EnableVerification = true,
                ProhibitedRegions = new List<string> { ProhibitedRegion },
            }
        };

        var regionVerificationFilter = new RegionVerificationFilter(netService, appSettings);

        // Act
        await regionVerificationFilter.OnActionExecutionAsync(actExecutingContext, actionExecutionDelegate);

        // Assert
        ClassicAssert.AreEqual(typeof(ForbiddenObjectResult), actExecutingContext.Result.GetType());

        Mock.Get(netService)
            .Verify(service => service.GetIpAddressDetails(It.IsAny<string>()), Times.Once);
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
