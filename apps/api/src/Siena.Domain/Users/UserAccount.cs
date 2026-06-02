namespace Siena.Domain.Users;

public sealed class UserAccount
{
    public UserAccount(
        string id,
        string phoneNumber,
        string displayName,
        UserRole role,
        bool isActive = true,
        PlayerPosition? position = null)
    {
        Id = id;
        PhoneNumber = phoneNumber;
        DisplayName = displayName;
        Role = role;
        IsActive = isActive;
        Position = position;
    }

    public string Id { get; }
    public string PhoneNumber { get; }
    public string DisplayName { get; }
    public UserRole Role { get; }
    public bool IsActive { get; }
    public PlayerPosition? Position { get; }
}
