using IdentityWebApi.Startup;
using IdentityWebApi.Startup.Configuration;

using Microsoft.AspNetCore.Builder;

using System.Diagnostics;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.ReadAppSettings();
appSettings.Validate();

builder.Services.Configure(appSettings);

var app = builder.Build();

app.Configure(appSettings);

app.Run();
