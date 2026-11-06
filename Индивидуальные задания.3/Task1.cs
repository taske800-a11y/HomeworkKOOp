public class Student
{
    private readonly string firstName;
    private readonly string lastName;
    private int age;
    private readonly double averageMark;

    public Student(string firstName, string lastName, int age, double averageMark)
    {
        EnsureNotEmpty(firstName, nameof(firstName));
        EnsureNotEmpty(lastName, nameof(lastName));
        EnsureValidAge(age);
        EnsureValidMark(averageMark);

        this.firstName = firstName;
        this.lastName = lastName;
        this.age = age;
        this.averageMark = averageMark;
    }

    public string GetFullName()
    {
        return $"{firstName} {lastName}";
    }

    public int GetAge()
    {
        return age;
    }

    public double GetAverageMark()
    {
        return averageMark;
    }

    public void SetAge(int newAge)
    {
        EnsureValidAge(newAge);
        age = newAge;
    }

    public string GetInfo()
    {
        return $"{GetFullName()}, {age} лет, средний балл: {averageMark:0.0}";
    }

    private static void EnsureNotEmpty(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Значение обязательно.", paramName);
        }
    }

    private static void EnsureValidAge(int value)
    {
        if (value < 14 || value > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value, "Возраст должен быть от 14 до 100.");
        }
    }

    private static void EnsureValidMark(double value)
    {
        if (!(value >= 0.0 && value <= 5.0))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value, "Средний балл должен быть от 0 до 5.");
        }
    }
}

class Program
{
    static void Main()
    {
        Student student = new Student("Анна", "Петрова", 20, 4.5);
        Console.WriteLine(student.GetInfo());

        TryCreate("Иван", "Сидоров", -3, 4.0);
        TryCreate("Иван", "", 20, 4.0);
        TryCreate("Иван", "Сидоров", 20, 5.5);

        try
        {
            student.SetAge(200);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"SetAge(200) отклонён: {ex.Message}");
        }
        Console.WriteLine($"Возраст после SetAge(200): {student.GetAge()}");

        student.SetAge(21);
        Console.WriteLine(student.GetInfo());
    }

    static void TryCreate(string firstName, string lastName, int age, double averageMark)
    {
        try
        {
            Student student = new Student(firstName, lastName, age, averageMark);
            Console.WriteLine($"Создан: {student.GetInfo()}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Студент не создан: {ex.Message}");
        }
    }
}
