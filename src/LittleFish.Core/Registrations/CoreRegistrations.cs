using LittleFish.Core.Models;
// using LittleFish.Core.Repositories;
using LittleFish.Core.Services;
// using LittleFish.NetCoreBoilerplate.BooksModule;
using Microsoft.Extensions.DependencyInjection;

namespace LittleFish.Core.Registrations
{
    public static class CoreRegistrations
    {
        public static IServiceCollection AddCoreComponents(this IServiceCollection services)
        {
            // services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            // services.AddScoped<ICarService, CarService>();
            // services.Add<MongoDBSettings, MongoDBSettings>();
            // services.Add(MongoDBService);

            return services;
        }
    }
}
