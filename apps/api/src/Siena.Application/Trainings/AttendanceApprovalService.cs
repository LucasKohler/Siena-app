using Siena.Application.Events;
using Siena.Domain.Attendance;
using Siena.Domain.Events;

namespace Siena.Application.Trainings;

public sealed class AttendanceApprovalService : IAttendanceApprovalService
{
    private readonly IEventRepository _eventRepository;
    private readonly IAttendanceRepository _attendanceRepository;

    public AttendanceApprovalService(
        IEventRepository eventRepository,
        IAttendanceRepository attendanceRepository)
    {
        _eventRepository = eventRepository;
        _attendanceRepository = attendanceRepository;
    }

    public async Task<SetApprovalResult> SetApprovalAsync(
        string eventId,
        string userId,
        string action,
        CancellationToken cancellationToken)
    {
        var eventItem = await _eventRepository.GetByIdAsync(eventId, cancellationToken);

        if (eventItem is null)
        {
            return SetApprovalResult.EventNotFound;
        }

        if (eventItem.Type != EventType.TreinoFisico)
        {
            return SetApprovalResult.NotATrainingEvent;
        }

        var attendance = await _attendanceRepository.GetAsync(eventId, userId, cancellationToken);

        if (attendance is null)
        {
            return SetApprovalResult.AttendanceNotFound;
        }

        if (attendance.ApprovalStatus != AttendanceApprovalStatus.Pending)
        {
            return SetApprovalResult.NotPending;
        }

        AttendanceApprovalStatus approvalStatus;

        try
        {
            approvalStatus = ParseAction(action);
        }
        catch (ArgumentException)
        {
            return SetApprovalResult.InvalidAction;
        }

        await _attendanceRepository.SetApprovalAsync(
            eventId,
            userId,
            approvalStatus,
            cancellationToken);

        return SetApprovalResult.Success;
    }

    private static AttendanceApprovalStatus ParseAction(string action) =>
        action.Trim().ToLowerInvariant() switch
        {
            "approve" => AttendanceApprovalStatus.Approved,
            "reject" => AttendanceApprovalStatus.Rejected,
            _ => throw new ArgumentException($"Unsupported approval action '{action}'.", nameof(action))
        };
}
