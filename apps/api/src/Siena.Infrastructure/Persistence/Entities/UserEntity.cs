namespace Siena.Infrastructure.Persistence.Entities;

public sealed class UserEntity
{
    public string Id { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string? Position { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<AttendanceEntity> Attendances { get; set; } = [];
}
