using IdentityWebApi.Startup;
using IdentityWebApi.Startup.Configuration;

using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.ReadAppSettings();
appSettings.Validate();

builder.Services.Configure(appSettings);

var app = builder.Build();

app.Configure(appSettings);

app.Run();
