using Siena.Application.Auth;
using Siena.Application.Events;
using Siena.Domain;
using Siena.Domain.Attendance;
using Siena.Domain.Events;

namespace Siena.Application.Trainings;

public sealed class TrainingQueryService : ITrainingQueryService
{
    private readonly IEventRepository _eventRepository;
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IUserRepository _userRepository;

    public TrainingQueryService(
        IEventRepository eventRepository,
        IAttendanceRepository attendanceRepository,
        IUserRepository userRepository)
    {
        _eventRepository = eventRepository;
        _attendanceRepository = attendanceRepository;
        _userRepository = userRepository;
    }

    public async Task<NextTrainingDto?> GetNextTrainingAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        var events = await _eventRepository.ListAsync(cancellationToken);
        var now = DateTimeOffset.UtcNow;

        var nextTraining = events
            .Where(eventItem => eventItem.Type == EventType.TreinoFisico)
            .Where(eventItem => eventItem.StartsAt >= now)
            .OrderBy(eventItem => eventItem.StartsAt)
            .FirstOrDefault();

        if (nextTraining is null)
        {
            return null;
        }

        var attendances = await _attendanceRepository.ListByEventAsync(
            nextTraining.Id,
            cancellationToken);

        var myAttendance = attendances.FirstOrDefault(attendance =>
            string.Equals(attendance.UserId, userId, StringComparison.Ordinal));

        var confirmed = await BuildConfirmedListAsync(
            attendances,
            cancellationToken);

        return new NextTrainingDto(
            nextTraining.Id,
            nextTraining.Title,
            DomainLabels.ToLabel(nextTraining.Category),
            nextTraining.StartsAt,
            nextTraining.Location,
            myAttendance is null ? null : DomainLabels.ToLabel(myAttendance.Status),
            myAttendance is null ? null : DomainLabels.ToLabel(myAttendance.ApprovalStatus),
            confirmed);
    }

    private async Task<IReadOnlyCollection<ConfirmedAttendeeDto>> BuildConfirmedListAsync(
        IReadOnlyCollection<Attendance> attendances,
        CancellationToken cancellationToken)
    {
        var attending = attendances
            .Where(attendance =>
                attendance.Status == AttendanceStatus.Attending
                && attendance.ApprovalStatus == AttendanceApprovalStatus.Approved)
            .ToArray();

        var confirmed = new List<ConfirmedAttendeeDto>();

        foreach (var attendance in attending)
        {
            var user = await _userRepository.GetByIdAsync(attendance.UserId, cancellationToken);

            if (user is null)
            {
                continue;
            }

            confirmed.Add(new ConfirmedAttendeeDto(
                user.DisplayName,
                user.Position is null ? null : DomainLabels.ToLabel(user.Position.Value)));
        }

        return confirmed
            .OrderBy(attendee => attendee.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }
}
