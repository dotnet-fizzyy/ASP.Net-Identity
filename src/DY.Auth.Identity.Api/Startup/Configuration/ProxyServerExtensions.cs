using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

namespace DY.Auth.Identity.Api.Startup.Configuration;

/// <summary>
/// Proxy server configuration.
/// </summary>
public static class ProxyServerExtensions
{
    /// <summary>
    /// Registers proxy server headers.
    /// </summary>
    /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
    public static void RegisterProxyServerHeaders(this IApplicationBuilder app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        });
    }
}
