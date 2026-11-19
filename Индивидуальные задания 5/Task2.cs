public class Student
{
    private static int createdCount;

    public static int CreatedCount => createdCount;

    static Student() { createdCount = 0; }

    public const int MaxGrade = 5;

    private readonly DateTime enrolledAt;

    public DateTime EnrolledAt => enrolledAt;

    public Student()
    {
        createdCount++;
        enrolledAt = DateTime.Now;
    }
}

public static class GradeCalculator { }

public readonly struct StudentCard { public string Number { get; init; } public int Course { get; init; } }

class Program
{
    static void Main()
    {
        Student first = new Student();
        Student second = new Student();

        Console.WriteLine($"Создано студентов: {Student.CreatedCount}");
        Console.WriteLine($"Максимальная оценка: {Student.MaxGrade}");
        Console.WriteLine($"Первый зачислен: {first.EnrolledAt}");
        Console.WriteLine($"Второй зачислен: {second.EnrolledAt}");

        StudentCard card = new StudentCard { Number = "Б-2417", Course = 2 };
        Console.WriteLine($"Студенческий билет: {card.Number}, курс {card.Course}");

        Console.WriteLine();
        Console.WriteLine("8. const вычисляется при компиляции, допускает только простые типы и подставляется в клиентский код; static readonly вычисляется при первом обращении к типу, допускает любые типы и может быть задан в статическом конструкторе.");
    }
}
