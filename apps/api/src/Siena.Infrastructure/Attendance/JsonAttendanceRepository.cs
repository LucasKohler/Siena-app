using System.Text.Json;
using Siena.Application.Trainings;
using Siena.Domain.Attendance;

namespace Siena.Infrastructure.Persistence;

public sealed class JsonAttendanceRepository : IAttendanceRepository
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    private static readonly SemaphoreSlim FileSemaphore = new(1, 1);
    private readonly string _dataPath;

    public JsonAttendanceRepository()
    {
        _dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "attendances.json");
    }

    public async Task<IReadOnlyCollection<Attendance>> ListByEventAsync(
        string eventId,
        CancellationToken cancellationToken)
    {
        var records = await LoadRecordsAsync(cancellationToken);

        return records
            .Where(record => string.Equals(record.EventId, eventId, StringComparison.Ordinal))
            .Select(ToDomain)
            .ToArray();
    }

    public async Task<Attendance?> GetAsync(
        string eventId,
        string userId,
        CancellationToken cancellationToken)
    {
        var records = await LoadRecordsAsync(cancellationToken);

        var record = records.FirstOrDefault(item =>
            string.Equals(item.EventId, eventId, StringComparison.Ordinal)
            && string.Equals(item.UserId, userId, StringComparison.Ordinal));

        return record is null ? null : ToDomain(record);
    }

    public async Task UpsertAsync(Attendance attendance, CancellationToken cancellationToken)
    {
        await FileSemaphore.WaitAsync(cancellationToken);

        try
        {
        var records = (await LoadRecordsAsync(cancellationToken)).ToList();
        var index = records.FindIndex(record =>
            string.Equals(record.EventId, attendance.EventId, StringComparison.Ordinal)
            && string.Equals(record.UserId, attendance.UserId, StringComparison.Ordinal));

        var record = new AttendanceRecord(
            attendance.EventId,
            attendance.UserId,
            TrainingMappings.ToLabel(attendance.Status),
            attendance.UpdatedAt);

        if (index >= 0)
        {
            records[index] = record;
        }
        else
        {
            records.Add(record);
        }

        await SaveRecordsAsync(records, cancellationToken);
        }
        finally
        {
            FileSemaphore.Release();
        }
    }

    private async Task<IReadOnlyCollection<AttendanceRecord>> LoadRecordsAsync(
        CancellationToken cancellationToken)
    {
        EnsureFileExists();

        await using var stream = File.OpenRead(_dataPath);
        var records = await JsonSerializer.DeserializeAsync<AttendanceRecord[]>(
            stream,
            SerializerOptions,
            cancellationToken);

        return records ?? [];
    }

    private async Task SaveRecordsAsync(
        IReadOnlyCollection<AttendanceRecord> records,
        CancellationToken cancellationToken)
    {
        await using var stream = File.Create(_dataPath);
        await JsonSerializer.SerializeAsync(
            stream,
            records,
            SerializerOptions,
            cancellationToken);
    }

    private void EnsureFileExists()
    {
        if (File.Exists(_dataPath))
        {
            return;
        }

        var directory = Path.GetDirectoryName(_dataPath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(_dataPath, "[]");
    }

    private static Attendance ToDomain(AttendanceRecord record)
    {
        return new Attendance(
            record.EventId,
            record.UserId,
            TrainingMappings.ParseStatus(record.Status),
            record.UpdatedAt);
    }
}

internal sealed record AttendanceRecord(
    string EventId,
    string UserId,
    string Status,
    DateTimeOffset UpdatedAt);
