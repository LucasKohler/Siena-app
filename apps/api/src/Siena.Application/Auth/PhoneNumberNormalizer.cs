namespace Siena.Application.Auth;

public static class PhoneNumberNormalizer
{
    public static string Normalize(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return string.Empty;
        }

        return new string(phoneNumber
            .Trim()
            .Where(character => char.IsDigit(character) || character == '+')
            .ToArray());
    }
}
