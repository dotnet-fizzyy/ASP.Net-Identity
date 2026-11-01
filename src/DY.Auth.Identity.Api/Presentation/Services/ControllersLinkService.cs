using DY.Auth.Identity.Api.Core.Interfaces.Presentation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using System;

namespace DY.Auth.Identity.Api.Presentation.Services;

/// <inheritdoc />
public class ControllersLinkService : IControllersLinkService
{
    private readonly HttpContext httpContext;
    private readonly LinkGenerator linkGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ControllersLinkService"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The instance of <see cref="HttpContextAccessor"/>.</param>
    /// <param name="linkGenerator">The instance of <see cref="LinkGenerator"/>.</param>
    public ControllersLinkService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
    {
        this.httpContext = httpContextAccessor?.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
    }

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
