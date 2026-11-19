public class Student
{
    public const int MaxGrade = 5;

    private static int createdCount;

    public static int CreatedCount => createdCount;

    public int Id { get; }

    public string FullName { get; }

    private readonly DateTime enrolledAt;

    public DateTime EnrolledAt => enrolledAt;

    public Student(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("ФИО студента не может быть пустым.", nameof(fullName));
        }

        FullName = fullName;
        Id = ++createdCount;
        enrolledAt = DateTime.Now;
    }
}

class Program
{
    static void Main()
    {
        Student first = new Student("Анна Петрова");
        Student second = new Student("Иван Сидоров");
        Student third = new Student("Мария Орлова");

        foreach (Student student in new[] { first, second, third })
        {
            Console.WriteLine($"Id: {student.Id}, {student.FullName}, зачислен: {student.EnrolledAt}");
        }

        Console.WriteLine($"Создано студентов: {Student.CreatedCount}");
        Console.WriteLine($"Максимальная оценка: {Student.MaxGrade}");

        try
        {
            Student invalid = new Student("   ");
            Console.WriteLine($"Создан: {invalid.FullName}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Студент не создан: {ex.Message}");
        }

        Console.WriteLine($"Создано студентов после неудачной попытки: {Student.CreatedCount}");
    }
}
