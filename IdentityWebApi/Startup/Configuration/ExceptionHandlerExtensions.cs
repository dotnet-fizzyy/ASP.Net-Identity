using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

using System.Net;

namespace IdentityWebApi.Startup.Configuration
{
    public static class ExceptionHandlerExtensions
    {
        public static void RegisterExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature is not null)
                    {
                        await context.Response.WriteAsync($@"
                            {{
                                ""errors"": [
                                    ""code"":""API_server_error"",
                                    ""source"": ""{contextFeature.Error.Source}"",
                                    ""message"":""{contextFeature.Error.Message}""
                                ]
                            }}
                        ");
                    }
                });
            });
        }
    }
}