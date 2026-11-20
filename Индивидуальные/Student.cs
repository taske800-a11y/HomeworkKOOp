using System;

namespace IndividualWork;

public class Student
{
    // 1. Константа: максимальная оценка.
    public const int MaxGrade = 5;

    // 2. Закрытый статический счётчик созданных студентов.
    private static int createdCount;

    // 3. Открытое статическое свойство только для чтения.
    public static int CreatedCount => createdCount;

    // 4. Уникальный номер студента, задаётся при создании.
    public int Id { get; }

    public string FullName { get; }

    // 5. Поле только для чтения: дата зачисления.
    private readonly DateTime enrolledAt;

    public DateTime EnrolledAt => enrolledAt;

    public Student(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Имя студента не может быть пустым.", nameof(fullName));

        FullName = fullName.Trim();
        Id = ++createdCount;
        enrolledAt = DateTime.Now;
    }
}