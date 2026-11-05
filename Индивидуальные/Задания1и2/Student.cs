namespace Assignment1And2;

public sealed class Student
{
    private readonly string _firstName;
    private readonly string _lastName;
    private int _age;
    private readonly double _averageMark;

    public Student(string firstName, string lastName, int age, double averageMark)
    {
        EnsureNotEmpty(firstName, nameof(firstName));
        EnsureNotEmpty(lastName, nameof(lastName));
        EnsureValidAge(age);
        EnsureValidAverageMark(averageMark);

        _firstName = firstName.Trim();
        _lastName = lastName.Trim();
        _age = age;
        _averageMark = averageMark;
    }

    public string GetFullName() => $"{_firstName} {_lastName}";

    public int GetAge() => _age;

    public double GetAverageMark() => _averageMark;

    public void SetAge(int newAge)
    {
        EnsureValidAge(newAge);
        _age = newAge;
    }

    public string GetInfo() =>
        $"{GetFullName()}, {GetAge()} лет, средний балл: {GetAverageMark():0.0}";

    private static void EnsureNotEmpty(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Значение обязательно.", parameterName);
    }

    private static void EnsureValidAge(int age)
    {
        if (age is < 14 or > 100)
            throw new ArgumentOutOfRangeException(
                nameof(age), "Возраст должен быть от 14 до 100 лет.");
    }

    private static void EnsureValidAverageMark(double averageMark)
    {
        if (double.IsNaN(averageMark) || double.IsInfinity(averageMark) ||
            averageMark is < 0 or > 5)
        {
            throw new ArgumentOutOfRangeException(
                nameof(averageMark), "Средний балл должен быть от 0 до 5.");
        }
    }
}
