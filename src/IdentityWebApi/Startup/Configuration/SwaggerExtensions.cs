using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace IdentityWebApi.Startup.Configuration;

public static class SwaggerExtensions
{
    public static void RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "IdentityWebApi",
                Version = "v1"
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            
            c.IncludeXmlComments(xmlPath);
        });
    }

    public static void UseSwaggerApp(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityWebApi v1"));
    }

    private static string CreateXmlFile()
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlFileContent = "<?xml version=\"1.0\"?><doc></doc>";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        var startedFileCreation = false;
        
        while (!File.Exists(xmlPath))
        {
            if (!startedFileCreation)
            {
                using (FileStream fs = File.Create(xmlPath))
                {
                    var info = new UTF8Encoding(true).GetBytes(xmlFileContent);
                    
                    fs.Write(info, 0, info.Length);
                }
            }
            
            Thread.Sleep(TimeSpan.FromSeconds(1));
            
            startedFileCreation = true;
        }
        
        return xmlPath;
    }
}
