using Siena.Application.Events;
using Siena.Domain.Attendance;
using Siena.Domain.Events;
using Siena.Domain.Users;

namespace Siena.Application.Trainings;

public sealed class AttendanceService : IAttendanceService
{
    private readonly IEventRepository _eventRepository;
    private readonly IAttendanceRepository _attendanceRepository;

    public AttendanceService(
        IEventRepository eventRepository,
        IAttendanceRepository attendanceRepository)
    {
        _eventRepository = eventRepository;
        _attendanceRepository = attendanceRepository;
    }

    public async Task<SetAttendanceResult> SetAttendanceAsync(
        string eventId,
        string userId,
        UserRole userRole,
        SetAttendanceRequest request,
        CancellationToken cancellationToken)
    {
        if (userRole != UserRole.Athlete)
        {
            return SetAttendanceResult.Forbidden;
        }

        var eventItem = await _eventRepository.GetByIdAsync(eventId, cancellationToken);

        if (eventItem is null)
        {
            return SetAttendanceResult.EventNotFound;
        }

        if (eventItem.Type != EventType.TreinoFisico)
        {
            return SetAttendanceResult.NotATrainingEvent;
        }

        if (eventItem.StartsAt <= DateTimeOffset.UtcNow)
        {
            return SetAttendanceResult.TrainingAlreadyStarted;
        }

        AttendanceStatus status;

        try
        {
            status = TrainingMappings.ParseStatus(request.Status);
        }
        catch (ArgumentException)
        {
            return SetAttendanceResult.InvalidStatus;
        }

        var attendance = new Attendance(
            eventId,
            userId,
            status,
            DateTimeOffset.UtcNow);

        await _attendanceRepository.UpsertAsync(attendance, cancellationToken);

        return SetAttendanceResult.Success;
    }
}
