using Business.Offices.Commands.ChangeOfficeStatus;
using Business.Offices.Commands.CreateOffice;
using Business.Offices.Commands.UpdateOffice;
using Business.Offices.Queries.GetAllOffices;
using Business.Offices.Queries.GetOfficeById;
using Business.Offices.Queries.GetOfficeStatus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs;
using System.Security.Claims;

namespace Presentation;

public static class OfficeEndpoints
{
    public static WebApplication MapOfficeEndpoints(this WebApplication app)
    {
        app.MapPost("/api/offices", [Authorize] async ([FromServices] IHttpContextAccessor httpContextAccessor, [FromForm] PostOfficeRequest request, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(userIdClaim.Value);

            var command = new CreateOfficeCommand(userId, request.City, request.Street, request.HouseNumber, request.OfficeNumber, request.Photo, request.RegistryPhoneNumber, request.IsActive);
          

            var officeId = await mediator.Send(command, cancellationToken);

            return Results.Ok(officeId);
        }).DisableAntiforgery();

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

        app.MapGet("/api/statuses/{id:guid}", async ([FromRoute] Guid id, [FromServices] ISender mediator, CancellationToken cancellationToken) =>
        {
            var status = await mediator.Send(new GetOfficeStatusQuery(id), cancellationToken);

            return Results.Ok(status);
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
