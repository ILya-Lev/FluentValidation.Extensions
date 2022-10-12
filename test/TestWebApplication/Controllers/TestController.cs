using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Internal;

namespace TestWebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ISystemClock _clock;

    public TestController(ISystemClock clock) => _clock = clock;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await Task.Yield();
        return Ok($"{_clock.UtcNow:R}");
    }
}