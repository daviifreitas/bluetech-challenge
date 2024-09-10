using BuildingBlocks.Entity;
using Schedule.Application.Dto;
using Schedule.Application.Schedule.Query.GetSchedule;

namespace Schedule.Test.Application.Schedule;

public class GetScheduleQueryTest
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Faker _faker = new();
    private readonly IScheduleService _scheduleService = A.Fake<IScheduleService>();
    private readonly GetScheduleQueryHandler _handler;

    public GetScheduleQueryTest()
    {
        _handler = new GetScheduleQueryHandler(_scheduleService);
    }

    [Fact]
    public async void Handle_CorrectScheduleData_ShouldReturnExpectedData()
    {
        GetScheduleQuery query = _fixture.Create<GetScheduleQuery>();
        IEnumerable<Domain.Entities.Schedule> schedules = _fixture.CreateMany<Domain.Entities.Schedule>();
        IEnumerable<ScheduleDto> expectedSchedules =
            schedules.Select(x => new ScheduleDto(x.Id, x.Name, x.Email, x.PhoneNumber));

        A.CallTo(() => _scheduleService.GetManyAsDynamicTypeByFilterAsync(A<Func<Domain.Entities.Schedule, bool>>._,
                A<Func<Domain.Entities.Schedule, ScheduleDto>>._))
            .Returns(expectedSchedules);

        GetScheduleResult getScheduleResult = await _handler.Handle(query, CancellationToken.None);

        getScheduleResult.Result.Should().BeEquivalentTo(expectedSchedules);
    }

    [Fact]
    public async void Handle_EmptyScheduleData_ShouldReturnEmptyData()
    {
        GetScheduleQuery query = _fixture.Create<GetScheduleQuery>();
        IEnumerable<Domain.Entities.Schedule> schedules = Enumerable.Empty<Domain.Entities.Schedule>();
        IEnumerable<ScheduleDto> expectedSchedules =
            schedules.Select(x => new ScheduleDto(x.Id, x.Name, x.Email, x.PhoneNumber));

        A.CallTo(() => _scheduleService.GetManyAsDynamicTypeByFilterAsync(A<Func<Domain.Entities.Schedule, bool>>._,
                A<Func<Domain.Entities.Schedule, ScheduleDto>>._))
            .Returns(expectedSchedules);

        GetScheduleResult getScheduleResult = await _handler.Handle(query, CancellationToken.None);

        getScheduleResult.Result.Should().BeEmpty();
    }

    [Fact]
    public async void Handle_ThrowsAnyException_ShouldThrows()
    {
        GetScheduleQuery? query = _fixture.Create<GetScheduleQuery>();

        A.CallTo(() => _scheduleService.GetManyAsDynamicTypeByFilterAsync(A<Func<Domain.Entities.Schedule, bool>>._,
                A<Func<Domain.Entities.Schedule, ScheduleDto>>._))
            .Throws<Exception>();

        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();
    }
}