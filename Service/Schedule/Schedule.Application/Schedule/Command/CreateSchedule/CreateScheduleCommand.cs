using System.Runtime.InteropServices.JavaScript;

namespace Schedule.Application.Schedule.Command.CreateSchedule;

public record CreateScheduleCommand(string Name,string PhoneNumber, string Email)
    : ICommand<CreateScheduleResult>
{
    public static implicit operator Domain.Entities.Schedule(CreateScheduleCommand command)
    {
        return new Domain.Entities.Schedule
        {
            Name = command.Name,
            PhoneNumber = command.PhoneNumber,
            Email = command.Email,
        };
    }
}
public record CreateScheduleResult(int Id);

public class CreateScheduleCommandValidator : AbstractValidator<CreateScheduleCommand>
{
    public CreateScheduleCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is required.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
    }
}