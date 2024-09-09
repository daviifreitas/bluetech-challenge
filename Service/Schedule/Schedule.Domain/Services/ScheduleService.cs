using Schedule.Domain.Interfaces.Repository;
using Schedule.Domain.Interfaces.Service;

namespace Schedule.Domain.Services;

public class ScheduleService(IScheduleRepository scheduleRepository)
    : ServiceBase<Entities.Schedule>(scheduleRepository), IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository = scheduleRepository;
}