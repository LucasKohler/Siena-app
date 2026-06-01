using Siena.Domain.Attendance;
using Siena.Domain.Users;

namespace Siena.Application.Trainings;

public static class TrainingMappings
{
    public static string ToLabel(AttendanceStatus status)
    {
        return status switch
        {
            AttendanceStatus.Attending => "Eu vou",
            AttendanceStatus.NotAttending => "Não vou",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }

    public static AttendanceStatus ParseStatus(string value)
    {
        return value.Trim() switch
        {
            "Eu vou" => AttendanceStatus.Attending,
            "Não vou" => AttendanceStatus.NotAttending,
            _ => throw new ArgumentException($"Unsupported attendance status '{value}'.", nameof(value))
        };
    }

    public static string ToLabel(PlayerPosition position)
    {
        return position switch
        {
            PlayerPosition.Levantadora => "Levantadora",
            PlayerPosition.Ponteiro => "Ponteiro",
            PlayerPosition.Central => "Central",
            PlayerPosition.Libero => "Líbero",
            _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
        };
    }
}
