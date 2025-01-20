using Business.PipelineBehaviors;
using Business.PipelineBehaviours;
using Business.Services;
using Business.Services.Interfaces;
using FluentValidation;
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

            .AddTransient<IFileCallbackService, FileCallbackService>()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()))
            .AddHttpClient<IFileCallbackService, FileCallbackService>();

        return services;
    }
}
