namespace Siena.Application.Trainings;

public sealed record NextTrainingDto(
    string EventId,
    string Title,
    string Category,
    DateTimeOffset StartsAt,
    string Location,
    string? MyStatus,
    string? MyApprovalStatus,
    IReadOnlyCollection<ConfirmedAttendeeDto> Confirmed);

public sealed record ConfirmedAttendeeDto(string DisplayName, string? Position);

public sealed record SetAttendanceRequest(string Status);
