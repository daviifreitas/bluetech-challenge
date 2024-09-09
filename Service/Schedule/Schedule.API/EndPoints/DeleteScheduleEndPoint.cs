using Schedule.Application.Schedule.Command.DeleteSchedule;

namespace Schedule.API.EndPoints;

public record DeleteScheduleResponse(bool IsDeleted);

public class DeleteScheduleEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/schedule/{id}", async (ISender sender, int id) =>
            {
                var deleteScheduleCommand = new DeleteScheduleCommand(id);
                var deleteScheduleResult = await sender.Send(deleteScheduleCommand);

                return new DeleteScheduleResponse(deleteScheduleResult.IsDeleted);
            })
            .Produces<DeleteScheduleResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Delete schedule endpoint")
            .WithName("Delete schedule");
    }
}