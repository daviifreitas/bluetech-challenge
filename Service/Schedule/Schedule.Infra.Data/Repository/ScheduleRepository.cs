using Schedule.Domain.Interfaces.Repository;
using Schedule.Infra.Data.DbContext;

namespace Schedule.Infra.Data.Repository;

public class ScheduleRepository(ScheduleDbContext dbContext)
    : RepositoryBase<Domain.Entities.Schedule>(dbContext), IScheduleRepository
{
}