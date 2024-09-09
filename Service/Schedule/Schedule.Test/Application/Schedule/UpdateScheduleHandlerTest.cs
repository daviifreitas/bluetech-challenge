using Schedule.Application.Exceptions;
using Schedule.Application.Schedule.Command.UpdateSchedule;

namespace Schedule.Test.Application.Schedule;

public class UpdateScheduleHandlerTest
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Faker _faker = new();
    private readonly IScheduleService _scheduleService = A.Fake<IScheduleService>();
    private readonly UpdateScheduleCommandHandler _handler;

    public UpdateScheduleHandlerTest()
    {
        _handler = new UpdateScheduleCommandHandler(_scheduleService);
    }

    [Fact]
    public async void Handle_CorrectScheduleData_ShouldUpdated()
    {
        UpdateScheduleCommand command = _fixture.Create<UpdateScheduleCommand>();
        Domain.Entities.Schedule scheduleByIdForUpdate = _fixture.Create<Domain.Entities.Schedule>();

        A.CallTo(() => _scheduleService.GetByIdAsync(A<int>.That.Matches(x => x == command.Id)))
            .Returns(scheduleByIdForUpdate);
        A.CallTo(() => _scheduleService.UpdateAsync(A<Domain.Entities.Schedule>._)).Returns(true);

        UpdateScheduleResult updateScheduleResult = await _handler.Handle(command, CancellationToken.None);

        command.UpdateEntity(scheduleByIdForUpdate);

        updateScheduleResult.IsUpdated.Should().BeTrue();
    }

    [Fact]
    public async void Handle_CorrectScheduleData_ShouldUpdatedExpectedData()
    {
        UpdateScheduleCommand command = _fixture.Create<UpdateScheduleCommand>();
        Domain.Entities.Schedule scheduleByIdForUpdate = _fixture.Create<Domain.Entities.Schedule>();

        A.CallTo(() => _scheduleService.GetByIdAsync(A<int>.That.Matches(x => x == command.Id)))
            .Returns(scheduleByIdForUpdate);
        A.CallTo(() => _scheduleService.UpdateAsync(A<Domain.Entities.Schedule>._)).Returns(true);

        await _handler.Handle(command, CancellationToken.None);

        command.UpdateEntity(scheduleByIdForUpdate);


        A.CallTo(() => _scheduleService.UpdateAsync(A<Domain.Entities.Schedule>.That.Matches(schedule =>
            schedule.Name == scheduleByIdForUpdate.Name &&
            schedule.PhoneNumber == scheduleByIdForUpdate.PhoneNumber &&
            schedule.Email == scheduleByIdForUpdate.Email))).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async void Handle_CorrectData_NotThrowsException()
    {
        UpdateScheduleCommand command = _fixture.Create<UpdateScheduleCommand>();
        Domain.Entities.Schedule scheduleByIdForUpdate = _fixture.Create<Domain.Entities.Schedule>();

        A.CallTo(() => _scheduleService.GetByIdAsync(A<int>.That.Matches(x => x == command.Id)))
            .Returns(scheduleByIdForUpdate);
        A.CallTo(() => _scheduleService.UpdateAsync(A<Domain.Entities.Schedule>._)).Returns(true);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().NotThrowAsync<ScheduleNotFoundException>();
    }

    [Fact]
    public async void Handle_ScheduleNotUpdated_ShouldReturnExpectedData()
    {
        UpdateScheduleCommand command = _fixture.Create<UpdateScheduleCommand>();
        Domain.Entities.Schedule scheduleByIdForUpdate = _fixture.Create<Domain.Entities.Schedule>();
        Domain.Entities.Schedule expectedUpdateSchedule = scheduleByIdForUpdate;
        command.UpdateEntity(expectedUpdateSchedule);

        A.CallTo(() => _scheduleService.GetByIdAsync(A<int>.That.Matches(x => x == command.Id)))
            .Returns(scheduleByIdForUpdate);
        A.CallTo(() => _scheduleService.UpdateAsync(A<Domain.Entities.Schedule>.That.Matches(updateSchedule =>
            updateSchedule.Name == expectedUpdateSchedule.Name &&
            updateSchedule.Email == expectedUpdateSchedule.Email &&
            updateSchedule.PhoneNumber == expectedUpdateSchedule.PhoneNumber))).Returns(false);

        UpdateScheduleResult updateScheduleResult = await _handler.Handle(command, CancellationToken.None);

        updateScheduleResult.IsUpdated.Should().BeFalse();
    }

    [Fact]
    public async void Handle_ScheduleNotFound_NotUpdate()
    {
        UpdateScheduleCommand command = _fixture.Create<UpdateScheduleCommand>();

        Domain.Entities.Schedule? returnedScheduleByCommandId = null;

        A.CallTo(() => _scheduleService.GetByIdAsync(A<int>.That.Matches(x => x == command.Id)))
            .Returns(returnedScheduleByCommandId);

        try
        {
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (ScheduleNotFoundException)
        {
            A.CallTo(() => _scheduleService.UpdateAsync(A<Domain.Entities.Schedule>._)).MustNotHaveHappened();
        }
    }

    [Fact]
    public async void Handle_ScheduleNotFound_ThrowsExpectedExceptionWithMessage()
    {
        UpdateScheduleCommand command = _fixture.Create<UpdateScheduleCommand>();

        Domain.Entities.Schedule? returnedScheduleByCommandId = null;

        A.CallTo(() => _scheduleService.GetByIdAsync(A<int>.That.Matches(x => x == command.Id)))
            .Returns(returnedScheduleByCommandId);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ScheduleNotFoundException>()
            .WithMessage($"Entity \"Schedule\" ({command.Id} was not found.)");
    }

    [Fact]
    public async void Validator_DataIsValid_ShouldBeValid()
    {
        UpdateScheduleCommand command = _fixture.Create<UpdateScheduleCommand>();
        UpdateScheduleCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(command);

        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async void Validator_NameIsInvalid_ShouldInvalidWithExpectedMessage()
    {
        UpdateScheduleCommand command =
            _fixture.Build<UpdateScheduleCommand>().With(x => x.Name, string.Empty).Create();
        UpdateScheduleCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(command);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == "Name is required.");
    }

    [Fact]
    public async void Validator_PhoneNumberIsRequired_ShouldInvalid_WithExpectedMessage()
    {
        UpdateScheduleCommand command =
            _fixture.Build<UpdateScheduleCommand>().With(x => x.PhoneNumber, string.Empty).Create();
        UpdateScheduleCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(command);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == "PhoneNumber is required.");
    }
    
    [Fact]
    public async void Validator_EmailIsRequired_ShouldInvalid_WithExpectedMessage()
    {
        UpdateScheduleCommand command =
            _fixture.Build<UpdateScheduleCommand>().With(x => x.Email, string.Empty).Create();
        UpdateScheduleCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(command);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == "Email is required.");
    }
}