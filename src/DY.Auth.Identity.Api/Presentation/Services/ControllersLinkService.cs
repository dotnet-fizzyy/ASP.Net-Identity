using DY.Auth.Identity.Api.Core.Interfaces.Presentation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using System;

namespace DY.Auth.Identity.Api.Presentation.Services;

/// <inheritdoc />
/// <summary>
/// Initializes a new instance of the <see cref="ControllersLinkService"/> class.
/// </summary>
/// <param name="httpContextAccessor">The instance of <see cref="HttpContextAccessor"/>.</param>
/// <param name="linkGenerator">The instance of <see cref="LinkGenerator"/>.</param>
public class ControllersLinkService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator) : IControllersLinkService
{
    private readonly HttpContext httpContext = httpContextAccessor?.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    private readonly LinkGenerator linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));

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

    /// <inheritdoc/>
    public string GenerateEmailTemplateLink(Guid id) =>
        this.linkGenerator.GetUriByAction(
            this.httpContext,
            "GetEmailTemplate",
            "EmailTemplate",
            new { id },
            this.httpContext.Request.Scheme);
}
