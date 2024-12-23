using Microsoft.Extensions.DependencyInjection;

namespace Application.Helpers;
public static class Helper
{
    public static IServiceCollection AddOptionsWithValidation<T>(this IServiceCollection services) where T : class
    {
        services.AddOptions<T>()
            .BindConfiguration(typeof(T).Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
