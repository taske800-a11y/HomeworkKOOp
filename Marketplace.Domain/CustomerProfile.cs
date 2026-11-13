namespace Marketplace.Domain;

public class CustomerProfile
{
    private string _email = string.Empty;
    private int _rating;

    public required string FullName { get; init; }
    public string Phone { get; set; } = string.Empty;

    public string Email
    {
        get => _email;
        set
        {
            EnsureValidEmail(value);
            _email = value;
        }
    }

    public int Rating
    {
        get => _rating;
        set
        {
            if (value is < 0 or > 5)
                throw new ArgumentOutOfRangeException(nameof(value), "Рейтинг должен быть от 0 до 5.");

            _rating = value;
        }
    }

    public bool IsTrusted => Rating >= 4;

    public string GetInfo() => GetInfo(includeRating: false);

    public string GetInfo(bool includeRating)
    {
        string info = $"Клиент: {FullName}, Email: {Email}";
        return includeRating ? $"{info}, рейтинг: {Rating}" : info;
    }

    public string GetContactInfo(bool includePhone)
    {
        string info = $"Email: {Email}";
        return includePhone ? $"{info}, телефон: {NormalizePhone(Phone)}" : info;
    }

    private static void EnsureValidEmail(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (!value.Contains('@'))
            throw new ArgumentException("Email должен содержать символ @.", nameof(value));
    }

    private static string NormalizePhone(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new string(value.Where(char.IsDigit).ToArray());
    }
}
