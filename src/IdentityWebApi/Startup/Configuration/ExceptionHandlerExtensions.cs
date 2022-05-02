using IdentityWebApi.Presentation.Models.Response;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace IdentityWebApi.Startup.Configuration;

public static class ExceptionHandlerExtensions
{
    public static void RegisterExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature is not null)
                {
                    await context.Response.WriteAsJsonAsync(
                        new ErrorResponse(contextFeature.Error.Source, contextFeature.Error.Message)
                    );
                }
            });
        });
    }
}
