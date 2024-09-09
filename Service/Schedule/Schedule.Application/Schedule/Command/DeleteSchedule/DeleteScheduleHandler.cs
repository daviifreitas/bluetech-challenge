namespace Schedule.Application.Schedule.Command.DeleteSchedule;

public class DeleteScheduleCommandHandler(IScheduleService service) : ICommandHandler<DeleteScheduleCommand,DeleteScheduleResult>
{
    public async Task<DeleteScheduleResult> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Schedule? scheduleByIdForDelete = await service.GetByIdAsync(request.Id);
        
        if(scheduleByIdForDelete is null)
        {
            throw new ScheduleNotFoundException(request.Id);
        }
        
        bool isScheduleDeleted = await service.DeleteAsync(scheduleByIdForDelete);
        
        return new DeleteScheduleResult(isScheduleDeleted);
    }
}