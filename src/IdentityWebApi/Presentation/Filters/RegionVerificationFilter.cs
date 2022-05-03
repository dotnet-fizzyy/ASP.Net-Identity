using IdentityWebApi.Core.ApplicationSettings;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Presentation.Models.Response;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWebApi.Presentation.Filters;

public class RegionVerificationFilter : IAsyncActionFilter
{
    private readonly INetService _netService;
    private readonly AppSettings _appSettings;

    public RegionVerificationFilter(INetService netService, AppSettings appSettings)
    {
        _appSettings = appSettings;
        _netService = netService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (_appSettings.RegionsVerificationSettings.AllowVerification)
        {
            var userIpV4 = GetIpV4AddressFromExecutingContext(context);

            if (userIpV4 == null)
            {
                CreateBadRequestResponseResult(context, "Cannot identify request IP address");

                return;
            }

            var ipAddressDetails = await _netService.GetIpAddressDetails(userIpV4);

            var isCountryCodeMissing = string.IsNullOrEmpty(ipAddressDetails.CountryCode);
            var isRequestRegionProhibited = _appSettings.RegionsVerificationSettings.ProhibitedRegions
                .Any(reg =>
                    string.Equals(reg, ipAddressDetails.CountryCode, StringComparison.OrdinalIgnoreCase)
                );

            if (isCountryCodeMissing || isRequestRegionProhibited)
            {
                CreateForbiddenResponseResult(context, "IP address region is not supported");

                return;
            }
        }

        await next();
    }

    private static string GetIpV4AddressFromExecutingContext(ActionExecutingContext context) =>
        context.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

    private static void CreateBadRequestResponseResult(ActionExecutingContext context, string message) =>
        context.Result = new BadRequestObjectResult(new ErrorResponse(message));

    private static void CreateForbiddenResponseResult(ActionExecutingContext context, string message) =>
        context.Result = new ForbiddenObjectResult(new ErrorResponse(message));
}