namespace Schedule.Application.Schedule.Query.GetScheduleById;

public class GetScheduleByIdQueryHandler(IScheduleService scheduleService)
    : IQueryHandler<GetScheduleByIdQuery, GetScheduleByIdResult>
{
    public async Task<GetScheduleByIdResult> Handle(GetScheduleByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Schedule? scheduleById = await scheduleService.GetByIdAsync(request.Id);

        if (scheduleById is null)
        {
            throw new ScheduleNotFoundException(request.Id);
        }

        ScheduleDto scheduleDto = new(scheduleById.Id, scheduleById.Name, scheduleById.Email, scheduleById.PhoneNumber);

        return new GetScheduleByIdResult(scheduleDto);
    }
}