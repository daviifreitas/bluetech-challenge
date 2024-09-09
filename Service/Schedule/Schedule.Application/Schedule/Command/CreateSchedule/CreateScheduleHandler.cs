namespace Schedule.Application.Schedule.Command.CreateSchedule;

public class CreateSchedulerCommandHandler(IScheduleService scheduleService) : ICommandHandler<CreateScheduleCommand, CreateScheduleResult>
{
    public async Task<CreateScheduleResult> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Schedule scheduleForCreate = request;
        var isCreated = await scheduleService.CreateAsync(scheduleForCreate);
        
        if(!isCreated)
            throw new BadRequestException("Schedule not created");
        
        return new CreateScheduleResult(scheduleForCreate.Id);
    }
}