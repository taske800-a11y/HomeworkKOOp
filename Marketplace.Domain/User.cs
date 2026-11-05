namespace Marketplace.Domain;

public sealed class User
{
    public int Id { get; }
    public string FullName { get; }
    public string Login { get; }

    public User(int id, string fullName, string login)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("ФИО обязательно.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(login)) throw new ArgumentException("Логин обязателен.", nameof(login));
        Id = id;
        FullName = fullName;
        Login = login;
    }
}
