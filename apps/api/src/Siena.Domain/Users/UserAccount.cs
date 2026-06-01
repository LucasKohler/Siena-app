namespace Siena.Domain.Users;

public sealed class UserAccount
{
    public UserAccount(
        string id,
        string phoneNumber,
        string displayName,
        UserRole role,
        PlayerPosition? position = null)
    {
        Id = id;
        PhoneNumber = phoneNumber;
        DisplayName = displayName;
        Role = role;
        Position = position;
    }

    public string Id { get; }
    public string PhoneNumber { get; }
    public string DisplayName { get; }
    public UserRole Role { get; }
    public PlayerPosition? Position { get; }
}
