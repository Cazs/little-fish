using LittleFish.NetCoreBoilerplate.Core.Repositories;
using LittleFish.NetCoreBoilerplate.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LittleFish.NetCoreBoilerplate.Core.Registrations
{
    public static class CoreRegistrations
    {
        public static IServiceCollection AddCoreComponents(this IServiceCollection services)
        {
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICarService, CarService>();

            return services;
        }
    }
}
