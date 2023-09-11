using LittleFish.NetCoreBoilerplate.Api.BackgroundServices;
using LittleFish.NetCoreBoilerplate.Api.Infrastructure.Configurations;
using LittleFish.NetCoreBoilerplate.Api.Infrastructure.Filters;
using LittleFish.NetCoreBoilerplate.Api.Infrastructure.Registrations;
using LittleFish.NetCoreBoilerplate.BooksModule;
using LittleFish.NetCoreBoilerplate.Core;
using LittleFish.NetCoreBoilerplate.Core.Registrations;
using LittleFish.NetCoreBoilerplate.Core.Settings;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace LittleFish.NetCoreBoilerplate.Api
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
            services
                .AddHttpContextAccessor()
                .AddRouting(options => options.LowercaseUrls = true);

            services.AddMvcCore(options =>
                {
                    options.Filters.Add<HttpGlobalExceptionFilter>();
                    options.Filters.Add<ValidateModelStateFilter>();
                    options.Filters.Add<ApiKeyAuthorizationFilter>();
                })
                .AddApiExplorer()
                .AddDataAnnotations();

            //there is a difference between AddDbContext() and AddDbContextPool(), more info https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-2.0#dbcontext-pooling and https://stackoverflow.com/questions/48443567/adddbcontext-or-adddbcontextpool
            services.AddDbContext<EmployeesContext>(options => options.UseMySql(_configuration.GetConnectionString("MySqlDb"), ServerVersion.Parse("8.0")), contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Singleton);
            services.AddDbContextPool<CarsContext>(options => options.UseSqlServer(_configuration.GetConnectionString("MsSqlDb")), poolSize: 10);

            services.Configure<ApiKeySettings>(_configuration.GetSection("ApiKey"));
            services.AddSwagger(_configuration);

            services.Configure<PingWebsiteSettings>(_configuration.GetSection("PingWebsite"));
            services.AddHostedService<PingWebsiteBackgroundService>();
            services.AddHttpClient(nameof(PingWebsiteBackgroundService));

            services.AddCoreComponents();
            services.AddBooksModule(_configuration);

            services.AddFeatureManagement()
                .AddFeatureFilter<TimeWindowFilter>();

            services.AddHealthChecks()
                .AddMySql(_configuration.GetConnectionString("MySqlDb"))
                .AddSqlServer(_configuration.GetConnectionString("MsSqlDb"))
                .AddBooksModule(_configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBooksModule();

                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple Api V1");
                c.DocExpansion(DocExpansion.None);
            });

            app.InitBooksModule();
        }
    }
}