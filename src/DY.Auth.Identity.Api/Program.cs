using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Startup;
using DY.Auth.Identity.Api.Startup.Configuration;

using Microsoft.AspNetCore.Builder;

using System;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.ReadAppSettings();
appSettings.Validate();

Activity.DefaultIdFormat = ActivityIdFormat.W3C;
Environment.SetEnvironmentVariable(EnvironmentVariablesConstants.AppNameKey, appSettings.TelemetrySettings.AppName);

builder.Services.Configure(appSettings);

var app = builder.Build();

app.Configure(appSettings);

app.Services.InitializeUserRoles(appSettings.IdentitySettings.Roles).Wait();
app.Services.InitializeDefaultUsers(appSettings.IdentitySettings.DefaultUsers).Wait();

app.Run();
