using Schedule.Domain.Services;

namespace Schedule.Application;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
        services.AddScoped<IScheduleService, ScheduleService>();
        return services;
    }
}
