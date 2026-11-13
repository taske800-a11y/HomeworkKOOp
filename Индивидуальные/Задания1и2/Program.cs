using IndividualAssignments;

StudentProgress progress = new()
{
    Subject = "ООП",
    TeacherName = "Иванова Ирина",
    SemesterPoints = 20
};

Console.WriteLine("Задание 1. StudentProgress");
Console.WriteLine(progress.GetInfo());
Console.WriteLine($"Нужна пересдача: {progress.NeedsRetake}");
Console.WriteLine($"GetInfo(false): {progress.GetInfo(false)}");

try
{
    progress.SemesterPoints = -10;
    Console.WriteLine("ОШИБКА: отрицательные баллы приняты.");
}
catch (ArgumentOutOfRangeException exception)
{
    Console.WriteLine($"Проверка SemesterPoints: исключение обработано ({exception.Message})");
}

Console.WriteLine();
Console.WriteLine("Задание 2. Ответы одной строкой кода:");
Console.WriteLine("""public string Group { get; set; } = "не распределён";""");
Console.WriteLine("public int Age { get; private set; }");
Console.WriteLine("public bool IsJunior => Age < 20;");
Console.WriteLine("public string CardNumber { get; init; }");
Console.WriteLine("public required string FullName { get; init; }");
Console.WriteLine("Тип возврата не входит в сигнатуру метода, поэтому перегрузка только типом возврата невозможна (CS0111).");
Console.WriteLine("В readonly struct set запрещён: он может изменять состояние после создания; допустимы init или get.");
