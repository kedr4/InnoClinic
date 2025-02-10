using Business;
using ContractsLib;
using DataAccess;
using Presentation.Middleware;
using Serilog;
using System.Text.Json.Serialization;

namespace Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ReceptionistOnly", policy => policy.RequireRole("Receptionist"));
        });

        builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
        });

        builder.Services.AddHttpContextAccessor();


        builder.Services.AddDataAccess(builder.Configuration);
        builder.Services.AddBusiness(builder.Configuration);
        builder.Services.AddPresentation(builder.Configuration);

        var app = builder.Build();

        app.UseSerilogRequestLogging();
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/", () => Results.Redirect("swagger"));

        app.MapOfficeEndpoints();

        app.Run();
    }
}
