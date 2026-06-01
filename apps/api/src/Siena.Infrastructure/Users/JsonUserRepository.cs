using System.Text.Json;
using Siena.Application.Auth;
using Siena.Domain.Users;

namespace Siena.Infrastructure.Users;

public sealed class JsonUserRepository : IUserRepository
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly string _dataPath;

    public JsonUserRepository()
    {
        _dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "users.json");
    }

    public async Task<UserAccount?> FindByPhoneAsync(
        string phoneNumber,
        CancellationToken cancellationToken)
    {
        var users = await LoadUsersAsync(cancellationToken);
        var normalizedPhone = PhoneNumberNormalizer.Normalize(phoneNumber);

        return users.FirstOrDefault(user =>
            string.Equals(
                PhoneNumberNormalizer.Normalize(user.PhoneNumber),
                normalizedPhone,
                StringComparison.Ordinal));
    }

    public async Task<UserAccount?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var users = await LoadUsersAsync(cancellationToken);

        return users.FirstOrDefault(user =>
            string.Equals(user.Id, id, StringComparison.Ordinal));
    }

    private async Task<IReadOnlyCollection<UserAccount>> LoadUsersAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_dataPath))
        {
            throw new FileNotFoundException("User data file was not found.", _dataPath);
        }

        await using var stream = File.OpenRead(_dataPath);
        var records = await JsonSerializer.DeserializeAsync<UserRecord[]>(
            stream,
            SerializerOptions,
            cancellationToken);

        return records?
            .Select(ToDomain)
            .ToArray() ?? [];
    }

    private static UserAccount ToDomain(UserRecord record)
    {
        return new UserAccount(
            record.Id,
            record.PhoneNumber,
            record.DisplayName,
            ParseRole(record.Role),
            ParsePosition(record.Position));
    }

    private static PlayerPosition? ParsePosition(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim() switch
        {
            "Levantadora" => PlayerPosition.Levantadora,
            "Ponteiro" => PlayerPosition.Ponteiro,
            "Central" => PlayerPosition.Central,
            "Líbero" => PlayerPosition.Libero,
            _ => throw new InvalidDataException($"Unsupported player position '{value}'.")
        };
    }

    private static UserRole ParseRole(string value)
    {
        return value.Trim() switch
        {
            "Atleta" => UserRole.Athlete,
            "Comissão" => UserRole.Coach,
            "Administrador" => UserRole.Admin,
            _ => throw new InvalidDataException($"Unsupported user role '{value}'.")
        };
    }
}

internal sealed record UserRecord(
    string Id,
    string PhoneNumber,
    string DisplayName,
    string Role,
    string? Position = null);
