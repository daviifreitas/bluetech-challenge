namespace Schedule.Application.Schedule.Command.DeleteSchedule;

public record DeleteScheduleCommand(int Id) : ICommand<DeleteScheduleResult>;

public record DeleteScheduleResult(bool IsDeleted);

public class DeleteScheduleCommandValidator : AbstractValidator<DeleteScheduleCommand>
{
    public DeleteScheduleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}