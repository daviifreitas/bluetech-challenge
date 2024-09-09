namespace Schedule.Application.Schedule.Command.UpdateSchedule;

public record UpdateScheduleCommand(int Id, string Name, string PhoneNumber, string Email)
    : ICommand<UpdateScheduleResult>
{
    public void UpdateEntity(Domain.Entities.Schedule scheduleForUpdate)
    {
        scheduleForUpdate.Name = Name;
        scheduleForUpdate.Email = Email;
        scheduleForUpdate.PhoneNumber = PhoneNumber;
    }
}

public record UpdateScheduleResult(bool IsUpdated);

public class UpdateScheduleCommandValidator : AbstractValidator<UpdateScheduleCommand>
{
    public UpdateScheduleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is required.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
    }
}