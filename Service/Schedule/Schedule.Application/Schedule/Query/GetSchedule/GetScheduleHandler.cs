using BuildingBlocks.Entity;

namespace Schedule.Application.Schedule.Query.GetSchedule;

public class GetScheduleHandler(IScheduleService scheduleService) : IQueryHandler<GetScheduleQuery, GetScheduleResult>
{
    public async Task<GetScheduleResult> Handle(GetScheduleQuery request, CancellationToken cancellationToken)
    {
        bool FilterActiveSchedules(Domain.Entities.Schedule x) => x.StatusDefault == StatusDefault.Active;

        ScheduleDto MapScheduleToDto(Domain.Entities.Schedule x) =>
            new(x.Id, x.Name, x.Email, x.PhoneNumber);

        IEnumerable<ScheduleDto> activeSchedulesAsDto =
            await scheduleService.GetManyAsDynamicTypeByFilterAsync(FilterActiveSchedules,
                MapScheduleToDto);

        return new GetScheduleResult(activeSchedulesAsDto);
    }
}