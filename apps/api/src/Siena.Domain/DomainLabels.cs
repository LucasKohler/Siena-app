using Siena.Domain.Attendance;
using Siena.Domain.Events;
using Siena.Domain.Users;

namespace Siena.Domain;

/// <summary>
/// Labels PT únicos para persistência e API (evita drift entre camadas).
/// </summary>
public static class DomainLabels
{
    public static string ToLabel(UserRole role) =>
        role switch
        {
            UserRole.Athlete => "Atleta",
            UserRole.Coach => "Comissão",
            UserRole.Admin => "Administrador",
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };

    public static UserRole ParseRole(string value) =>
        value.Trim() switch
        {
            "Atleta" => UserRole.Athlete,
            "Comissão" => UserRole.Coach,
            "Administrador" => UserRole.Admin,
            _ => throw new ArgumentException($"Unsupported user role '{value}'.", nameof(value))
        };

    public static string ToRoleClaimValue(UserRole role) =>
        role switch
        {
            UserRole.Athlete => nameof(UserRole.Athlete),
            UserRole.Coach => nameof(UserRole.Coach),
            UserRole.Admin => nameof(UserRole.Admin),
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };

    public static string ToLabelFromRoleClaim(string claimValue) =>
        claimValue switch
        {
            nameof(UserRole.Athlete) => ToLabel(UserRole.Athlete),
            nameof(UserRole.Coach) => ToLabel(UserRole.Coach),
            nameof(UserRole.Admin) => ToLabel(UserRole.Admin),
            _ => claimValue
        };

    public static string ToLabel(PlayerPosition position) =>
        position switch
        {
            PlayerPosition.Levantadora => "Levantadora",
            PlayerPosition.Ponteiro => "Ponteiro",
            PlayerPosition.Central => "Central",
            PlayerPosition.Libero => "Líbero",
            _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
        };

    public static PlayerPosition? ParsePosition(string? value)
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
            _ => throw new ArgumentException($"Unsupported player position '{value}'.", nameof(value))
        };
    }

    public static string ToLabel(EventType type) =>
        type switch
        {
            EventType.LigaNacional => "Liga Nacional",
            EventType.TreinoFisico => "Treino Físico",
            EventType.Amistoso => "Amistoso",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

    public static EventType ParseEventType(string value) =>
        value.Trim() switch
        {
            "Liga Nacional" => EventType.LigaNacional,
            "Treino Físico" => EventType.TreinoFisico,
            "Amistoso" => EventType.Amistoso,
            _ => throw new ArgumentException($"Unsupported event type '{value}'.", nameof(value))
        };

    public static string ToLabel(EventCategory category) =>
        category switch
        {
            EventCategory.Masculino => "Masculino",
            EventCategory.Feminino => "Feminino",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };

    public static EventCategory ParseEventCategory(string value) =>
        value.Trim() switch
        {
            "Masculino" => EventCategory.Masculino,
            "Feminino" => EventCategory.Feminino,
            _ => throw new ArgumentException($"Unsupported event category '{value}'.", nameof(value))
        };

    public static string ToLabel(AttendanceStatus status) =>
        status switch
        {
            AttendanceStatus.Attending => "Eu vou",
            AttendanceStatus.NotAttending => "Não vou",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };

    public static AttendanceStatus ParseAttendanceStatus(string value) =>
        value.Trim() switch
        {
            "Eu vou" => AttendanceStatus.Attending,
            "Não vou" => AttendanceStatus.NotAttending,
            _ => throw new ArgumentException($"Unsupported attendance status '{value}'.", nameof(value))
        };

    public static string ToLabel(AttendanceApprovalStatus status) =>
        status switch
        {
            AttendanceApprovalStatus.Pending => "Pendente",
            AttendanceApprovalStatus.Approved => "Aprovado",
            AttendanceApprovalStatus.Rejected => "Rejeitado",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };

    public static AttendanceApprovalStatus ParseApprovalStatus(string value) =>
        value.Trim() switch
        {
            "Pendente" => AttendanceApprovalStatus.Pending,
            "Aprovado" => AttendanceApprovalStatus.Approved,
            "Rejeitado" => AttendanceApprovalStatus.Rejected,
            _ => throw new ArgumentException($"Unsupported approval status '{value}'.", nameof(value))
        };
}
