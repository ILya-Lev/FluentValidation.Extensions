using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FluentValidation.ConfigurationOnStartup;

internal static class OptionsBuilderFluentValidationExtensions
{
    public static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(
        this OptionsBuilder<TOptions> optionsBuilder)
        where TOptions: class
    {
        optionsBuilder
            .Services
            .AddSingleton<IValidateOptions<TOptions>>(sp =>
            {
                //but relies on usually-scoped validators => needs service provider
                //the class is registered as a singleton,
                var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new FluentValidationOptions<TOptions>(optionsBuilder.Name, serviceScopeFactory);
            });

        return optionsBuilder;
    }
}