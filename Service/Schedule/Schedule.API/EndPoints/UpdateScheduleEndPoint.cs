namespace Schedule.API.EndPoints;

public record UpdateScheduleRequest(string Name, string PhoneNumber, string Email);

public record UpdateScheduleResponse(bool IsSuccess);

public class UpdateScheduleEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/schedule/{id}", async (UpdateScheduleRequest request, ISender sender, int id) =>
            {
                UpdateScheduleCommand command = new(id, request.Name, request.PhoneNumber, request.Email);
                var updateScheduleResult = await sender.Send(command);
                return new UpdateScheduleResponse(updateScheduleResult.IsUpdated);
            })
            .Produces<UpdateScheduleResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Update schedule endpoint")
            .WithName("Update schedule");
    }
}