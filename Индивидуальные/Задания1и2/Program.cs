using Assignment1And2;

Console.WriteLine("Задание 1: Student");

var student = new Student("Анна", "Петрова", 20, 4.8);
Console.WriteLine(student.GetInfo());

try
{
    _ = new Student("Иван", "Иванов", -3, 4.0);
}
catch (ArgumentOutOfRangeException exception)
{
    Console.WriteLine($"Возраст -3 отклонён: {exception.Message}");
}

int ageBeforeInvalidUpdate = student.GetAge();
try
{
    student.SetAge(200);
}
catch (ArgumentOutOfRangeException exception)
{
    Console.WriteLine($"Возраст 200 отклонён: {exception.Message}");
}

Console.WriteLine(
    $"После SetAge(200) возраст не изменился: {student.GetAge() == ageBeforeInvalidUpdate}");

try
{
    _ = new Student("Иван", "", 20, 4.0);
}
catch (ArgumentException exception)
{
    Console.WriteLine($"Пустая фамилия отклонена: {exception.Message}");
