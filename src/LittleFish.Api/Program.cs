using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
// using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using Swashbuckle.AspNetCore.SwaggerUI;
using HealthChecks.UI.Client;
using LittleFish.Api.BackgroundServices;
using LittleFish.Api.Infrastructure.Configurations;
using LittleFish.Api.Infrastructure.Filters;
using LittleFish.Api.Infrastructure.Registrations;
// using LittleFish.NetCoreBoilerplate.BooksModule;
using LittleFish.Core;
using LittleFish.Core.Registrations;
using LittleFish.Core.Settings;
using LittleFish.Core.Services;
using LittleFish.Core.Models;
using LittleFish.Api.IdentityServer4;



using LittleFish.Api;
using LittleFish.Api.Infrastructure.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using Serilog;

Log.Logger = SerilogConfigurator.CreateLogger();

try
{
    Log.Logger.Information("Starting up");
    using var webHost = CreateWebHostBuilder(args).Build();
    await webHost.RunAsync();
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Application start-up failed");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

static IHostBuilder CreateWebHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureWebHostDefaults(webBuilder =>
        {
    
            // // ...

            // webBuilder.Services().AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer("Bearer", options =>
            //     {
            //         options.Authority = "https://login.yourdomain.com";

            //         options.TokenValidationParameters = new TokenValidationParameters
            //         {
            //             //..
            //         };
            //     });

            // // ...

            webBuilder.UseStartup<Startup>();
        });
