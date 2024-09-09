using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Core.Interfaces.Presentation;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Presentation.Services;

/// <inheritdoc />
public class HttpContextService : IHttpContextService
{
    private readonly HttpContext httpContext;
    private readonly LinkGenerator linkGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpContextService"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The instance of <see cref="HttpContextAccessor"/>.</param>
    /// <param name="linkGenerator">The instance of <see cref="LinkGenerator"/>.</param>
    public HttpContextService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
    {
        this.httpContext = httpContextAccessor?.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
    }

    /// <inheritdoc/>
    public Task SignInUsingCookiesAsync(ClaimsPrincipal user) =>
        this.httpContext.SignInAsync(AuthConstants.CookiesAuthScheme, user);

    /// <inheritdoc/>
    public string GenerateConfirmEmailLink(string email, string token) =>
        this.linkGenerator.GetUriByAction(
            this.httpContext,
            "ConfirmEmail",
            "Auth",
            new { email, token },
            this.httpContext.Request.Scheme);

    /// <inheritdoc/>
    public string GenerateGetUserLink(Guid id) =>
        this.linkGenerator.GetUriByAction(
            this.httpContext,
            "GetUserById",
            "User",
            new { id },
            this.httpContext.Request.Scheme);

    /// <inheritdoc/>
    public string GenerateGetRoleLink(Guid id) =>
        this.linkGenerator.GetUriByAction(
            this.httpContext,
            "GetRoleById",
            "Role",
            new { id },
            this.httpContext.Request.Scheme);
}
