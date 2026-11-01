using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Core.Interfaces.Presentation;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Presentation.Services;

/// <inheritdoc />
public class CookiesService : ICookiesService
{
    private readonly HttpContext httpContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CookiesService"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The instance of <see cref="HttpContextAccessor"/>.</param>
    public CookiesService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContext = httpContextAccessor?.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <inheritdoc/>
    public Task SignInUsingCookiesAsync(ClaimsPrincipal user) =>
        this.httpContext.SignInAsync(AuthConstants.CookiesAuthScheme, user);
}
