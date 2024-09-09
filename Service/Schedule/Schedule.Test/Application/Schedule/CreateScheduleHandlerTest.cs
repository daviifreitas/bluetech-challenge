namespace Schedule.Test.Application.Schedule;

public class CreateScheduleHandlerTest
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Faker _faker = new();
    private readonly IScheduleService _scheduleService = A.Fake<IScheduleService>();
    private readonly CreateSchedulerCommandHandler _handler;

    public CreateScheduleHandlerTest()
    {
        _handler = new CreateSchedulerCommandHandler(_scheduleService);
    }


    [Fact]
    public async void Handle_CorrectScheduleData_ShouldCreated()
    {
        CreateScheduleCommand command = _fixture.Create<CreateScheduleCommand>();

        Domain.Entities.Schedule expectedScheduleForCreate = command;

        A.CallTo(() => _scheduleService.CreateAsync(A<Domain.Entities.Schedule>._)).Returns(true);

        await _handler.Handle(command, CancellationToken.None);

        A.CallTo(() => _scheduleService.CreateAsync(A<Domain.Entities.Schedule>.That.Matches(schedule =>
            schedule.Name == expectedScheduleForCreate.Name &&
            schedule.PhoneNumber == expectedScheduleForCreate.PhoneNumber &&
            schedule.Email == expectedScheduleForCreate.Email))).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async void Handle_InvalidScheduleData_NotCreated()
    {
        CreateScheduleCommand command = _fixture.Create<CreateScheduleCommand>();

        Domain.Entities.Schedule expectedScheduleForCreate = command;

        A.CallTo(() => _scheduleService.CreateAsync(A<Domain.Entities.Schedule>.That.Matches(schedule =>
            schedule.Name == expectedScheduleForCreate.Name &&
            schedule.PhoneNumber == expectedScheduleForCreate.PhoneNumber &&
            schedule.Email == expectedScheduleForCreate.Email))).Returns(false);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>().WithMessage("Schedule not created");
    }

    [Fact]
    public async void Handle_CorrectData_ShouldReturnExpectedData()
    {
        CreateScheduleCommand command = _fixture.Create<CreateScheduleCommand>();

        Domain.Entities.Schedule expectedScheduleForCreate = command;

        A.CallTo(() => _scheduleService.CreateAsync(A<Domain.Entities.Schedule>.Ignored)).Returns(true);

        var createScheduleResult = await _handler.Handle(command, CancellationToken.None);

        createScheduleResult.Id.Should().Be(expectedScheduleForCreate.Id);
    }

    [Fact]
    public async void Handle_CorrectData_ShouldNotThrowsBadRequestException()
    {
        CreateScheduleCommand? command = _fixture.Create<CreateScheduleCommand>();

        Domain.Entities.Schedule expectedScheduleForCreate = command;

        A.CallTo(() => _scheduleService.CreateAsync(A<Domain.Entities.Schedule>.Ignored)).Returns(true);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().NotThrowAsync<BadRequestException>();
    }

    [Fact]
    public async void Handler_NotCreate_ShouldThrowsBadExceptionWithExpectedMessage()
    {
        CreateScheduleCommand command = _fixture.Create<CreateScheduleCommand>();

        A.CallTo(() => _scheduleService.CreateAsync(A<Domain.Entities.Schedule>.Ignored)).Returns(false);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>().WithMessage("Schedule not created");
    }

    [Fact]
    public async void Validator_DataAreValid_ShouldPass()
    {
        CreateScheduleCommandValidator validator = new CreateScheduleCommandValidator();

        CreateScheduleCommand command = _fixture.Create<CreateScheduleCommand>();

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async void Validator_NameAreInvalid_ShouldFailWithExpectedMessage()
    {
        CreateScheduleCommandValidator validator = new CreateScheduleCommandValidator();

        CreateScheduleCommand command =
            _fixture.Build<CreateScheduleCommand>().With(x => x.Name, string.Empty).Create();

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == "Name is required.");
    }

    [Fact]
    public async void Validator_PhoneNumberInvalid_ShouldFailWithExpectedMessage()
    {
        CreateScheduleCommandValidator validator = new CreateScheduleCommandValidator();

        CreateScheduleCommand command =
            _fixture.Build<CreateScheduleCommand>().With(x => x.PhoneNumber, string.Empty).Create();

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == "PhoneNumber is required.");
    }
    
    [Fact]
    public async void Validator_EmailInvalid_ShouldFailWithExpectedMessage()
    {
        CreateScheduleCommandValidator validator = new CreateScheduleCommandValidator();

        CreateScheduleCommand command =
            _fixture.Build<CreateScheduleCommand>().With(x => x.Email, string.Empty).Create();

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == "Email is required.");
    }
}