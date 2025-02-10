using Business.Consumers;
using Business.GrpcClients;
using Business.PipelineBehaviors;
using Business.PipelineBehaviours;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Business;

public static class BusinessInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehaviour<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
            .AddScoped<AuthGrpcClient>()
            .AddRabbitAndMassTransit()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        return services;
    }

    private static IServiceCollection AddRabbitAndMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            busConfigurator.AddConsumer<FileUploadResponseConsumer>();

            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("file-upload-response", e =>
                {
                    e.ConfigureConsumer<FileUploadResponseConsumer>(context);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
