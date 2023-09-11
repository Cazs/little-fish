using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace LittleFish.NetCoreBoilerplate.Api.Infrastructure.Registrations
{
    public static class SwaggerRegistration
    {
        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            string secretKey = configuration.GetValue<string>("ApiKey:SecretKey");

            services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Simple Api",
                    Version = "v1",
                    Description = $"ApiKey {secretKey}",
                    Contact = new OpenApiContact
                    {
                        Name = "Casper Ndlovu",
                        Url = new Uri("https://casper.ndlovu.website/"),
                    }
                });

                swaggerOptions.OrderActionsBy(x => x.RelativePath);
                swaggerOptions.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LittleFish.NetCoreBoilerplate.Api.xml"));

                swaggerOptions.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "ApiKey needed to access the endpoints (eg: `Authorization: ApiKey xxx-xxx`)",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });

                swaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                     {
                        new OpenApiSecurityScheme
                        {
                            Name = "ApiKey",
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey",
                            },
                        },
                        Array.Empty<string>()
                     }
                });
            });
        }
    }
}
