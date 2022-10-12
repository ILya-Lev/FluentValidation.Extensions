using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FluentValidation.ConfigurationOnStartup;

internal class FluentValidationOptions<TOptions> : IValidateOptions<TOptions>
    where TOptions : class
{
    private readonly string? _name;
    private readonly IServiceScopeFactory _scopeFactory;

    public FluentValidationOptions(string? name, IServiceScopeFactory scopeFactory)
    {
        _name = name;
        _scopeFactory = scopeFactory;
    }

    public ValidateOptionsResult Validate(string name, TOptions options)
    {
        //null name is used to configure all named options...
        if (_name is not null && _name != name)
            return ValidateOptionsResult.Skip;
        
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        using var scope = _scopeFactory.CreateScope();
        var validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();

        var results = validator.Validate(options);
        if (results.IsValid)
            return ValidateOptionsResult.Success;

        return ValidateOptionsResult.Fail(FormatFailures(results));
    }

    private static string[] FormatFailures(ValidationResult results)
    {
        try
        {
            return results.Errors
                .Select(vf => $"{vf.Severity}.{vf.PropertyName}.{vf.ErrorCode}" +
                              $" details: '{vf.ErrorMessage}'" +
                              $" value: '{vf.AttemptedValue}'")
                .ToArray();
        }
        catch
        {
            return results.Errors
                .Select(vf => $"{vf.Severity}.{vf.PropertyName}.{vf.ErrorCode}")
                .ToArray();
        }
    }
}