namespace Schedule.Application.Schedule.Command.UpdateSchedule;

public class UpdateScheduleCommandHandler(IScheduleService scheduleService) : ICommandHandler<UpdateScheduleCommand, UpdateScheduleResult> 
{
    public async Task<UpdateScheduleResult> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Schedule? scheduleByIdForUpdate = await scheduleService.GetByIdAsync(request.Id);
        
        if(scheduleByIdForUpdate is null)
        {
            throw new ScheduleNotFoundException(request.Id);
        }
        
        request.UpdateEntity(scheduleByIdForUpdate);

        bool isScheduleUpdated = await scheduleService.UpdateAsync(scheduleByIdForUpdate);

        return new UpdateScheduleResult(isScheduleUpdated);
    }
}