using Microsoft.AspNetCore.Mvc;

namespace Schedule.API.EndPoints;

public record CreateScheduleRequest(string Name, string PhoneNumber, string Email)
{
    public static implicit operator CreateScheduleCommand(CreateScheduleRequest request)
    {
        return new CreateScheduleCommand(request.Name, request.PhoneNumber, request.Email);
    }
}

public record CreateScheduleResponse(int Id);


public class CreateScheduleEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/schedule", async (CreateScheduleRequest request, ISender sender) =>
            {
                CreateScheduleCommand command = request;
                CreateScheduleResult result = await sender.Send(command);
                return new CreateScheduleResponse(result.Id);
            })
            .Produces<CreateScheduleResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Create schedule endpoint")
            .WithName("Create schedule");
    }
}
