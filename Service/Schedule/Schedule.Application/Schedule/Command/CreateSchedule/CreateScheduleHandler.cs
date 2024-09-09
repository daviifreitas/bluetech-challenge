namespace Schedule.Application.Schedule.Command.CreateSchedule;

public class CreateSchedulerCommandHandler(IScheduleService scheduleService) : ICommandHandler<CreateScheduleCommand, CreateScheduleResult>
{
    public async Task<CreateScheduleResult> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Schedule scheduleForCreate = request;
        await scheduleService.CreateAsync(scheduleForCreate);
        return new CreateScheduleResult(scheduleForCreate.Id);
    }
}