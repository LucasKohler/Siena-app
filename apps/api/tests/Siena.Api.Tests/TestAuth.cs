using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Siena.Api.Tests;

internal static class TestAuth
{
    public static async Task<string> LoginAsync(HttpClient client, string phoneNumber)
    {
        using var response = await client.PostAsJsonAsync(
            "/api/auth/login",
            new { phoneNumber });

        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);
        var token = document.RootElement.GetProperty("token").GetString();

        return token ?? throw new InvalidOperationException("Login did not return a token.");
    }

    public static HttpRequestMessage WithBearer(HttpMethod method, string url, string token)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return request;
    }
}
