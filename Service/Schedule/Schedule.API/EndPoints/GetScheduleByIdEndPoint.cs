

namespace Schedule.API.EndPoints;

public record GetScheduleByIdResponse(int Id, string Name, string PhoneNumber, string Email)
{
    public static implicit operator GetScheduleByIdResponse(GetScheduleByIdResult result)
    {
        return new GetScheduleByIdResponse(result.Result.Id, result.Result.Name, result.Result.PhoneNumber,
            result.Result.Email);
    }
};

public class GetScheduleByIdEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/schedule/{id}", async (ISender sender, int id) =>
            {
                var getScheduleByIdQuery = new GetScheduleByIdQuery(id);
                GetScheduleByIdResult getScheduleByIdResult = await sender.Send(getScheduleByIdQuery);
                GetScheduleByIdResponse getScheduleByIdResponse = getScheduleByIdResult;
                
                return getScheduleByIdResponse;
            })
            .Produces<GetScheduleByIdResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Get schedule by id endpoint")
            .WithName("Get schedule by id");
    }
}