using RRHH.Core.Application.Interfaces.Services;
using RRHH.Core.Domain.Services;

namespace RRHH.WebApi
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<IEmployeeService, EmployeeService>();

            return services;
        }
    }
}
