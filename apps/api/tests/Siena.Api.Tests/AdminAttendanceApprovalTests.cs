using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Siena.Api.Tests;

public sealed class AdminAttendanceApprovalTests : IClassFixture<SienaWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AdminAttendanceApprovalTests(SienaWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ApproveAttendance_AthleteAppearsInConfirmedList()
    {
        var athleteToken = await TestAuth.LoginAsync(_client, "+5511999990003");

        using var postRequest = TestAuth.WithBearer(
            HttpMethod.Post,
            "/api/trainings/treino-fisico-2026-06-15/attendance",
            athleteToken);
        postRequest.Content = JsonContent.Create(new { status = "Eu vou" });

        using var postResponse = await _client.SendAsync(postRequest);
        Assert.Equal(HttpStatusCode.NoContent, postResponse.StatusCode);

        using var getBeforeApproval = TestAuth.WithBearer(
            HttpMethod.Get,
            "/api/trainings/next",
            athleteToken);

        using var beforeResponse = await _client.SendAsync(getBeforeApproval);
        Assert.Equal(HttpStatusCode.OK, beforeResponse.StatusCode);

        await using var beforeStream = await beforeResponse.Content.ReadAsStreamAsync();
        using var beforeDocument = await JsonDocument.ParseAsync(beforeStream);

        var beforeTraining = beforeDocument.RootElement;
        Assert.Equal("Eu vou", beforeTraining.GetProperty("myStatus").GetString());
        Assert.Equal("Pendente", beforeTraining.GetProperty("myApprovalStatus").GetString());
        Assert.Empty(beforeTraining.GetProperty("confirmed").EnumerateArray());

        var coachToken = await TestAuth.LoginAsync(_client, "+5511999990002");

        using var approveRequest = TestAuth.WithBearer(
            HttpMethod.Post,
            "/api/admin/trainings/treino-fisico-2026-06-15/attendances/user-athlete-dev-1/approval",
            coachToken);
        approveRequest.Content = JsonContent.Create(new { action = "approve" });

        using var approveResponse = await _client.SendAsync(approveRequest);
        Assert.Equal(HttpStatusCode.NoContent, approveResponse.StatusCode);

        using var getAfterApproval = TestAuth.WithBearer(
            HttpMethod.Get,
            "/api/trainings/next",
            athleteToken);

        using var afterResponse = await _client.SendAsync(getAfterApproval);
        Assert.Equal(HttpStatusCode.OK, afterResponse.StatusCode);

        await using var afterStream = await afterResponse.Content.ReadAsStreamAsync();
        using var afterDocument = await JsonDocument.ParseAsync(afterStream);

        var afterTraining = afterDocument.RootElement;
        Assert.Equal("Aprovado", afterTraining.GetProperty("myApprovalStatus").GetString());

        var confirmed = afterTraining.GetProperty("confirmed");
        Assert.Contains(
            confirmed.EnumerateArray(),
            attendee => attendee.GetProperty("displayName").GetString() == "Atleta DEV 1");
    }

    [Fact]
    public async Task RejectAttendance_DoesNotAppearInConfirmedList()
    {
        var athleteToken = await TestAuth.LoginAsync(_client, "+5511999990004");

        using var postRequest = TestAuth.WithBearer(
            HttpMethod.Post,
            "/api/trainings/treino-fisico-2026-06-15/attendance",
            athleteToken);
        postRequest.Content = JsonContent.Create(new { status = "Eu vou" });

        using var postResponse = await _client.SendAsync(postRequest);
        Assert.Equal(HttpStatusCode.NoContent, postResponse.StatusCode);

        var adminToken = await TestAuth.LoginAsync(_client, "+5511999990001");

        using var rejectRequest = TestAuth.WithBearer(
            HttpMethod.Post,
            "/api/admin/trainings/treino-fisico-2026-06-15/attendances/user-athlete-dev-2/approval",
            adminToken);
        rejectRequest.Content = JsonContent.Create(new { action = "reject" });

        using var rejectResponse = await _client.SendAsync(rejectRequest);
        Assert.Equal(HttpStatusCode.NoContent, rejectResponse.StatusCode);

        using var getRequest = TestAuth.WithBearer(HttpMethod.Get, "/api/trainings/next", athleteToken);

        using var getResponse = await _client.SendAsync(getRequest);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        await using var stream = await getResponse.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var confirmed = document.RootElement.GetProperty("confirmed");
        Assert.DoesNotContain(
            confirmed.EnumerateArray(),
            attendee => attendee.GetProperty("displayName").GetString() == "Atleta DEV 2");
    }
}
