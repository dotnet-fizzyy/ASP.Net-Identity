using IdentityWebApi.Core.Interfaces.Presentation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using System;

namespace IdentityWebApi.Presentation.Services;

public class HttpContextService : IHttpContextService
{
    private readonly HttpContext _httpContext;
    private readonly LinkGenerator _linkGenerator;
    
    public HttpContextService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _linkGenerator = linkGenerator;
    }

    public string GenerateConfirmEmailLink(string email, string token) => 
        _linkGenerator.GetUriByAction(
            _httpContext,  
            "ConfirmEmail", 
            "Auth",
            new { email, token }, 
            _httpContext.Request.Scheme
        );

    public string GenerateGetUserLink(Guid id) =>
        _linkGenerator.GetUriByAction(
            _httpContext,
            "GetUser", 
            "User", 
            new { id }, 
            _httpContext.Request.Scheme
        );
}