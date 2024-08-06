using DY.Auth.Identity.Api.Core.Interfaces.Infrastructure;
using DY.Auth.Identity.Api.Presentation.Models.Response;
using DY.Auth.Identity.Api.Startup.ApplicationSettings;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Presentation.Filters;

/// <summary>
/// Filter of allowed regions from HTTP requests.
/// </summary>
public class RegionVerificationFilter : IAsyncActionFilter
{
    private readonly INetService netService;
    private readonly AppSettings appSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegionVerificationFilter"/> class.
    /// </summary>
    /// <param name="netService">INetService.</param>
    /// <param name="appSettings">AppSettings.</param>
    public RegionVerificationFilter(INetService netService, AppSettings appSettings)
    {
        this.appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        this.netService = netService ?? throw new ArgumentNullException(nameof(netService));
    }

    /// <inheritdoc/>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (this.appSettings.RegionsVerificationSettings.AllowVerification)
        {
            var userIpV4 = GetIpV4AddressFromExecutingContext(context);

            if (userIpV4 == null)
            {
                CreateBadRequestResponseResult(context, "Cannot identify request IP address");

                return;
            }

            var ipAddressDetails = await this.netService.GetIpAddressDetails(userIpV4);

            var isCountryCodeMissing = string.IsNullOrEmpty(ipAddressDetails.CountryCode);
            var isRequestRegionProhibited = this.IsRegionProhibitedInSettings(ipAddressDetails.CountryCode);

            if (isCountryCodeMissing || isRequestRegionProhibited)
            {
                CreateForbiddenResponseResult(context, "IP address region is not supported");

                return;
            }
        }

        await next();
    }

    private bool IsRegionProhibitedInSettings(string regionCode) =>
        this.appSettings.RegionsVerificationSettings.ProhibitedRegions.Any(region =>
            string.Equals(region, regionCode, StringComparison.OrdinalIgnoreCase));

    private static string GetIpV4AddressFromExecutingContext(ActionExecutingContext context) =>
        context.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

    private static void CreateBadRequestResponseResult(ActionExecutingContext context, string message) =>
        context.Result = new BadRequestObjectResult(new ErrorResponse(message));

    private static void CreateForbiddenResponseResult(ActionExecutingContext context, string message) =>
        context.Result = new ForbiddenObjectResult(new ErrorResponse(message));
}
