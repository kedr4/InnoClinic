using Application.Abstractions.Validators;
using Application.Filters;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using Serilog;

namespace Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
        builder.Services.AddValidation();

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ValidateModelFilter>();
            options.Filters.Add<ExceptionLoggingFilter>();  // Фильтр для логгирования исключений
        });

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();
        IConfiguration configuration = builder.Configuration;

        builder.Services.AddProgramOptions(configuration);
        builder.Services.AddSwagger();
        builder.Services.AddControllers();
        builder.Services.AddEntityFramework(configuration);
        builder.Services.AddAuthorization();
        builder.Services.AddAuthenticationServices();
        builder.Services.AddServices();
        builder.Services.AddOpenApi();
       // builder.AddSerilog(builder.Configuration);
        //builder.Host.UseSerilog((context, configuration) =>
        //{
        //    configuration.Enrich.FromLogContext();
        //    configuration.Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
        //    configuration.WriteTo.Console();
        //});

        var app = builder.Build();

        app.UseCustomExceptionHandlingMiddleware();
        app.UseSerilog(builder.Configuration);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapGet("/", () => Results.Redirect("/swagger"));

        }

        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        //app.UseSerilogRequestLogging();

        app.MapControllers();

        app.Run();
    }
}
