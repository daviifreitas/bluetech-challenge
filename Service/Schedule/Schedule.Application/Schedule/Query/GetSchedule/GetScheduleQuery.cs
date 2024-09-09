
namespace Schedule.Application.Schedule.Query.GetSchedule;

public record GetScheduleQuery() : IQuery<GetScheduleResult>;

public record GetScheduleResult(IEnumerable<ScheduleDto> Result);