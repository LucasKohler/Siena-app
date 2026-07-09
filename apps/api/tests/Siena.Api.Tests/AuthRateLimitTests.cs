using System.Net;
using System.Net.Http.Json;

namespace Siena.Api.Tests;

/// <summary>
/// Isolated from other auth tests so the fixed-window login limiter does not pollute shared fixtures.
/// </summary>
public sealed class AuthRateLimitTests : IClassFixture<SienaWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthRateLimitTests(SienaWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ExceedingRateLimit_ReturnsTooManyRequests()
    {
        HttpStatusCode? lastStatus = null;

        for (var attempt = 0; attempt < 21; attempt++)
        {
            using var response = await _client.PostAsJsonAsync(
                "/api/auth/login",
                new { phoneNumber = "+5511999999999" });

            lastStatus = response.StatusCode;
        }

        Assert.Equal(HttpStatusCode.TooManyRequests, lastStatus);
    }
}
