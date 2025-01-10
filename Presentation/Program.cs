
using Business;
using Business.Commands.CreateOffice;
using Business.Repositories.Interfaces;
using DataAccess;
using MediatR;
using Presentation.Middleware;

namespace Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDataAccess(builder.Configuration);
        builder.Services.AddBusiness(builder.Configuration);
        
        var app = builder.Build();

        app.UseMiddleware<CustomExceptionHandlerMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGet("/", () => Results.Redirect("swagger"));


        app.MapPost("/api/offices", async (CreateOfficeCommand command, IMediator sender, CancellationToken cancellationToken) =>
        {
            var officeId = await sender.Send(command, cancellationToken);
            return Results.Ok(new { OfficeId = officeId });

        });

        app.MapGet("/getall", async (IOfficeRepository officeRepository) => { return await officeRepository.GetAllAsync(CancellationToken.None); }); //for testing purposes

        app.Run();
    }
}
