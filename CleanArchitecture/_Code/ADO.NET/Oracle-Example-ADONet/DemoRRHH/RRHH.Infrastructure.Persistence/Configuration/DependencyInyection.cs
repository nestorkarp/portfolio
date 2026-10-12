using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RRHH.Core.Application.Interfaces.Repository;
using RRHH.Infrastructure.Persistence.Repositories;
using System.Data.Common;


namespace RRHH.Infrastructure.Persistence.Configuration
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Lectura de la cadena de conexión desde el appsettings.json
            string connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no fue encontrada.");

            // 2. Registrar el DbProviderFactory específico de la base de datos.
            services.AddSingleton<DbProviderFactory>(Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance);

            // 3. Registrar el repositorio
            services.AddScoped<IEmployeeRepository>(sp =>
                new EmployeeRepository(
                    sp.GetRequiredService<DbProviderFactory>(),
                    connectionString
                ));


            return services;
        }
    }
}
