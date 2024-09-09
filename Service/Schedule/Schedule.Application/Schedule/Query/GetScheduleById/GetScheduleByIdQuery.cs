namespace Schedule.Application.Schedule.Query.GetScheduleById;

public record GetScheduleByIdQuery(int Id) : IQuery<GetScheduleByIdResult>;

public record GetScheduleByIdResult(ScheduleDto Result);

public class GetScheduleByIdQueryValidator : AbstractValidator<GetScheduleByIdQuery>
{
    public GetScheduleByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}