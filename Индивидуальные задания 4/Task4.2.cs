public class Student
{
    public string Group { get; set; } = "не распределён";

    public int Age { get; private set; }

    public bool IsJunior => Age < 20;

    public string CardNumber { get; init; } = string.Empty;

    public required string FullName { get; init; }

    public void SetAge(int age) => Age = age;
}

class Program
{
    static void Main()
    {
        Student student = new Student { FullName = "Анна Петрова", CardNumber = "Б-2417" };
        Console.WriteLine($"{student.FullName}: группа - {student.Group}, билет - {student.CardNumber}");

        student.SetAge(19);
        Console.WriteLine($"Возраст: {student.Age}, младше 20: {student.IsJunior}");

        student.Group = "ИТ-21";
        Console.WriteLine($"Группа после изменения: {student.Group}");

        Console.WriteLine();
        Console.WriteLine("6. Тип возврата не входит в сигнатуру метода, поэтому два метода GetInfo() с разными типами возврата имеют одну и ту же сигнатуру - ошибка CS0111.");
        Console.WriteLine("7. readonly struct запрещает изменять свои поля после создания, поэтому set не компилируется: допустимы init (значение задаётся при создании) или свойство только для чтения.");
    }
}
