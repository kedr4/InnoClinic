using Business.Offices.Commands.ChangeOfficeStatus;
using Business.Offices.Commands.CreateOffice;
using Business.Offices.Commands.UpdateOffice;
using Business.Offices.Queries.GetAllOffices;
using Business.Offices.Queries.GetOfficeById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation;

public static class OfficeEndpoints
{
    public static WebApplication MapOfficeEndpoints(this WebApplication app) 
    {


        app.MapPost("/api/offices", async ([FromBody] CreateOfficeCommand command, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
        {
            var officeId = await mediator.Send(command, cancellationToken);

            return Results.Ok(officeId);
        });

        app.MapGet("/api/offices/{active:bool}/{page:int}/{size:int}", async ([FromRoute] int page, [FromRoute] int size, [FromRoute] bool? active, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetAllOfficesQuery(page, size, active), cancellationToken);

            return Results.Ok(result);
        });

        app.MapGet("/api/offices/{page:int}/{size:int}", async ([FromRoute] int page, [FromRoute] int size, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetAllOfficesQuery(page, size, null));

            return Results.Ok(result);
        });

        app.MapGet("/api/offices/{id:guid}", async ([FromRoute] Guid id, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
        {
            var office = await mediator.Send(new GetOfficeByIdQuery(id), cancellationToken);

            return office;
        });

        app.MapPut("/api/offices", async ([FromBody] UpdateOfficeCommand command, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
         {
             var office = await mediator.Send(command, cancellationToken);

             return office;
         });

        app.MapPut("/api/offices/{id:guid}/{status:bool}", async ([FromRoute] Guid id, [FromRoute] bool status,
            [FromServices] ISender mediator, CancellationToken cancellationToken) =>
        {
            var command = new ChangeOfficeStatusCommand(id, status);

            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(result);
        });

        return app;
    }
}
