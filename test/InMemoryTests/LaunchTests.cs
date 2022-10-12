using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace InMemoryTests;

public class LaunchTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly ITestOutputHelper _output;
    private readonly HttpClient _client;

    public LaunchTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _output = output;
        _client = factory.CreateDefaultClient();
    }

    [Fact]
    public async Task Get_Ok()
    {
        var response = await _client.GetAsync("Test");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsStringAsync();
        _output.WriteLine(message);
    }
}