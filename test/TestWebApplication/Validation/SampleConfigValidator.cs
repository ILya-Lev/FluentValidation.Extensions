using System.Text.RegularExpressions;
using FluentValidation;
using TestWebApplication.Configuration;

namespace TestWebApplication.Validation;

public class SampleConfigValidator : AbstractValidator<SampleConfig>
{
    public SampleConfigValidator()
    {
        RuleFor(c => c.Amount)
            .NotNull()
            .Must(a => a > 0)
            .WithMessage($"{nameof(SampleConfig.Amount)} should be positive");
        
        RuleFor(c => c.Timeout)
            .NotNull()
            .Must(t => TimeSpan.Zero < t && t < TimeSpan.FromHours(1))
            .WithMessage($"{nameof(SampleConfig.Timeout)} should be in range (0, 1h)");

        RuleFor(c => c.QuoteDate)
            .NotNull()
            .Must(quoteDate => DateTime.TryParse(quoteDate, out var qd)
                               //it's a pain in the neck; kind is unspecified
                            && qd >= new DateTime(2022, 01, 01))
            .WithMessage($"{nameof(SampleConfig.QuoteDate)} should be parsable and on or after 2022-01-01");

        RuleFor(c => c.Name)
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage($"{nameof(SampleConfig.Name)} is required");

        RuleFor(c => c.Token)
            .NotNull()
            .Must(t => Regex.IsMatch(t, @"\w+"))
            .WithMessage($"{nameof(SampleConfig.Token)} should consist of word characters only");
    }
}