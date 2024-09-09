using Schedule.Application.Exceptions;
using Schedule.Application.Schedule.Command.DeleteSchedule;

namespace Schedule.Test.Application.Schedule;

public class DeleteScheduleHandlerTest
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Faker _faker = new();
    private readonly IScheduleService _scheduleService = A.Fake<IScheduleService>();
    private readonly DeleteScheduleCommandHandler _handler;

    public DeleteScheduleHandlerTest()
    {
        _handler = new DeleteScheduleCommandHandler(_scheduleService);
    }

    [Fact]
    public async void Handle_CorrectScheduleData_ShouldDeleted()
    {
        DeleteScheduleCommand command = _fixture.Create<DeleteScheduleCommand>();
        Domain.Entities.Schedule scheduleByIdForDelete = _fixture.Create<Domain.Entities.Schedule>();

        A.CallTo(() => _scheduleService.GetByIdAsync(A<int>.That.Matches(x => x == command.Id)))
            .Returns(scheduleByIdForDelete);
        A.CallTo(() => _scheduleService.DeleteAsync(A<Domain.Entities.Schedule>._)).Returns(true);

        DeleteScheduleResult deleteScheduleResult = await _handler.Handle(command, CancellationToken.None);

        deleteScheduleResult.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async void Handle_DeleteExpectedScheduleData_ShouldDeleted()
    {
        DeleteScheduleCommand command = _fixture.Create<DeleteScheduleCommand>();
        Domain.Entities.Schedule scheduleByIdForDelete = _fixture.Create<Domain.Entities.Schedule>();

        A.CallTo(() => _scheduleService.GetByIdAsync(A<int>.That.Matches(x => x == command.Id)))
            .Returns(scheduleByIdForDelete);
        A.CallTo(() => _scheduleService.DeleteAsync(A<Domain.Entities.Schedule>.That.Matches(x =>
            x.Name == scheduleByIdForDelete.Name && x.Email == scheduleByIdForDelete.Email &&
            x.PhoneNumber == scheduleByIdForDelete.PhoneNumber))).Returns(true);

        DeleteScheduleResult deleteScheduleResult = await _handler.Handle(command, CancellationToken.None);

        deleteScheduleResult.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async void Handle_GetByIdReturnsNull_ShouldScheduleNotUpdated()
    {
        DeleteScheduleCommand command = _fixture.Create<DeleteScheduleCommand>();
        Domain.Entities.Schedule? scheduleByIdForDelete = null;

        A.CallTo(() => _scheduleService.GetByIdAsync(A<int>.That.Matches(x => x == command.Id)))
            .Returns(scheduleByIdForDelete);
        try
        {
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (ScheduleNotFoundException)
        {
            A.CallTo(() => _scheduleService.DeleteAsync(A<Domain.Entities.Schedule>._)).MustNotHaveHappened();
        }
    }

    [Fact]
    public async void Handle_ScheduleNotFoundById_ThrowsExpectedExceptionWithMessage()
    {
        DeleteScheduleCommand command = _fixture.Create<DeleteScheduleCommand>();
        Domain.Entities.Schedule? scheduleByIdForDelete = null;

        A.CallTo(() => _scheduleService.GetByIdAsync(A<int>.That.Matches(x => x == command.Id)))
            .Returns(scheduleByIdForDelete);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ScheduleNotFoundException>()
            .WithMessage($"Entity \"Schedule\" ({command.Id} was not found.)");
    }

    [Fact]
    public async void Handle_ScheduleNotDelete_ShouldReturnExpectedData()
    {
        
        DeleteScheduleCommand command = _fixture.Create<DeleteScheduleCommand>();
        Domain.Entities.Schedule scheduleByIdForDelete = _fixture.Create<Domain.Entities.Schedule>();

        A.CallTo(() => _scheduleService.GetByIdAsync(A<int>.That.Matches(x => x == command.Id)))
            .Returns(scheduleByIdForDelete);
        A.CallTo(() => _scheduleService.DeleteAsync(A<Domain.Entities.Schedule>.Ignored)).Returns(false);

        DeleteScheduleResult deleteScheduleResult = await _handler.Handle(command, CancellationToken.None);

        deleteScheduleResult.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public async void Validator_DataIsValid_ShouldBeValid()
    {
        DeleteScheduleCommandValidator validator = new();
        var command = _fixture.Create<DeleteScheduleCommand>();
        var validationResult = await validator.ValidateAsync(command, CancellationToken.None);
        
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async void Validator_IdIsInvalid_ShouldFailWithExpectedMessage()
    {
        DeleteScheduleCommandValidator validator = new();
        var command = _fixture.Build<DeleteScheduleCommand>().With(x => x.Id, 0).Create();
        var validationResult = await validator.ValidateAsync(command, CancellationToken.None);
        
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == "Id is required.");
    }
}