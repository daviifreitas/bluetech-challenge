using Microsoft.EntityFrameworkCore;


namespace Schedule.Infra.Data.DbContext;

public class ScheduleDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options) : base(options)
    {
    }
    public DbSet<Domain.Entities.Schedule> Schedules { get; set; }
    
}
