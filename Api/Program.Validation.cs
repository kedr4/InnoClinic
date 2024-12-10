//using Application.Abstractions.Validators;
namespace Presentation;

public static class ProgramValidation
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        // services.AddValidatorsFromAssemblyContaining<CreateReceptionistRequestValidator>();

        return services;
    }
}