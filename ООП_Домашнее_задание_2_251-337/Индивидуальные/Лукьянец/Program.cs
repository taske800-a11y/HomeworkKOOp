using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("=== Индивидуальные задания Лукьянца Владислава Максимовича ===");

Console.WriteLine("\n--- Задание 1: Auditorium ---");
Auditorium lecture = new("205", "Главный корпус", 90);
Console.WriteLine(lecture.Describe());
Auditorium defaultValue = default;
Console.WriteLine($"default: {defaultValue.Describe()} (поля получают значения по умолчанию)");
Auditorium copied = lecture;
copied.Seats = 45;
Console.WriteLine($"Оригинал: {lecture.Describe()}");
Console.WriteLine($"Копия: {copied.Describe()}");
Auditorium withCopy = lecture with { Seats = 50 };
Console.WriteLine($"with-копия: {withCopy.Describe()}");
Show(lecture);
ReserveByValue(lecture, 5);
Console.WriteLine($"После ReserveByValue: {lecture.Seats} (оригинал не изменился)");
Reserve(ref lecture, 10);
Console.WriteLine($"После Reserve(ref): {lecture.Describe()}");

Console.WriteLine("\n--- Задание 2: ссылка класса и копия структуры ---");
var student1 = new Student("Анна", 20);
var student2 = student1;
Console.WriteLine($"До: {student1}; {student2}");
student2.Age = 21;
Console.WriteLine($"После изменения через вторую ссылку: {student1}; {student2}");

StudentCard card1 = new("Б-1001", 2);
StudentCard card2 = card1;
Console.WriteLine($"До: {card1}; {card2}");
card2.Course = 3;
Console.WriteLine($"После изменения копии: оригинал={card1}; копия={card2}");

Console.WriteLine("\nОтветы:");
Console.WriteLine("1. Объект класса находится в управляемой куче, локальный int — значимый тип; конкретное размещение зависит от контекста и оптимизаций.");
Console.WriteLine("2. Сборщик мусора может освободить объект, когда он недостижим из корней управляемой памяти.");
Console.WriteLine("3. Структура копирует все поля, но поле-ссылка продолжает указывать на тот же объект.");

static void Show(Auditorium auditorium) => Console.WriteLine($"Show: {auditorium.Describe()}");
static void ReserveByValue(Auditorium auditorium, int seats) => auditorium.Seats -= seats;

static void Reserve(ref Auditorium auditorium, int seats)
{
    if (seats <= 0 || seats > auditorium.Seats) throw new ArgumentOutOfRangeException(nameof(seats));
    auditorium.Seats -= seats;
}

struct Auditorium
{
    public string? Number;
    public string? Building;
    public int Seats;
    public Auditorium(string number, string building, int seats)
    {
        Number = number;
        Building = building;
        Seats = seats;
    }
    public Auditorium(string number) : this(number, "Главный корпус", 0) { }
    public string Describe() => $"ауд. {Number ?? "(null)"}, {Building ?? "(null)"}, мест: {Seats}";
}

sealed class Student
{
    public string Name { get; }
    public int Age { get; set; }
    public Student(string name, int age) { Name = name; Age = age; }
    public override string ToString() => $"{Name}, {Age} лет";
}

struct StudentCard
{
    public string Number;
    public int Course;
    public StudentCard(string number, int course) { Number = number; Course = course; }
    public override string ToString() => $"билет {Number}, {Course} курс";
}
