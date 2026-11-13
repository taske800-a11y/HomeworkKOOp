public class StudentProgress
{
    public required string Subject { get; init; }

    private string teacherName = string.Empty;
    public string TeacherName
    {
        get => teacherName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Имя преподавателя не может быть пустым.", nameof(value));
            }
            teacherName = value;
        }
    }

    private int semesterPoints;
    public int SemesterPoints
    {
        get => semesterPoints;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Баллы за семестр не могут быть отрицательными.");
            }
            semesterPoints = value;
        }
    }

    public bool NeedsRetake => SemesterPoints < 25;

    public string GetInfo() => GetInfo(true);

    public string GetInfo(bool includeIntProperty)
    {
        string info = $"Предмет: {Subject}, преподаватель: {TeacherName}";
        if (includeIntProperty)
        {
            string retake = NeedsRetake ? "да" : "нет";
            info += $", баллы за семестр: {SemesterPoints}, нужна пересдача: {retake}";
        }
        return info;
    }
}

public readonly struct StudentProgressRecord
{
    public required string Subject { get; init; }

    private readonly string? teacherName;
    public string TeacherName
    {
        get => teacherName ?? string.Empty;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Имя преподавателя не может быть пустым.", nameof(value));
            }
            teacherName = value;
        }
    }

    private readonly int semesterPoints;
    public int SemesterPoints
    {
        get => semesterPoints;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Баллы за семестр не могут быть отрицательными.");
            }
            semesterPoints = value;
        }
    }

    public bool NeedsRetake => SemesterPoints < 25;

    public string GetInfo() => GetInfo(true);

    public string GetInfo(bool includeIntProperty)
    {
        string info = $"Предмет: {Subject}, преподаватель: {TeacherName}";
        if (includeIntProperty)
        {
            string retake = NeedsRetake ? "да" : "нет";
            info += $", баллы за семестр: {SemesterPoints}, нужна пересдача: {retake}";
        }
        return info;
    }
}

class Program
{
    static void Main()
    {
        DemonstrateClass();
        Console.WriteLine();
        DemonstrateStruct();
    }

    static void DemonstrateClass()
    {
        Console.WriteLine("--- Класс StudentProgress ---");

        var progress = new StudentProgress
        {
            Subject = "ООП",
            TeacherName = "Иван Петров",
            SemesterPoints = 18
        };

        Console.WriteLine(progress.GetInfo());
        Console.WriteLine(progress.GetInfo(false));
        Console.WriteLine($"Нужна пересдача при 18 баллах: {progress.NeedsRetake}");

        progress.SemesterPoints = 60;
        Console.WriteLine($"Нужна пересдача при 60 баллах: {progress.NeedsRetake}");

        try
        {
            progress.SemesterPoints = -10;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"SemesterPoints = -10 отклонено: {ex.Message}");
        }
        Console.WriteLine($"Баллы после попытки: {progress.SemesterPoints}");

        try
        {
            progress.TeacherName = "   ";
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Пустое имя преподавателя отклонено: {ex.Message}");
        }
        Console.WriteLine($"Преподаватель после попытки: {progress.TeacherName}");
    }

    static void DemonstrateStruct()
    {
        Console.WriteLine("--- readonly struct StudentProgressRecord ---");

        var record = new StudentProgressRecord
        {
            Subject = "Алгоритмы",
            TeacherName = "Мария Орлова",
            SemesterPoints = 40
        };

        Console.WriteLine(record.GetInfo());
        Console.WriteLine(record.GetInfo(false));

        try
        {
            var invalid = new StudentProgressRecord
            {
                Subject = "Алгоритмы",
                TeacherName = "Мария Орлова",
                SemesterPoints = -10
            };
            Console.WriteLine(invalid.GetInfo());
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"SemesterPoints = -10 отклонено при создании: {ex.Message}");
        }
    }
}
