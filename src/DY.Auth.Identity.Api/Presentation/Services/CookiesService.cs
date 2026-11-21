using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Core.Interfaces.Presentation;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Presentation.Services;

/// <summary>
/// Cookies service.
/// </summary>
/// <param name="httpContextAccessor">The instance of <see cref="HttpContextAccessor"/>.</param>
public class CookiesService(IHttpContextAccessor httpContextAccessor) : ICookiesService
{
    private readonly HttpContext httpContext = httpContextAccessor?.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    /// <inheritdoc/>
    public Task SignInUsingCookiesAsync(ClaimsPrincipal user) =>
        this.httpContext.SignInAsync(AuthConstants.CookiesAuthScheme, user);
}
