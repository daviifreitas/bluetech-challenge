using Schedule.Application.Dto;
using Schedule.Application.Exceptions;
using Schedule.Application.Schedule.Query.GetScheduleById;

namespace Schedule.Test.Application.Schedule;

public class GetScheduleByIdQueryTest
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Faker _faker = new();
    private readonly IScheduleService _scheduleService = A.Fake<IScheduleService>();
    private readonly GetScheduleByIdQueryHandler _handler;
    
    public GetScheduleByIdQueryTest()
    {
        _handler = new GetScheduleByIdQueryHandler(_scheduleService);
    }
    
    [Fact]
    public async void Handle_ExistScheduleById_ShouldReturnExpectedData()
    {
        GetScheduleByIdQuery query = _fixture.Create<GetScheduleByIdQuery>();
        Domain.Entities.Schedule schedule = _fixture.Create<Domain.Entities.Schedule>();
        ScheduleDto expectedSchedule = new(schedule.Id, schedule.Name, schedule.Email, schedule.PhoneNumber);
        
        A.CallTo(() => _scheduleService.GetByIdAsync(A<int>.That.Matches(x => x == query.Id)))
            .Returns(schedule);
        
        GetScheduleByIdResult getScheduleByIdResult = await _handler.Handle(query, CancellationToken.None);
        
        getScheduleByIdResult.Result.Should().BeEquivalentTo(expectedSchedule);
    }
    
    [Fact]
    public async void Handle_NotExistScheduleById_ShouldThrows()
    {
        GetScheduleByIdQuery query = _fixture.Create<GetScheduleByIdQuery>();
        Domain.Entities.Schedule? schedule = null;
        
        A.CallTo(() => _scheduleService.GetByIdAsync(A<int>.That.Matches(x => x == query.Id)))
            .Returns(schedule);
        
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
        
        await act.Should().ThrowAsync<ScheduleNotFoundException>().WithMessage($"Entity \"Schedule\" ({query.Id} was not found.)");
    }
    
    [Fact]
    public async void Handle_ThrowsAnyException_ShouldThrows()
    {
        GetScheduleByIdQuery query = _fixture.Create<GetScheduleByIdQuery>();
        
        A.CallTo(() => _scheduleService.GetByIdAsync(A<int>.That.Matches(x => x == query.Id)))
            .Throws<Exception>();
        
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
        
        await act.Should().ThrowAsync<Exception>();
    }
    
    [Fact]
    public async void Validator_InvalidId_ShouldFail()
    {
        GetScheduleByIdQuery query = _fixture.Build<GetScheduleByIdQuery>().With(x => x.Id, 0).Create();
        
        GetScheduleByIdQueryValidator validator = new();
        
        FluentValidation.Results.ValidationResult validationResult = await validator.ValidateAsync(query);
        
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(error => error.ErrorMessage == "Id is required.");
    }
    
    [Fact]
    public async void Validator_ValidId_ShouldPass()
    {
        GetScheduleByIdQuery query = _fixture.Create<GetScheduleByIdQuery>();
        
        GetScheduleByIdQueryValidator validator = new();
        
        FluentValidation.Results.ValidationResult validationResult = await validator.ValidateAsync(query);
        
        validationResult.IsValid.Should().BeTrue();
    }
}