using Application.Abstractions.Validators;
using FluentValidation;

namespace Presentation;

public static class ProgramValidation
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateReceptionistRequestValidator>();

        return services;
    }
}