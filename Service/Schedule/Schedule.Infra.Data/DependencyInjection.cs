using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Schedule.Domain.Interfaces.Repository;
using Schedule.Infra.Data.DbContext;
using Schedule.Infra.Data.Repository;

namespace Schedule.Infra.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSqlServer<ScheduleDbContext>(configuration.GetConnectionString("DatabaseConnection") ??
                                                     string.Empty);
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            return services;
        }
    }
}
