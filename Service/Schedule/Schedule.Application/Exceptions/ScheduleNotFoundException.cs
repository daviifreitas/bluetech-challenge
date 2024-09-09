namespace Schedule.Application.Exceptions;

[ExcludeFromCodeCoverage]
public class ScheduleNotFoundException(int scheduleId) : NotFoundException("Schedule", scheduleId)
{
    
}