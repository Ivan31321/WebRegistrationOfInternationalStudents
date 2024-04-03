using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Services;
using MonitoringTheProgressOfForeignStudents.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MonitoringTheProgressOfForeignStudents.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IOfficeService, OfficeService>();

            return services;
        }
    }
}
