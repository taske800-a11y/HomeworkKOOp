public class User
{
    // ид
    public int Id { get; set; }

    // фио
    public string FullName { get; set; }

    // логин
    public string Login { get; set; }

    // пароль
    public string Password { get; set; }

    // конструктор
    public User(int id, string fullName, string login, string password)
    {
        Id = id;
        FullName = fullName;
        Login = login;
        Password = password;
    }
}
