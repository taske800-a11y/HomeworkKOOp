using System;
using IndividualWork;

namespace IndividualWork;

internal class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Student first = new("Иван Петров");
        Student second = new("Мария Смирнова");
        Student third = new("Алексей Иванов");

        Console.WriteLine($"Студент 1: Id = {first.Id}, {first.FullName}, зачислен {first.EnrolledAt}");
        Console.WriteLine($"Студент 2: Id = {second.Id}, {second.FullName}");
        Console.WriteLine($"Студент 3: Id = {third.Id}, {third.FullName}");
        Console.WriteLine($"Всего создано студентов: {Student.CreatedCount}");
        Console.WriteLine($"Максимальная оценка: {Student.MaxGrade}");
    }
}