namespace Schedule.API.EndPoints;

public record GetScheduleResponse(IEnumerable<ScheduleDto> Schedules);

public class GetSchedulesEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/schedule", async (ISender sender) =>
            {
                var getSchedulesQuery = new GetScheduleQuery();
                GetScheduleResult getScheduleResult = await sender.Send(getSchedulesQuery);
                GetScheduleResponse getSchedulesResponse = new GetScheduleResponse(getScheduleResult.Result);
                return getSchedulesResponse;
            }).Produces<GetScheduleResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Get schedules endpoint")
            .WithName("Get schedules");
    }
}