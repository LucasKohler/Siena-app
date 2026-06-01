using System.Text.Json;
using Siena.Application.Events;
using Siena.Domain.Events;

namespace Siena.Infrastructure.Events;

public sealed class JsonEventRepository : IEventRepository
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly string _dataPath;

    public JsonEventRepository()
    {
        _dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "events.json");
    }

    public async Task<IReadOnlyCollection<Event>> ListAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_dataPath))
        {
            throw new FileNotFoundException("Event data file was not found.", _dataPath);
        }

        await using var stream = File.OpenRead(_dataPath);
        var records = await JsonSerializer.DeserializeAsync<EventRecord[]>(
            stream,
            SerializerOptions,
            cancellationToken);

        return records?
            .Select(ToDomain)
            .ToArray() ?? [];
    }

    public async Task<Event?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var events = await ListAsync(cancellationToken);

        return events.FirstOrDefault(eventItem =>
            string.Equals(eventItem.Id, id, StringComparison.OrdinalIgnoreCase));
    }

    private static Event ToDomain(EventRecord record)
    {
        return new Event(
            record.Id,
            record.Title,
            ParseType(record.Type),
            ParseCategory(record.Category),
            record.StartsAt,
            record.Location,
            string.IsNullOrWhiteSpace(record.Opponent) ? null : record.Opponent,
            string.IsNullOrWhiteSpace(record.Description) ? null : record.Description);
    }

    private static EventType ParseType(string value)
    {
        return value.Trim() switch
        {
            "Liga Nacional" => EventType.LigaNacional,
            "Treino Físico" => EventType.TreinoFisico,
            "Amistoso" => EventType.Amistoso,
            _ => throw new InvalidDataException($"Unsupported event type '{value}'.")
        };
    }

    private static EventCategory ParseCategory(string value)
    {
        return value.Trim() switch
        {
            "Masculino" => EventCategory.Masculino,
            "Feminino" => EventCategory.Feminino,
            _ => throw new InvalidDataException($"Unsupported event category '{value}'.")
        };
    }
}

internal sealed record EventRecord(
    string Id,
    string Title,
    string Type,
    string Category,
    DateTimeOffset StartsAt,
    string Location,
    string? Opponent,
    string? Description);
