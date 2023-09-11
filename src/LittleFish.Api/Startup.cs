// using LittleFish.Api.BackgroundServices;
// using LittleFish.Api.Infrastructure.Configurations;
// using LittleFish.Api.Infrastructure.Filters;
// using LittleFish.Api.Infrastructure.Registrations;
// using LittleFish.BooksModule;
// using LittleFish.NetCoreBoilerplate.BooksModule;
// using LittleFish.Core;
// using LittleFish.Core.Registrations;
// using LittleFish.Core.Settings;
// using HealthChecks.UI.Client;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Diagnostics.HealthChecks;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Microsoft.FeatureManagement;
// using Microsoft.FeatureManagement.FeatureFilters;
// using Swashbuckle.AspNetCore.SwaggerUI;
// using LittleFish.Core.Services;
// using LittleFish.Core.Models;

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
// using LittleFish.Auth.IdentityServer4;

namespace LittleFish.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddInMemoryClients(Config.Clients)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddTestUsers(Users.Get())
                .AddDeveloperSigningCredential();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication("Bearer", options => {
                    options.ApiName = "product";
                    options.Authority = "http://localhost:5000/api";
                    options.RequireHttpsMetadata = false;
                });
                // .AddJwtBearer("Bearer", options => {
                //     // options.ApiName = "demoapi";
                //     options.Authority = "http://localhost:5000";
                //     options.RequireHttpsMetadata = false;
                //     // options.TokenValidationParameters = new TokenValidationParameters {};
                // });
             // ...

            services.AddControllers();

           /* services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://login.yourdomain.com";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //..
                    };
                });*/

            // ...

            services.AddMvc();

            services
                .AddHttpContextAccessor()
                .AddRouting(options => options.LowercaseUrls = true);

            services.AddMvcCore(options =>
                {
                    options.Filters.Add<HttpGlobalExceptionFilter>();
                    options.Filters.Add<ValidateModelStateFilter>();
                    // options.Filters.Add<ApiKeyAuthorizationFilter>();
                })
                .AddApiExplorer()
                .AddDataAnnotations();

            //there is a difference between AddDbContext() and AddDbContextPool(), more info https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-2.0#dbcontext-pooling and https://stackoverflow.com/questions/48443567/adddbcontext-or-adddbcontextpool
            // services.AddDbContext<EmployeesContext>(options => options.UseMySql(_configuration.GetConnectionString("MySqlDb"), ServerVersion.Parse("8.0")), contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Singleton);
            // services.AddDbContextPool<CarsContext>(options => options.UseSqlServer(_configuration.GetConnectionString("MsSqlDb")), poolSize: 10);

            services.Configure<ApiKeySettings>(_configuration.GetSection("ApiKey"));
            services.AddSwagger(_configuration);

            services.Configure<PingWebsiteSettings>(_configuration.GetSection("PingWebsite"));
            // var builder = WebApplication.CreateBuilder(args);
            services.Configure<MongoDBSettings>(_configuration.GetSection("MongoDB"));
            services.AddSingleton<MongoDBService>();

            services.Configure<MongoDBSettings>(_configuration.GetSection("MongoDB2"));
            services.AddSingleton<ProductsMongoDBService>();

            services.AddHostedService<PingWebsiteBackgroundService>();
            services.AddHttpClient(nameof(PingWebsiteBackgroundService));
            // services.AddHttpClient(nameof(MongoDBService));
            // services.AddHostedService<MongoDBService>();

            services.AddCoreComponents();
            // services.AddBooksModule(_configuration);

            services.AddFeatureManagement()
                .AddFeatureFilter<TimeWindowFilter>();

            services.AddHealthChecks();
                // .AddMySql(_configuration.GetConnectionString("MySqlDb"))
                // .AddSqlServer(_configuration.GetConnectionString("MsSqlDb"))
                // .AddBooksModule(_configuration); *
            
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple Api V1");
                    c.DocExpansion(DocExpansion.None);
                });
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapUserModule();
                // endpoints.MapBooksModule();

                // endpoints.MapHealthChecks("/health", new HealthCheckOptions
                // {
                //     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                // });
            });

            // app.InitBooksModule();
            // app.InitUserModule();
        }
    }
}
