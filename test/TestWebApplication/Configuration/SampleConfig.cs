using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApplication.Configuration;

public record SampleConfig
{
    public TimeSpan? Timeout { get; init; }
    public string? QuoteDate { get; init; }
    public string? Name { get; init; }
    public int? Amount { get; init; }
    public string? Token { get; init; }
}