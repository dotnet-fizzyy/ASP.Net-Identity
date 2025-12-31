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
    private readonly IRegionVerificationService regionVerificationService;
    private readonly AppSettings appSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegionVerificationFilter"/> class.
    /// </summary>
    /// <param name="regionVerificationService">The instance of <see cref="IRegionVerificationService"/>.</param>
    /// <param name="appSettings">The instance of <see cref="AppSettings"/>.</param>
    public RegionVerificationFilter(IRegionVerificationService regionVerificationService, AppSettings appSettings)
    {
        this.appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        this.regionVerificationService = regionVerificationService ?? throw new ArgumentNullException(nameof(regionVerificationService));
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

            var ipAddressDetails = await this.regionVerificationService.GetIpAddressDetailsAsync(userIpV4);

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

    private static void CreateBadRequestResponseResult(ActionExecutingContext context, string message)
    {
        var errorResponse = new ErrorResponse
        {
            Message = message,
        };

        context.Result = new BadRequestObjectResult(errorResponse);
    }

    private static void CreateForbiddenResponseResult(ActionExecutingContext context, string message)
    {
        var errorResponse = new ErrorResponse
        {
            Message = message,
        };

        context.Result = new ForbiddenObjectResult(errorResponse);
    }
}
