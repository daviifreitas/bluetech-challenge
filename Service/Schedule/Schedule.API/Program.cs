using Schedule.Infra.Data;
using Schedule.Infra.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApiServices(builder.Configuration).AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseApiServices();


//app.UseCors(
//    options => options.WithOrigins("https://localhost").AllowAnyMethod()); 

app.Run();
