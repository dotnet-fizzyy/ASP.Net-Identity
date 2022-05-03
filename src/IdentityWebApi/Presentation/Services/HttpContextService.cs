using IdentityWebApi.Core.Interfaces.Presentation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using System;

namespace IdentityWebApi.Presentation.Services;

public class HttpContextService : IHttpContextService
{
    private readonly HttpContext httpContext;
    private readonly LinkGenerator linkGenerator;

    public HttpContextService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
    {
        this.httpContext = httpContextAccessor.HttpContext;
        this.linkGenerator = linkGenerator;
    }

    public string GenerateConfirmEmailLink(string email, string token) =>
        this.linkGenerator.GetUriByAction(
            this.httpContext,
            "ConfirmEmail",
            "Auth",
            new { email, token },
            this.httpContext.Request.Scheme
        );

    public string GenerateGetUserLink(Guid id) =>
        this.linkGenerator.GetUriByAction(
            this.httpContext,
            "GetUser",
            "User",
            new { id },
            this.httpContext.Request.Scheme
        );
}