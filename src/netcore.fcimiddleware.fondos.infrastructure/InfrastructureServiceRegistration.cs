using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.infrastructure.Persistence;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;

namespace netcore.fcimiddleware.fondos.infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var writePostgreSQL =
                configuration.GetConnectionString("WritePostgreSQL")
                ?? throw new ArgumentNullException("No hay una cadena de escritura configurada.");

            var readPostgreSQL =
                configuration.GetConnectionString("ReadPostgreSQL")
                ?? throw new ArgumentNullException("No hay una cadena de lectura configurada.");

            services.AddDbContext<ApplicationWriteDbContext>(options =>
                options.UseNpgsql(writePostgreSQL,
                b => b.MigrationsAssembly(typeof(ApplicationWriteDbContext).Assembly.FullName)));

            services.AddDbContext<ApplicationReadDbContext>(options =>
                options
                .UseNpgsql(readPostgreSQL)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IAsyncWriteRepository<>), typeof(RepositoryWriteBase<>));
            services.AddScoped(typeof(IAsyncReadRepository<>), typeof(RepositoryReadBase<>));

            return services;
        }
    }
}