using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace FluentValidation.ConfigurationOnStartup;

public static class ServicesFluentValidationExtensions
{
    public static OptionsBuilder<TOptions> AddWithValidation<TOptions, TValidator>(
        this IServiceCollection services
        , string configurationSectionName)
        where TOptions: class
        where TValidator : class, IValidator<TOptions>
    {
        //add the validator
        services.TryAddScoped<IValidator<TOptions>, TValidator>();
        
        //allow client's code to consume TOptions as is without dependency on IOptions interface family
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<TOptions>>().Value);

        //make sure it's called on the startup
        return services.AddOptions<TOptions>()
            .BindConfiguration(configurationSectionName)
            .ValidateFluentValidation()
            .ValidateOnStart();
    }
}