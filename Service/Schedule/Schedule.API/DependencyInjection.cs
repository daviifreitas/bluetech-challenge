using Microsoft.EntityFrameworkCore;
using Schedule.Infra.Data.DbContext;

namespace Schedule.API;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddCarter();
        services.AddHealthChecks();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo() { Title = "schedule API", Version = "v1" });
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                policy =>
                {
                    policy.AllowAnyOrigin()   // Allow all origins
                        .AllowAnyHeader()   // Allow any headers
                        .AllowAnyMethod();  // Allow any HTTP methods
                });
        });

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();
        app.UseSwagger();
        app.UseHealthChecks("/health", new HealthCheckOptions()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.UseCors("AllowAllOrigins");

        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ScheduleDbContext>();

        context.Database.MigrateAsync().GetAwaiter().GetResult();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "schedule API v1"));
        return app;
    }
}
